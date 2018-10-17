using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    // AI for the big cat only

    private int width = Constants.WIDTH, height = Constants.HEIGHT;

    private int selfNode, playerNode;
    private int[,] levelGrid;

    private float speed = Constants.catSpeed;

    private GameObject player;

    // every frame OR every x seconds
    // find players current grid position
    // get self current grid position
    // path-find to player (Dijkstra? DFS? BFS?) BFS since unweighted graph

    private List<List<int>> adjList;

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
                adjList.Add(new List<int>());
                if (levelGrid[i, j] == 0)
                {
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
        int xpos = Mathf.RoundToInt(transform.position.x);
        int ypos = Mathf.RoundToInt(transform.position.y);

        return GetNode(xpos, ypos);
    }

    private int GetPlayerPos()
    {
        // temporarily set it to mouseclick pos
        int xpos = Mathf.RoundToInt(player.transform.position.x);
        int ypos = Mathf.RoundToInt(player.transform.position.y);

        return GetNode(xpos, ypos);
    }

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
        if (l == null) return;

        int targetNode = l[1];

        int xPos = targetNode / height;
        int yPos = targetNode % height;

        //this.GetComponent<Rigidbody2D>().AddForce(10 * new Vector2(xPos - this.transform.position.x, yPos - this.transform.position.y));
        //this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(xPos, yPos));
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos, yPos, 0), step);
    }

    public void Initialise(int[,] levelGrid, GameObject player)
    {
        adjList = new List<List<int>>();
        this.levelGrid = levelGrid;
        this.player = player;

        GenAdjList();
    }

    public void UpdateCat()
    {
        selfNode = GetCurrentGrid();
        playerNode = GetPlayerPos();
        Dictionary<int, int> map = PathFind();
        List<int> l = GetPath(playerNode, map);
        MoveCat(l);
    }
}
