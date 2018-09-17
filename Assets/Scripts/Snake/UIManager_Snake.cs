using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Snake : MonoBehaviour {

    static UIManager_Snake uiManager = null;
    static Vector2 screenSize = Vector2.zero;

    public static Vector2 TopLeftPos
    {
        get
        {
            if (isInitalized)
            {
                return uiManager.topLeftPos;
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
                return uiManager.botRightPos;
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

    Vector2 topLeftPos;
    Vector2 botRightPos;

    Font myFont;

    void InitUI()
    {
        myFont = Resources.Load<Font>("Fonts/NanumBarunGothic");
        screenSize = new Vector2(Screen.width, Screen.height);
        RectTransform upImageTransform = transform.Find("UpImage").GetComponent<RectTransform>();
        RectTransform downImageTransform = transform.Find("DownImage").GetComponent<RectTransform>();
        RectTransform leftImageTransform = transform.Find("LeftImage").GetComponent<RectTransform>();
        RectTransform rightImageTransform = transform.Find("RightImage").GetComponent<RectTransform>();
        Vector2 updownSizeVector = new Vector2(screenSize.x, screenSize.y / 18);
        upImageTransform.sizeDelta = updownSizeVector;
        downImageTransform.sizeDelta = updownSizeVector;
        float gameScreenSizeValue = screenSize.y / 18 * 17;
        leftImageTransform.sizeDelta = new Vector2(updownSizeVector.y, screenSize.y);
        rightImageTransform.sizeDelta = new Vector2(screenSize.x - gameScreenSizeValue, screenSize.y);
        topLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(updownSizeVector.y, gameScreenSizeValue));
        botRightPos = Camera.main.ScreenToWorldPoint(new Vector3(gameScreenSizeValue, screenSize.y - gameScreenSizeValue));
    }

}
