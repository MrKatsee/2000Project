using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    #region Singleton
    public static Wall instance;
    public GameObject wall1;
    public GameObject wall2;

    void Awake()
    {
        instance = this;
    }
    #endregion


}
