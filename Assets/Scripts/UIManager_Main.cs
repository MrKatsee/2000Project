using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_Main : MonoBehaviour {

    int selectIndex = 0;
    int Index
    {
        get
        {
            return selectIndex;
        }
        set
        {
            selectIndex = value;
            switch (selectIndex) 
            {
                case 1://Gallag
                    gallagButton.GetComponent<Image>().sprite = buttonSprite[1];
                    snakeButton.GetComponent<Image>().sprite = buttonSprite[2];
                    packManButton.GetComponent<Image>().sprite = buttonSprite[4];

                    break;
                case 2://Snake
                    gallagButton.GetComponent<Image>().sprite = buttonSprite[0];
                    snakeButton.GetComponent<Image>().sprite = buttonSprite[3];
                    packManButton.GetComponent<Image>().sprite = buttonSprite[4];
                    break;
                case 3://Pac-Man
                    gallagButton.GetComponent<Image>().sprite = buttonSprite[0];
                    snakeButton.GetComponent<Image>().sprite = buttonSprite[2];
                    packManButton.GetComponent<Image>().sprite = buttonSprite[5];
                    break;
            }
        }
    }
    float fadeOutTime = 1f;

    public GameObject packManButton;
    public GameObject snakeButton;
    public GameObject gallagButton;
    public RawImage fadeOutEffect;
    public AudioSource audioSource;
    public AudioClip BGM;
    public AudioClip SFX;
    public Sprite[] buttonSprite = new Sprite[6];

    private void Start()
    {

        fadeOutEffect.color = Color.clear;
        if (audioSource && BGM)
        {
            audioSource.clip = BGM;
            audioSource.loop = true;
            audioSource.Play();
        }
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        if(audioSource && SFX) audioSource.PlayOneShot(SFX);
        Index = 1;
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (audioSource && SFX) audioSource.PlayOneShot(SFX);
                Index++;
                if (Index > 3) Index = 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (audioSource && SFX) audioSource.PlayOneShot(SFX);
                Index--;
                if (Index < 1) Index = 3;
            }
            yield return null;
        }
        if (audioSource && SFX) audioSource.PlayOneShot(SFX);
        float nowTime = 0;
        float alpha = 0;
        fadeOutEffect.color = new Color(0, 0, 0, alpha);
        float volume = audioSource.volume;
        while (nowTime < fadeOutTime)
        {
            alpha += Time.deltaTime;
            audioSource.volume -= volume * Time.deltaTime;
            fadeOutEffect.color = new Color(0, 0, 0, alpha);
            nowTime += Time.deltaTime;
            yield return null;
        }
        fadeOutEffect.color = Color.black;
        audioSource.Stop();

        switch (Index)
        {
            case 1://Gallag
                SceneManager.LoadScene("Gallag");
                break;
            case 2://Snake
                SceneManager.LoadScene("Snake");
                break;
            case 3://Pac-Man
                SceneManager.LoadScene("Pac-Man");
                break;
        }
    }

}
