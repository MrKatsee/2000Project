using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_EnemyAI : MonoBehaviour {
    float timer = 0;
    public Vector2 HomePos;
    float OriginX;
    float OriginY;
    float w;
    float h;

	// Use this for initialization
	void Start ()
    {
        OriginX = transform.position.x;
        OriginY = transform.position.y;
        w = 1;
        h = 1;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timer += 3f*Time.deltaTime;
        MoveCircle();
    }

    private void MoveCircle()
    {
        


        transform.position = new Vector2(OriginX + w*Mathf.Cos(timer),OriginY+h*Mathf.Sin(timer));


    }
}
