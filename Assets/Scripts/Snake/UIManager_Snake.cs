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

    public static void EndUI(bool isAlive)
    {
        if (isInitalized)
        {
            uiManager.StartCoroutine(uiManager.EndUIAnimation(isAlive));
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
        Vector2 updownSizeVector = new Vector2(screenSize.x, screenSize.y / 18);
        upImageTransform.sizeDelta = updownSizeVector;
        downImageTransform.sizeDelta = updownSizeVector;
        float gameScreenSizeValue = screenSize.y / 18 * 16;
        leftImageTransform.sizeDelta = new Vector2((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y);
        rightImageTransform.sizeDelta = new Vector2((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y);
        topLeftPos = new Vector3((screenSize.x - gameScreenSizeValue) * .5f, screenSize.y / 18 * 17);
        botRightPos = new Vector3((screenSize.x - gameScreenSizeValue) * .5f + gameScreenSizeValue, screenSize.y - (screenSize.y / 18 * 17));
        myFont = Resources.Load<Font>("Fonts/NanumBarunGothic");
    }

    Text startText;

    void StartUI()
    {
        startText = new GameObject("StartText").AddComponent<Text>();
        startText.gameObject.AddComponent<Outline>();
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
    }

    void RemoveStartUI()
    {
        if (startText)
        {
            Destroy(startText.gameObject);
            startText = null;
        }
    }

    IEnumerator EndUIAnimation(bool isAlive)
    {
        if (!rawImage) yield break;
        yield return new WaitForSeconds(1.5f);
        int length = PlayManager_Snake.Length;
        float delayTime = 1f / (length * length);
        float time = 0f;
        for(int x = 0; x < length; ++x)
        {
            for (int y = 0; y < length; ++y)
            {
                time += Time.deltaTime;
                yield return null;
                if (time >= delayTime)
                {
                    RawImage newObject = Instantiate<RawImage>(rawImage, transform);
                    newObject.rectTransform.position = new Vector2(topLeftPos.x + (x * newObject.rectTransform.sizeDelta.x), topLeftPos.y - (y * newObject.rectTransform.sizeDelta.y));
                    time = 0;
                }
            }
        }

        Text winlostText = new GameObject("WinLostText").AddComponent<Text>();
        winlostText.font = myFont;
        winlostText.resizeTextForBestFit = true;
        winlostText.alignment = TextAnchor.MiddleCenter;
        winlostText.resizeTextMinSize = 0;
        winlostText.resizeTextMaxSize = 500;
        if (isAlive) winlostText.text = "You Win!";
        else winlostText.text = "You lose!";
        winlostText.color = Color.black;
        winlostText.rectTransform.parent = transform;
        winlostText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y / 10 * 3);
        winlostText.rectTransform.localPosition = new Vector2(0, screenSize.y / 5 * 1);

        Text noticeText = new GameObject("NoticeText").AddComponent<Text>();
        noticeText.font = myFont;
        noticeText.resizeTextForBestFit = true;
        noticeText.alignment = TextAnchor.MiddleCenter;
        noticeText.resizeTextMinSize = 0;
        noticeText.resizeTextMaxSize = 500;
        noticeText.text = "아무 키나 눌러 재도전 하세요.\nESC키를 누르면 메인 화면으로 돌아갑니다.";
        noticeText.color = Color.black;
        noticeText.rectTransform.parent = transform;
        noticeText.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.85f, screenSize.y / 10 * 3);
        noticeText.rectTransform.localPosition = new Vector2(0, screenSize.y / 5 * -1);

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
