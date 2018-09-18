using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_PM : MonoBehaviour {

    public int pNum_Tile = 0;
    public Sprite[] tileSprite = new Sprite[3];

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "PacMan")
        {
            pNum_Tile = c.GetComponent<Character_PM>().pNum;
            GetComponent<SpriteRenderer>().sprite = tileSprite[pNum_Tile];
        }
        
    }
}
