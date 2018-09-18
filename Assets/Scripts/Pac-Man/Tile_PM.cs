using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_PM : MonoBehaviour {

    public int pNum_Tile = 0;
    public int tempPNum_Tile;
    public Sprite[] tileSprite = new Sprite[3];

    private void Awake()
    {
        tempPNum_Tile = pNum_Tile;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "PacMan")
        {
            pNum_Tile = c.GetComponent<Character_PM>().pNum;
            GetComponent<SpriteRenderer>().sprite = tileSprite[pNum_Tile];

            GameManager_PM.SetTileNum(pNum_Tile, tempPNum_Tile);

            tempPNum_Tile = pNum_Tile;
        }

    }
}
