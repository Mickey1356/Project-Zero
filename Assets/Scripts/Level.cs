using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject wall;

    private int width = Constants.WIDTH, height = Constants.HEIGHT;

    private int[,] levelGrid;
    private List<GameObject> gos;

    private Vector2 playerSpawn, playerExit;

    // parameters
    private int buildings = 100; // more than 100 might make the game crash because it cannot generate enough buildings
    private int genLimit = 20000; // how many times to generate buildings
    private int spawnXOffset = 10, spawnYOffset = 5;
    private float distLimt = .85f; // what percentage of the furthest distance to choose a random end point from


    private void GenerateMap()
    {
        levelGrid = new int[width, height];

        GenerateBuildings(buildings);
        GenerateBounds();
        PlaceSpawnAndExit(spawnXOffset, spawnYOffset);
    }

    private void GenerateBuildings(int buildings)
    {

        int bl = 0;
        int cnt = 0;
        while (bl < buildings && cnt < genLimit)
        {
            cnt++;

            int buildingWidth, buildingHeight;
            if (Random.value < 0.5f)
            {
                buildingWidth = Random.Range(4, 14);
                buildingHeight = Random.Range(4, 7);
            }
            else
            {
                buildingWidth = Random.Range(4, 7);
                buildingHeight = Random.Range(4, 14);
            }

            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            bool valid = true;

            int spacingX = Random.Range(1, 3);
            int spacingY = Random.Range(1, 3);

            for (int dx = -spacingX; dx < buildingWidth + spacingX; dx++)
            {
                for (int dy = -spacingY; dy < buildingHeight + spacingY; dy++)
                {
                    if (!(x + dx >= 0 && x + dx < width && y + dy >= 0 && y + dy < height))
                    {
                        valid = false;
                    }
                    else if (levelGrid[x + dx, y + dy] != 0)
                    {
                        valid = false;
                    }
                }
            }

            if (valid)
            {
                bl++;
                for (int dx = 0; dx < buildingWidth; dx++)
                {
                    for (int dy = 0; dy < buildingHeight; dy++)
                    {
                        levelGrid[x + dx, y + dy] = 1;
                    }
                }
            }
        }
    }

    private void GenerateBounds()
    {
        for (int x = 0; x < width; x++)
        {
            levelGrid[x, 0] = 1;
            levelGrid[x, height - 1] = 1;
        }
        for (int y = 0; y < height; y++)
        {
            levelGrid[0, y] = 1;
            levelGrid[width - 1, y] = 1;
        }
    }

    private void PlaceSpawnAndExit(int spawnXOffset, int spawnYOffset)
    {
        List<Vector2> possibleSpawns = new List<Vector2>();

        for(int x = 1; x < width; x++)
        {
            for(int h = 0; h < spawnYOffset; h++)
            {
                if (levelGrid[x, h] == 0) possibleSpawns.Add(new Vector2(x, h));
                if (levelGrid[x, height - 2 - h] == 0) possibleSpawns.Add(new Vector2(x, height - 2 - h));
            }
        }
        for (int y = 1; y < height; y++)
        {
            for (int w = 0; w < spawnXOffset; w++)
            {
                if (levelGrid[w, y] == 0) possibleSpawns.Add(new Vector2(w, y));
                if (levelGrid[width - 2 - w, y] == 0) possibleSpawns.Add(new Vector2(width - 2 - w, y));
            }
        }

        // pick a random location for spawn
        int index = Random.Range(0, possibleSpawns.Count);
        playerSpawn = possibleSpawns[index];
        possibleSpawns.RemoveAt(index);
        //levelGrid[(int)playerSpawn.x, (int)playerSpawn.y] = 2;

        possibleSpawns = possibleSpawns.OrderBy(x => Vector2.Distance(x, playerSpawn)).ToList();
        int tlSpawns = possibleSpawns.Count;
        int minIndex = (int)(tlSpawns * distLimt);
        index = Random.Range(minIndex, tlSpawns);
        playerExit = possibleSpawns[index];
        levelGrid[(int)playerExit.x, (int)playerExit.y] = 3;
    }

    private void TestRender()
    {
        foreach(GameObject go in gos)
        {
            Destroy(go);
        }

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (levelGrid[x, y] != 0)
                {
                    GameObject go = Instantiate(wall);
                    go.transform.position = new Vector3(x, y, 0);
                    gos.Add(go);

                    switch (levelGrid[x, y])
                    {
                        case 1:
                            break;
                        case 2:
                            go.GetComponent<Renderer>().material.color = Color.red;
                            break;
                        case 3:
                            go.GetComponent<Renderer>().material.color = Color.yellow;
                            break;
                    }
                }
            }
        }
    }

    public void Initialise()
    {
        levelGrid = new int[width, height];
        gos = new List<GameObject>();
    }

    public void CreateLevel()
    {
        GenerateMap();
        TestRender();
    }

    public int[,] GetLevel()
    {
        return levelGrid;
    }

    public Vector2 GetPlayerSpawn()
    {
        return playerSpawn;
    }

    public Vector2 GetCatSpawn()
    {
        return new Vector2(1, 1);
    }
}
