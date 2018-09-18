using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    #region Singleton
    public static Wall instance;
    public GameObject wallR;
    public GameObject wallL;

    void Awake()
    {
        instance = this;
    }
    #endregion


}
