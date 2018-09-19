using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer_PM : MonoBehaviour {

    Image timerImage;
    GameObject gameEnd;


	// Use this for initialization
	void Start () {
        gameEnd = GameObject.Find("GameEnd");
        timerImage = GetComponent<Image>();

        StartCoroutine(GameTimer_PM());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GameTimer_PM()
    {
        float timer = 0f;

        gameEnd.SetActive(false);

        while(true)
        {
            if (timer >= 65f)
            {
                break;
            }

            timer += Time.deltaTime;
            timerImage.fillAmount = timer / 65f;
            yield return null;
        }

        gameEnd.SetActive(true);
        gameEnd.transform.Find("Text").GetComponent<Text>().text = "GameEnd\n" + (GameManager_PM.tileNum[1] > GameManager_PM.tileNum[2] ? "P1 WIN!\n" : "P2 WIN!\n") + "게임 종료를 위해 ESC를 눌러주세요.";
        StartCoroutine(ESCTimer());
    }

    IEnumerator ESCTimer()
    {

        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main");
            }

            yield return null;
        }
    }
}
