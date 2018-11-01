using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    // AI for the big cat only

    private int width = Constants.WIDTH, height = Constants.HEIGHT;

    private int selfNode, playerNode;
    private int[,] levelGrid;

    private float speed = Constants.BIGCAT_SPEED;
    private float collision_dist = Constants.SIZE_SCALE / 4;

    private bool hitPlayer = false, gameOver = false;

    private GameObject player;

    // every frame OR every x seconds
    // find players current grid position
    // get self current grid position
    // path-find to player (Dijkstra? DFS? BFS?) BFS since unweighted graph

    private List<HashSet<int>> adjList;

    private int GetNode(int i, int j)
    {
        return i * height + j;
    }

    // GenAdjList need only to be called once
    private void GenAdjList()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // node k = i * height + j (0 -> height on the y-axis)
                int nodeK = GetNode(i, j);
                adjList.Add(new HashSet<int>());
                if (levelGrid[i, j] == 0)
                {

                    //for(int dx = -1; dx < 2; dx++)
                    //{
                    //    for(int dy = -1; dy < 2; dy++)
                    //    {
                    //        if((dx + i >= 0 || dx + i < width || dy + j < height || dy + j >= 0) && !(dx == 0 && dy == 0))
                    //        {
                    //            if (levelGrid[dx + i, dy + j] == 0) adjList[nodeK].Add(GetNode(i + dx, j + dy));
                    //        }
                    //    }
                    //}

                    if (i < width - 1)
                    {
                        if (levelGrid[i + 1, j] == 0)
                        {
                            adjList[nodeK].Add(GetNode(i + 1, j));
                        }
                    }
                    if (i > 0)
                    {
                        if (levelGrid[i - 1, j] == 0)
                        {
                            adjList[nodeK].Add(GetNode(i - 1, j));
                        }
                    }
                    if (j < height - 1)
                    {
                        if (levelGrid[i, j + 1] == 0)
                        {
                            adjList[nodeK].Add(GetNode(i, j + 1));
                        }
                    }
                    if (j > 0)
                    {
                        if (levelGrid[i, j - 1] == 0)
                        {
                            adjList[nodeK].Add(GetNode(i, j - 1));
                        }
                    }
                }

            }
        }
    }

    private int GetCurrentGrid()
    {
        int xpos = Mathf.RoundToInt(transform.position.x / Constants.SIZE_SCALE);
        int ypos = Mathf.RoundToInt(transform.position.y / Constants.SIZE_SCALE);

        return GetNode(xpos, ypos);
    }

    private int GetPlayerPos()
    {
        // temporarily set it to mouseclick pos
        int xpos = Mathf.RoundToInt(player.transform.position.x / Constants.SIZE_SCALE);
        int ypos = Mathf.RoundToInt(player.transform.position.y / Constants.SIZE_SCALE);

        return GetNode(xpos, ypos);
    }

    private float prevX;
    private bool facingRight = true;
    private Vector3 rightScale;

    private Dictionary<int, int> PathFind()
    {
        // Classic BFS

        Dictionary<int, int> map = new Dictionary<int, int>();

        HashSet<int> visited = new HashSet<int>();
        Queue<int> queue = new Queue<int>();

        visited.Add(selfNode);
        map[selfNode] = -1;

        foreach (int node in adjList[selfNode])
        {
            visited.Add(node);
            queue.Enqueue(node);
            map[node] = selfNode;
        }
        while (queue.Count > 0)
        {
            int curNode = queue.Dequeue();
            if (curNode == playerNode)
            {
                return map;
            }
            foreach (int node in adjList[curNode])
            {
                if(!visited.Contains(node))
                {
                    visited.Add(node);
                    queue.Enqueue(node);
                    map[node] = curNode;
                }
            }
        }
        return null;
    }

    private List<int> GetPath(int endNode, Dictionary<int, int> map)
    {
        List<int> l = new List<int>();

        if (map == null) return null;

        l.Add(endNode);
        int next = map[endNode];

        while(next!=-1)
        {
            l.Add(next);
            next = map[next];
        }

        l.Reverse();

        return l;
    }

    private void MoveCat(List<int> l)
    {
        float xPos = 0f, yPos = 0f;
        bool useGrid = false;

        if (l != null)
        {
            int targetNode = l[1];

            xPos = (targetNode / height) * Constants.SIZE_SCALE;
            yPos = (targetNode % height) * Constants.SIZE_SCALE;
        }
        float step = speed * Time.deltaTime;

        //Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, .2f, 0), new Vector3(3f, 2f, 1f), 0f);
        //foreach(var col in colls)
        //{
        //    //Debug.Log(col);
        //    if (col.tag == "Wall")
        //    {
        //        useGrid = true;
        //        break;
        //    }
        //}

        Vector3 _min = GetComponent<BoxCollider2D>().bounds.min;
        Vector3 _max = GetComponent<BoxCollider2D>().bounds.max;

        Vector3 pos1 = new Vector2(_min.x, _min.y);
        Vector3 pos2 = new Vector2(_min.x, _max.y);
        Vector3 pos3 = new Vector2(_max.x, _min.y);
        Vector3 pos4 = new Vector2(_max.x, _max.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        RaycastHit2D hit1 = Physics2D.Raycast(pos1, player.transform.position - pos1);
        RaycastHit2D hit2 = Physics2D.Raycast(pos2, player.transform.position - pos2);
        RaycastHit2D hit3 = Physics2D.Raycast(pos3, player.transform.position - pos3);
        RaycastHit2D hit4 = Physics2D.Raycast(pos4, player.transform.position - pos4);

        Debug.DrawRay(pos1, player.transform.position - pos1);
        Debug.DrawRay(pos2, player.transform.position - pos2);
        Debug.DrawRay(pos3, player.transform.position - pos3);
        Debug.DrawRay(pos4, player.transform.position - pos4);

        string tag1 = hit1.collider.tag;
        string tag2 = hit2.collider.tag;
        string tag3 = hit3.collider.tag;
        string tag4 = hit4.collider.tag;

        if ((l == null || hit.collider.tag == "Player") && !useGrid && tag1 == "Player" && tag2 == "Player" && tag3 == "Player" && tag4 == "Player")
        {
            //Debug.Log(step);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos, yPos, 0), step);
        }

        //this.GetComponent<Rigidbody2D>().AddForce(10 * new Vector2(xPos - this.transform.position.x, yPos - this.transform.position.y));
        //this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(xPos, yPos));
    }

    private void DebugMap(int target, Dictionary<int, int> map)
    {
        int next = map[target];
        Debug.Log(target);
        for(int i = 0; i < 500; i++)
        {
            Debug.Log(next);
            if (map.ContainsKey(next))
            {
                next = map[next];
            }
            else break;
        }
    }

    public void GameOver()
    {
        Debug.Log("i have hit the dog");
        hitPlayer = true;
        GetComponent<Animator>().SetBool("moving", false);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.tag == "Player")
    //    {
    //        GameOver();
    //    }
    //}

    public void Initialise(int[,] levelGrid, GameObject player)
    {
        adjList = new List<HashSet<int>>();
        this.levelGrid = levelGrid;
        this.player = player;
        this.prevX = transform.position.x;
        rightScale = transform.localScale;

        GetComponent<Animator>().SetBool("moving", true);

        GenAdjList();
    }

    public void UpdateCat()
    {
        if (!gameOver && !hitPlayer)
        {
            selfNode = GetCurrentGrid();
            playerNode = GetPlayerPos();
            Dictionary<int, int> map = PathFind();
            //DebugMap(playerNode, map);
            List<int> l = GetPath(playerNode, map);
            MoveCat(l);

            if (prevX < transform.position.x)
            {
                facingRight = true;
            }
            else
            {
                facingRight = false;
            }

            if (facingRight)
            {
                transform.localScale = rightScale;
            }
            else
            {
                transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);
            }
            prevX = transform.position.x;

            if(Vector2.Distance(transform.position, player.transform.position) < collision_dist)
            {
                gameOver = true;
            }
        }
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }
}
