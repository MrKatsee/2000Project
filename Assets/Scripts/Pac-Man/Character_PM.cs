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

    protected float speed = 7.5f;

    Rigidbody2D r;

    protected int directionNum = 4;
    protected int directionNum_Temp;

    protected int dTriggerNum = 0;

    protected bool[] dTrigger = new bool[4]; 
    protected DTrigger[] characterDTrigger = new DTrigger[4];

    protected Vector2 tempPosition;

    protected GameObject sprite;

    public int pNum;

    public float stun = 0f;

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
        //r.velocity = DirectionToVector(directionNum) * speed;


    }

    // Update is called once per frame
    protected virtual void Update () {
        if (tempPosition.x - transform.position.x >= 1f || tempPosition.y - transform.position.y >= 1f
            || tempPosition.x - transform.position.x <= -1f || tempPosition.y - transform.position.y <= -1f
            || (tempPosition.x - transform.position.x == 0f && tempPosition.y - transform.position.y == 0f))
        {
            tempPosition.x = transform.position.x >= 0f ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f;
            tempPosition.y = transform.position.y >= 0f ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f;

            ProcessPM();
        }

        if (stun > 0f)
        {
            stun -= Time.deltaTime;
            r.velocity = new Vector2(0f, 0f);
            transform.position = tempPosition;
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else if (pNum != 0)
        {
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
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
        else
        {
            dVector = new Vector2(0f, 0f);
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

        if (directionNum_Temp == 4)
        {
            r.velocity = new Vector2(0f, 0f);
            return;
        }
        else
        {

            if (characterDTrigger[directionNum_Temp].trigger == true)
            {
                r.velocity = new Vector2(0f, 0f);
                if (pNum == 0)
                {
                    if ((characterDTrigger[(directionNum_Temp + 1) % 4].trigger == true))
                    {
                        directionNum_Temp = (directionNum_Temp + 3) % 4;

                    }
                    else
                    {
                        directionNum_Temp = (directionNum_Temp + 1) % 4;

                    }
                }
            }
            else
            {
                r.velocity = DirectionToVector(directionNum) * speed;
            }
        }


    }
}
