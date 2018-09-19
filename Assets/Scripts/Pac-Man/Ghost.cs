using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "PacMan")
        {
            if(c.GetComponent<Character_PM>().pNum != 0 && c.GetComponent<Character_PM>().stun <= 0f)
            {
                c.GetComponent<Character_PM>().stun = 3f;
            }
        }
    }
}
