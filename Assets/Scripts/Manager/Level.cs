﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject wall, ground;

    private int width = Constants.WIDTH, height = Constants.HEIGHT;

    private int[,] levelGrid;
    private List<GameObject> gos;

    private Vector2 playerSpawn, playerExit, catSpawn;

    private List<Vector2> sCatSpawns;

    // parameters
    private int buildings = 100; // more than 100 might make the game crash because it cannot generate enough buildings
    private int genLimit = 20000; // how many times to try to generate buildings
    private int spawnXOffset = 10, spawnYOffset = 5;
    private float distLimt = .85f; // what percentage of the furthest distance to choose a random end point from
    private float smallCats = .025f; // what percentage of empty space should be small cat spawn

    private float catMin = 0.3f;
    private float catMax = 0.4f;
    private float sCatMin = 0.05f;

    private void GenerateMap()
    {
        levelGrid = new int[width, height];
        sCatSpawns = new List<Vector2>();

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

        for (int x = 1; x < width; x++)
        {
            for (int h = 0; h < spawnYOffset; h++)
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

        int catMinIndex = (int)(tlSpawns * catMin);
        int catMaxIndex = (int)(tlSpawns * catMax);
        index = Random.Range(catMinIndex, catMaxIndex);
        catSpawn = possibleSpawns[index];
        //levelGrid[(int)catSpawn.x, (int)catSpawn.y] = 4;

        List<Vector2> sCatPossibleSpawns = new List<Vector2>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (levelGrid[i, j] == 0) sCatPossibleSpawns.Add(new Vector2(i, j));
            }
        }

        sCatPossibleSpawns = sCatPossibleSpawns.OrderBy(x => Vector2.Distance(x, playerSpawn)).ToList();

        // pick random locations for small cat spawns
        sCatPossibleSpawns.Remove(catSpawn);
        sCatPossibleSpawns.Remove(playerExit);
        tlSpawns = sCatPossibleSpawns.Count;
        int nCats = (int)(tlSpawns * smallCats);
        Debug.Log(nCats);
        for (int i = 0; i < nCats; i++)
        {
            tlSpawns = sCatPossibleSpawns.Count;
            int sCatMinIndex = (int)(tlSpawns * sCatMin);
            index = Random.Range(sCatMinIndex, tlSpawns);
            Vector2 spawn = sCatPossibleSpawns[index];
            sCatSpawns.Add(spawn);
            // temporary put green thing to see
            //levelGrid[(int)spawn.x, (int)spawn.y] = 4;
            sCatPossibleSpawns.Remove(spawn);
        }

        // make walls that are not touching outside black
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bool sur = true;
                if (levelGrid[i, j] == 1 || levelGrid[i, j] == 5)
                {
                    for (int dx = -1; dx < 2; dx++)
                    {
                        for (int dy = -1; dy < 2; dy++)
                        {
                            if (i + dx >= 0 && i + dx < width && j + dy >= 0 && j + dy < height && !(dx == 0 && dy == 0))
                            {
                                if (levelGrid[i + dx, j + dy] == 0 || levelGrid[i+dx, j+dy] == 3)
                                {
                                    sur = false;
                                }
                            }
                        }
                    }
                    if (sur) levelGrid[i, j] = 5;
                }
            }
        }

    }

    private void TestRender()
    {
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (levelGrid[x, y] != 0)
                {
                    GameObject go = Instantiate(wall);
                    go.transform.position = new Vector3(x * Constants.SIZE_SCALE, y * Constants.SIZE_SCALE, 0);
                    gos.Add(go);

                    switch (levelGrid[x, y])
                    {
                        case 1:
                            break;
                        //case 2:
                        //    go.GetComponent<Renderer>().material.color = Color.red;
                        //    break;
                        case 3:
                            go.GetComponent<Renderer>().material.color = Color.yellow;
                            Destroy(go.GetComponent<BoxCollider2D>());
                            break;
                        case 4:
                            go.GetComponent<Renderer>().material.color = Color.green;
                            Destroy(go.GetComponent<BoxCollider2D>());
                            break;
                        case 5:
                            go.GetComponent<Renderer>().material.color = Color.black;
                            break;
                    }
                }
                else
                {
                    GameObject go = Instantiate(ground);
                    go.transform.position = new Vector3(x * Constants.SIZE_SCALE, y * Constants.SIZE_SCALE, 2);
                }
            }
        }
    }

    public void Initialise()
    {
        wall.transform.localScale = Constants.SIZE_SCALE * Vector3.one;
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
        return playerSpawn * Constants.SIZE_SCALE;
    }

    public Vector2 GetCatSpawn()
    {
        return catSpawn * Constants.SIZE_SCALE;
    }

    public Vector2 GetExit()
    {
        return playerExit * Constants.SIZE_SCALE;
    }

    public List<Vector2> GetSmallCatSpawns()
    {
        return sCatSpawns;
    }
}
