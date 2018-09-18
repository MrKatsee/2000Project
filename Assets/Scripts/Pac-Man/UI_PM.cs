using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PM : MonoBehaviour {

    public int pNum_UI;
    Image uiImage;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        uiImage.fillAmount = GameManager_PM.GetTileNum(pNum_UI);
	}
}
