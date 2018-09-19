using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChecker : MonoBehaviour {

    float x, y;
    GameObject tile;
    GameObject tile_Temp;

	// Use this for initialization
	void Start () {
        x = -16.5f;
        y = 5.5f;

        tile = Resources.Load("Pac-Man/Prefabs/Tile_Floor") as GameObject;

        for (x = -16.5f; x <= 16.5f; x += 1f)
        {
            for (y = 5.5f; y >= -8.5f; y -= 1f)
            {
                if (Physics2D.OverlapPoint(new Vector2(x, y)) == null)
                {
                    tile_Temp = Instantiate(tile, new Vector3(x, y, 1), Quaternion.identity);
                }
                
            }
        }


        Instantiate(Resources.Load("Pac-Man/Prefabs/PacMan_1") as GameObject, new Vector3(-9.5f, -0.5f, 0f), Quaternion.identity);
        Instantiate(Resources.Load("Pac-Man/Prefabs/PacMan_2") as GameObject, new Vector3(9.5f, -0.5f, 0f), Quaternion.identity);

        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
