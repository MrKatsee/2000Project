using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : Character_PM {

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.W))
        {
            directionNum_Temp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            directionNum_Temp = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            directionNum_Temp = 2;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            directionNum_Temp = 3;
        }
    }
}
