using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class J_GameManager : MonoBehaviour {
    public static int score=0;
    public Text scoreText;
    public GameObject Win;

	// Use this for initialization
	void Start () {
        Win.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        scoreText.text = "Score : "+score.ToString();
        
    }
    private void FixedUpdate()
    {
        if(score == 34)
        {
            Debug.Log("Winner");
            Win.SetActive (true);
        }
    }
}
