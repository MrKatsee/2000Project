using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_PM : MonoBehaviour {

    /*
     * 방향 숫자입니당.
     * Up : 0
     * Rihgt : 1
     * Down : 2
     * Left : 3
     */

    protected float speed = 10f;

    Rigidbody2D r;

    protected int directionNum = 1;
    protected int directionNum_Temp;

    protected bool[] dTrigger = new bool[4]; 
    protected DTrigger[] characterDTrigger = new DTrigger[4];

    protected Vector2 tempPosition;

    private void Awake()
    {
        tempPosition.x = (int)transform.position.x + 0.5f;
        tempPosition.y = (int)transform.position.y + 0.5f;

        r = GetComponent<Rigidbody2D>();

        directionNum_Temp = directionNum;

        characterDTrigger[0] = transform.Find("PMT_Up").GetComponent<DTrigger>();
        characterDTrigger[1] = transform.Find("PMT_Right").GetComponent<DTrigger>();
        characterDTrigger[2] = transform.Find("PMT_Down").GetComponent<DTrigger>();
        characterDTrigger[3] = transform.Find("PMT_Left").GetComponent<DTrigger>();
    }

    // Use this for initialization
    protected virtual void Start () {
        r.velocity = DirectionToVector(directionNum) * speed;
	}

    // Update is called once per frame
    protected virtual void Update () {
        if (tempPosition.x - transform.position.x >= 1f || tempPosition.y - transform.position.y >= 1f
            || tempPosition.x - transform.position.x <= -1f || tempPosition.y - transform.position.y <= -1f
            || (tempPosition.x - transform.position.x == 0f && tempPosition.y - transform.position.y == 0f))
        {
            tempPosition.x = transform.position.x >= 0f ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f;
            tempPosition.y = transform.position.y >= 0f ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f;

            Debug.Log(tempPosition);
            Debug.Log(transform.position);

            ProcessPM();
        }
	}

    protected Vector2 DirectionToVector(int dNum)
    {
        Vector2 dVector = new Vector2 (1f, 0f);

        if (dNum == 0)
        {
            dVector = new Vector2(0f, 1f);
        }
        else if (dNum == 1)
        {
            dVector = new Vector2(1f, 0f);
        }
        else if (dNum == 2)
        {
            dVector = new Vector2(0f, -1f);
        }
        else if (dNum == 3)
        {
            dVector = new Vector2(-1f, 0f);
        }

        return dVector;
    }

    protected virtual void ProcessPM()
    {
        transform.position = tempPosition;
        /*
        if (characterDTrigger[directionNum_Temp].trigger == false)
        {
            directionNum = directionNum_Temp;
        }
        //이게 해당 방향에 벽이 없을 경우, 방향을 변경한다.
        */
        directionNum = directionNum_Temp; //벽이 있을 때도 방향 변경 가능

        directionNum_Temp = directionNum;

        if (characterDTrigger[directionNum_Temp].trigger == true)
        {
            r.velocity = new Vector2(0f, 0f);
        }
        else
        {
            r.velocity = DirectionToVector(directionNum) * speed;
        }

    }
}
