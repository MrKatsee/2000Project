﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTrigger : MonoBehaviour {

    public bool trigger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.name == "Tile_Wall")
        {
            trigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "Tile_Wall")
        {
            trigger = true;
        }
    }

}
