using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_PM : MonoBehaviour {

    public static int[] tileNum = new int[3];

    void Awake()
    {
        tileNum[0] = 100;
        tileNum[1] = 1;
        tileNum[2] = 1;

        float y = 5.5f;
        float x = -16.5f;
        GameObject tile_Temp;


        for (y = 5.5f; y >= -8.5f; y -= 1f)
        {
            if (y == 5.5f || y == -8.5f)
            {
                for (x = -16.5f; x <= 16.5f; x += 1f)
                {
                    tile_Temp = Instantiate(Resources.Load("Pac-Man/Prefabs/Tile_Wall") as GameObject, new Vector2(x, y), Quaternion.identity);
                    tile_Temp.transform.parent = GameObject.Find("Tile").transform;
                    tile_Temp.name = "Tile_Wall";
                }
            }

            else
            {
                tile_Temp = Instantiate(Resources.Load("Pac-Man/Prefabs/Tile_Wall") as GameObject, new Vector2(-16.5f, y), Quaternion.identity);
                tile_Temp.transform.parent = GameObject.Find("Tile").transform;
                tile_Temp.name = "Tile_Wall";
                tile_Temp = Instantiate(Resources.Load("Pac-Man/Prefabs/Tile_Wall") as GameObject, new Vector2(16.5f, y), Quaternion.identity);
                tile_Temp.transform.parent = GameObject.Find("Tile").transform;
                tile_Temp.name = "Tile_Wall";
            }
        }

        Instantiate(Resources.Load("Pac-Man/Prefabs/TileChecker") as GameObject, new Vector3 (100f, 100f, 0f), Quaternion.identity);
        StartCoroutine(GameTimer_PM());
    }

    private void Update()
    {

    }

    public static void SetTileNum(int pNum, int pNum_Temp)
    {
        tileNum[pNum] += 1;
        tileNum[pNum_Temp] -= 1;
    }

    public static float GetTileNum(int pNum)
    {
        float value = 0f;
        int oNum = 0;

        if (pNum == 1)
        {
            oNum = 2;
        }else if (pNum == 2)
        {
            oNum = 1;
        }

        value = (float)tileNum[pNum] / ((float)tileNum[pNum] + (float)tileNum[oNum]);


        return value;
    }

    IEnumerator GameTimer_PM()
    {

        yield return new WaitForSeconds(15f);

        Instantiate(Resources.Load("Pac-Man/Prefabs/Ghost2") as GameObject, new Vector3(-0.5f, -4.5f, 0f), Quaternion.identity);

        yield return new WaitForSeconds(15f);

        Instantiate(Resources.Load("Pac-Man/Prefabs/Ghost3") as GameObject, new Vector3(-0.5f, -4.5f, 0f), Quaternion.identity);

    }
}
