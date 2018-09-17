using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_PlayerManager : MonoBehaviour {

    #region Singleton
    public static J_PlayerManager instance;
    public GameObject player;

    void Awake()
    {
        instance = this;
    }
    #endregion
}
