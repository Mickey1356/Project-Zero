using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private int width = 128, height = 64;

    private int[,] levelGrid;
    private List<GameObject> gos;

    // parameters
    private int buildings = 100; // more than 100 might make the game crash
    private int spawnXOffset = 10, spawnYOffset = 5;


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
        while (bl < buildings)
        {
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
        Vector2 spawnLoc = possibleSpawns[index];
        levelGrid[(int)spawnLoc.x, (int)spawnLoc.y] = 2;
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
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(x, y, 0);
                    gos.Add(go);

                    switch (levelGrid[x, y])
                    {
                        case 1:
                            break;
                        case 2:
                            go.GetComponent<Renderer>().material.color = Color.red;
                            break;
                    }
                }
            }
        }
    }

    private void RenderSpawns()
    {

    }

    private void Awake()
    {
        levelGrid = new int[width, height];
        gos = new List<GameObject>();
    }

    private void Start()
    {
        GenerateMap();
        TestRender();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GenerateMap();
            TestRender();
        }
    }
}
