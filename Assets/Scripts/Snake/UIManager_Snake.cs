using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_Snake : MonoBehaviour {

    static UIManager_Snake uiManager = null;
    static Vector2 screenSize = Vector2.zero;

    public static Vector2 TopLeftPos
    {
        get
        {
            if (isInitalized)
            {
                return Camera.main.ScreenToWorldPoint(uiManager.topLeftPos);
            }
            else return default(Vector2);
        }
    }
    public static Vector2 BotRightPos
    {
        get
        {
            if (isInitalized)
            {
                return Camera.main.ScreenToWorldPoint(uiManager.botRightPos);
            }
            else return default(Vector2);
        }
    }

    static bool isInitalized = false;
    public static bool IsInitalized
    {
        get
        {
            if (!isInitalized)
            {
                Init();
                return false;
            }
            return isInitalized;
        }
    }

    public static void Init()
    {
        if (!isInitalized)
        {
            if(uiManager == null)
            {
                if(GameObject.Find("Canvas"))
                    uiManager = GameObject.Find("Canvas").AddComponent<UIManager_Snake>();
                else
                    uiManager = new GameObject("Canvas").AddComponent<UIManager_Snake>();
                uiManager.InitUI();
                isInitalized = true;
            }
        }
    }

    public static void SetRawImage(Vector2 size)
    {
        uiManager.rawImage = new GameObject("Test").AddComponent<RawImage>();
        uiManager.rawImage.rectTransform.parent = uiManager.transform;
        uiManager.rawImage.rectTransform.pivot = new Vector2(0, 1);
        uiManager.rawImage.rectTransform.anchorMin = new Vector2(0, 0);
        uiManager.rawImage.rectTransform.anchorMax = new Vector2(0, 0);
        uiManager.rawImage.rectTransform.sizeDelta = size;
    }

    public static void EndUI(bool isAlive, int score)
    {
        if (isInitalized)
        {
            uiManager.StartCoroutine(uiManager.EndUIAnimation(isAlive, score));
        }
    }

    public static void StartUI(bool isActive)
    {
        if (isInitalized)
        {
            if (isActive)
                uiManager.StartUI();
            else
                uiManager.RemoveStartUI();
        }
    }

    public static void SetScoreUI(int score)
    {
        if (isInitalized)
        {
            uiManager.SetScore(score);
        }
    }

    public static void PlayEffect()
    {
        if (isInitalized)
        {
            uiManager.PlayEffect(uiManager.ATE, 0.8f);
        }
    }


    Vector2 topLeftPos;
    Vector2 botRightPos;

    RawImage rawImage;

    Font myFont;

    void InitUI()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        RectTransform upImageTransform = transform.Find("UpImage").GetComponent<RectTransform>();
        RectTransform downImageTransform = transform.Find("DownImage").GetComponent<RectTransform>();
        RectTransform leftImageTransform = transform.Find("LeftImage").GetComponent<RectTransform>();
        RectTransform rightImageTransform = transform.Find("RightImage").GetComponent<RectTransform>();
        RectTransform cienTransform = transform.Find("CienImage").GetComponent<RectTransform>();
        RectTransform snakeTransform = transform.Find("SnakeImage").GetComponent<RectTransform>();
        Vector2 updownSizeVector = new Vector2(screenSize.x, screenSize.y / 18);
        upImageTransform.sizeDelta = updownSizeVector;
        downImageTransform.sizeDelta = updownSizeVector;
        float gameScreenSizeValue = screenSize.y / 18 * 16;
        leftImageTransform.sizeDelta = new Vector2((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y);
        rightImageTransform.sizeDelta = new Vector2((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y);
        topLeftPos = new Vector3((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y / 18 * 17);
        botRightPos = new Vector3((screenSize.x - gameScreenSizeValue) * .5f + gameScreenSizeValue, screenSize.y - (screenSize.y / 18 * 17));
        cienTransform.sizeDelta = new Vector2(screenSize.x / 12, screenSize.y / 2);
        cienTransform.position = new Vector2(screenSize.x / 10 * 1, screenSize.y / 15 * 14);
        snakeTransform.sizeDelta = new Vector2(screenSize.x / 12, screenSize.y / 3 * 2);
        snakeTransform.position = new Vector2(screenSize.x / 10 * 9, screenSize.y / 15 * 14);
        Debug.Log(cienTransform);
        myFont = Resources.Load<Font>("Fonts/DungGeunMo");
        BGM = Resources.Load<AudioClip>("Sounds/SnakeBGM");
        WIN = Resources.Load<AudioClip>("Sounds/SnakeWin");
        ATE = Resources.Load<AudioClip>("Sounds/SnakeAte");
        OOPS = Resources.Load<AudioClip>("Sounds/SnakeOops");
        LOSE = Resources.Load<AudioClip>("Sounds/SnakeLose");
        audioSource = GetComponent<AudioSource>();
    }

    Text startText;
    Text scoreText;

    AudioClip BGM;
    AudioClip WIN;
    AudioClip ATE;
    AudioClip LOSE;
    AudioClip OOPS;

    AudioSource audioSource;

    void StartUI()
    {
        startText = new GameObject("StartText").AddComponent<Text>();
        startText.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        startText.font = myFont;
        startText.resizeTextForBestFit = true;
        startText.alignment = TextAnchor.MiddleCenter;
        startText.resizeTextMinSize = 0;
        startText.resizeTextMaxSize = 500;
        startText.color = Color.white;
        startText.text = "←, ↑, → 중 하나를 눌러\n\n게임을 시작하세요.";
        startText.rectTransform.parent = transform;
        startText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y *.5f);
        startText.rectTransform.localPosition = new Vector2(0, 0);
        audioSource.clip = BGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void RemoveStartUI()
    {
        if (startText)
        {
            Destroy(startText.gameObject);
            startText = null;
        }
        scoreText = new GameObject("ScoreText").AddComponent<Text>();
        scoreText.gameObject.AddComponent<Outline>();
        scoreText.font = myFont;
        scoreText.resizeTextForBestFit = true;
        scoreText.alignment = TextAnchor.MiddleLeft;
        scoreText.resizeTextMinSize = 0;
        scoreText.resizeTextMaxSize = 500;
        scoreText.color = Color.white;
        scoreText.text = "   Score : 0";
        scoreText.rectTransform.parent = transform;
        scoreText.rectTransform.anchorMin = new Vector2(0, 1);
        scoreText.rectTransform.anchorMax = new Vector2(0, 1);
        scoreText.rectTransform.pivot = new Vector2(0, 0);
        scoreText.rectTransform.sizeDelta = new Vector2((screenSize.x - (screenSize.y / 18 * 16)) * .5f, screenSize.y / 18);
        scoreText.rectTransform.position = botRightPos;
    }

    void SetScore(int score)
    {
        if (scoreText)
        {
            scoreText.text = "   Score : " + score;
        }
    }

    void PlayEffect(AudioClip clip, float volume)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    IEnumerator EndUIAnimation(bool isAlive, int score)
    {
        if (!rawImage) yield break;
        audioSource.Stop();
        if (!isAlive) PlayEffect(OOPS, 1f);
        yield return new WaitForSeconds(0.9f);
        RectTransform cienTransform = transform.Find("CienImage").GetComponent<RectTransform>();
        RectTransform snakeTransform = transform.Find("SnakeImage").GetComponent<RectTransform>();
        Destroy(cienTransform.gameObject);
        Destroy(snakeTransform.gameObject);
        int length = PlayManager_Snake.Length;

        float delayTime = 0.6f / length;
        for (int y = 0; y < length; ++y)
        {
            for (int x = 0; x < length; ++x)
            {
                RawImage newObject = Instantiate<RawImage>(rawImage, transform);
                newObject.rectTransform.position = new Vector2(topLeftPos.x + (x * newObject.rectTransform.sizeDelta.x), topLeftPos.y - (y * newObject.rectTransform.sizeDelta.y));
            }
            yield return new WaitForSeconds(delayTime);
        }
        yield return new WaitForSeconds(delayTime);
        Destroy(uiManager.scoreText.gameObject);
        if (isAlive)
        {
            PlayEffect(WIN, 0.8f);
        }
        else
        {
            PlayEffect(LOSE, 0.8f);
        }

        Text winlostText = new GameObject("WinLostText").AddComponent<Text>();
        winlostText.font = myFont;
        winlostText.resizeTextForBestFit = true;
        winlostText.alignment = TextAnchor.MiddleCenter;
        winlostText.resizeTextMinSize = 0;
        winlostText.resizeTextMaxSize = 500;
        if (isAlive) winlostText.text = "Victory!";
        else winlostText.text = "Game Over!";
        winlostText.color = Color.black;
        winlostText.rectTransform.parent = transform;
        winlostText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y / 10 * 3);
        winlostText.rectTransform.localPosition = new Vector2(0, screenSize.y / 5 * 1);

        Text scoreText = new GameObject("ScoreText").AddComponent<Text>();
        scoreText.font = myFont;
        scoreText.resizeTextForBestFit = true;
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.resizeTextMinSize = 0;
        scoreText.resizeTextMaxSize = 500;

        scoreText.text = "Score : " + score;
        scoreText.color = Color.black;
        scoreText.rectTransform.parent = transform;
        scoreText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y / 10 * 1);
        scoreText.rectTransform.localPosition = new Vector2(0, 0);

        Text noticeText = new GameObject("NoticeText").AddComponent<Text>();
        noticeText.font = myFont;
        noticeText.resizeTextForBestFit = true;
        noticeText.alignment = TextAnchor.MiddleCenter;
        noticeText.resizeTextMinSize = 0;
        noticeText.resizeTextMaxSize = 500;
        noticeText.text = "아무 키나 눌러 재도전 하세요.\nESC키를 누르면\n메인 화면으로 돌아갑니다.";
        noticeText.color = Color.black;
        noticeText.rectTransform.parent = transform;
        noticeText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y / 10 * 3);
        noticeText.rectTransform.localPosition = new Vector2(0, screenSize.y / 4 * -1);

        yield return new WaitUntil(() => Input.anyKeyDown);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void OnDestroy()
    {
        isInitalized = false;
        uiManager = null;
    }

}
