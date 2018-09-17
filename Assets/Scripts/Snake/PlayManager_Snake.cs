using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager_Snake : MonoBehaviour {

    class WallObject
    {
        public int x;
        public int y;
        public Transform transform;
        public SpriteRenderer renderer;
        private static List<WallObject> objects = new List<WallObject>();
        private static Transform parentTransform;

        public static void SetParent(Transform _tr)
        {
            parentTransform = _tr;
        }

        public static bool CheckCollision(int _x, int _y)
        {
            for(int i = 0; i < objects.Count; ++i)
            {
                WallObject wallObject = objects[i];
                if (wallObject.x == _x && wallObject.y == _y) return true;
            }
            return false;
        }

        public static void CreateObject(int _x, int _y)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.tag = "Wall";
            newObject.name = "Wall";
            newObject.transform.parent = parentTransform;
            newObject.GetComponent<SpriteRenderer>().sprite = wallSprite;
            WallObject newWall = new WallObject(_x, _y, newObject.transform);
            newWall.SetPosition();
            newWall.SetScale();
            objects.Add(newWall);
        }

        public WallObject(int _x, int _y, Transform _tr)
        {
            x = _x;
            y = _y;
            transform = _tr;
        }

        public void SetScale()
        {
            transform.localScale = SizeOfSlot;
        }

        public void SetPosition()
        {
            transform.position = GetPosition(x, y);
        }

        public void SetCoordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    static PlayManager_Snake playManager = null;

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

    public static float DelaySecond
    {
        get
        {
            if (isInitalized)
                return playManager.delaySecond;
            else
                return 0;
        }
    }

    public static void Init()
    {
        if (!isInitalized)
        {
            if (playManager == null)
            {
                if (GameObject.Find("PlayManager"))
                    playManager = GameObject.Find("PlayManager").GetComponent<PlayManager_Snake>();
                else
                    playManager = new GameObject("PlayManager").AddComponent<PlayManager_Snake>();
                playManager.InitManager();
                isInitalized = true;
            }
        }
    }

    public static Vector2 GetPosition(int x, int y)
    {
        if (x < 1 || x > playManager.length || y < 1 || y > playManager.length) return default(Vector2);
        // 값은 1 ~ length 까지의 값을 가져야 한다.
        Vector2 pointTopLeftPos = new Vector2(
            Mathf.Lerp(UIManager_Snake.TopLeftPos.x, UIManager_Snake.BotRightPos.x, (float)(x - 1) / playManager.length),
            Mathf.Lerp(UIManager_Snake.TopLeftPos.y, UIManager_Snake.BotRightPos.y, (float)(y - 1) / playManager.length));

        Vector2 pointBotRightPos = new Vector2(
            Mathf.Lerp(UIManager_Snake.TopLeftPos.x, UIManager_Snake.BotRightPos.x, (float)x / playManager.length),
            Mathf.Lerp(UIManager_Snake.TopLeftPos.y, UIManager_Snake.BotRightPos.y, (float)y / playManager.length));

        return Vector2.Lerp(pointTopLeftPos, pointBotRightPos, 0.5f);
    }

    public static void CreateApple()
    {
        int x = Random.Range(1, Length);
        int y = Random.Range(1, Length);
        while(Snake.CheckCollision(x, y) || WallObject.CheckCollision(x, y))
        {
            x = Random.Range(1, Length);
            y = Random.Range(1, Length);
        }
        CreateObject(x, y, "Apple");
    }

    static void CreateObject(int x, int y, string tag)
    {
        switch (tag)
        {
            case "Wall":
                {
                    if(prefab && wallSprite)
                    {
                        WallObject.CreateObject(x, y);
                    }
                    break;
                }
            case "Apple":
                {
                    if (prefab && appleSprite)
                    {
                        GameObject newObject = Instantiate(prefab);
                        newObject.GetComponent<SpriteRenderer>().sprite = appleSprite;
                        newObject.transform.position = GetPosition(x, y);
                        newObject.tag = tag;
                        newObject.name = tag;
                        newObject.transform.localScale = SizeOfSlot;
                    }
                    break;
                }
        }
    }

    private static GameObject prefab;
    private static Sprite wallSprite;
    private static Sprite appleSprite;

    void InitManager()
    {
        prefab = Resources.Load<GameObject>("Prefabs/SpriteObject");
        wallSprite = Resources.Load<Sprite>("Sprites/Wall");
        appleSprite = Resources.Load<Sprite>("Sprites/Apple");
        UIManager_Snake.Init();
        Snake.Init();
        WallObject.SetParent(transform);
        for(int y = 1; y <= length; ++y)
        {
            if(y == 1 || y == length)
            {
                for(int x = 1; x <= length; ++x)
                {
                    CreateObject(x, y, "Wall");
                }
            }
            else
            {
                CreateObject(1, y, "Wall");
                CreateObject(length, y, "Wall");
            }
        }
    }

    int level = 0; // 레벨
    int score = 0; // 점수
    int length = 9; // 필드의 한 변의 길이
    float slotSize { get { return Mathf.Abs((UIManager_Snake.TopLeftPos - GetPosition(1, 1)).x * 2); } }
    float delaySecond = 0.25f;

    public static int Length
    {
        get
        {
            if (isInitalized)
                return playManager.length;
            else return 0;
        }
    }

    public static int Center
    {
        get
        {
            if (isInitalized)
                return Mathf.CeilToInt(playManager.length * .5f);
            else return 0;
        }
    }

    public static Vector2 CenterPosition
    {
        get
        {
            if (isInitalized)
            {
                int centerValue = Center;
                return GetPosition(centerValue, centerValue);
            }
            else return default(Vector2);
        }
    }

    public static Vector2 SizeOfSlot
    {
        get
        {
            return new Vector2(playManager.slotSize, playManager.slotSize);
        }
    }

    private void Start()
    {
        if(!isInitalized) Init();
    }

}
