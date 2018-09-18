using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : Character_PM {

    float rd;
    GameObject sprite;

    protected override void Start()
    {
        base.Start();

        if (pNum == 0)
        {

        }
        else
        {
            sprite = transform.Find("Sprite").gameObject;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (pNum == 1)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                directionNum_Temp = 0;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 90f);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                directionNum_Temp = 1;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                directionNum_Temp = 2;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, -90f);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                directionNum_Temp = 3;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 180f);
                sprite.transform.Rotate(Vector3.right, 180f);
            }
        }


        else if (pNum == 2)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                directionNum_Temp = 0;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 90f);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                directionNum_Temp = 1;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                directionNum_Temp = 2;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, -90f);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                directionNum_Temp = 3;
                sprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                sprite.transform.Rotate(Vector3.forward, 180f);
                sprite.transform.Rotate(Vector3.right, 180f);
            }
        }

        else if (pNum == 0)
        {
            dTriggerNum = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (characterDTrigger[i].trigger == false)
                {
                    dTriggerNum += 1;
                }
            }

            rd = Random.Range(0f, 9.99f);

            if (rd >= 0f && rd <= 0.5f)
            {
                
            }
            else if (rd > 0.5f && rd <= 0.75f)
            {
                directionNum_Temp = (directionNum_Temp + 1) % 4;
            }
            else if (rd > 0.75f && rd <= 0.99f)
            {
                directionNum_Temp = (directionNum_Temp + 3) % 4;
            }
            /*
            if (dTriggerNum == 3)
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
       
            if (dTriggerNum == 2 && (characterDTrigger[directionNum_Temp].trigger == true))
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
                 */
        }
    }
}
