using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//좌표계, 0 , 0은 좌측 상단이다.
public class Snake : MonoBehaviour {

    static Snake snake = null;

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

    public static bool IsAlive
    {
        get
        {
            if (!isInitalized) return false;
            else return snake.isAlive;
        }
    }

    public static void Init()
    {
        if (!isInitalized)
        {
            if(snake == null)
            {
                if (GameObject.Find("Snake"))
                    snake = GameObject.Find("Snake").GetComponent<Snake>();
                else
                    snake = new GameObject("Snake").AddComponent<Snake>();
                
                snake.StartCoroutine(snake.Routine());
                isInitalized = true;
            }
        }
    }

    public static bool CheckCollision(int _x, int _y)
    {
        return SnakeObject.CheckCollision(_x, _y);
    }

    enum SnakeObjectType
    {
        Head,
        Body,
        Tail
    }

    enum HeadDirection
    {
        Stop,
        Down,
        Up,
        Left,
        Right
    }

    class SnakeObject
    {
        private static GameObject prefab;
        private static Sprite headSprite;
        private static Sprite bodySprite;
        private static Sprite tailSprite;
        private static LinkedList<SnakeObject> objects = new LinkedList<SnakeObject>();
        private static Transform parentTransform;

        public static int Count
        {
            get
            {
                return objects.Count;
            }
        }

        public static void SetPrefab()
        {
            prefab = Resources.Load<GameObject>("Prefabs/SpriteObject");
        }

        public static void SetParent(Transform _tr)
        {
            parentTransform = _tr;
        }

        public static void SetSpriteData()
        {
            headSprite = Resources.Load<Sprite>("Sprites/SnakeHead");
            bodySprite = Resources.Load<Sprite>("Sprites/SnakeBody");
            tailSprite = Resources.Load<Sprite>("Sprites/SnakeTail");
        }

        public static bool CheckCollision(int _x, int _y)
        {
            LinkedListNode<SnakeObject> node = objects.First;
            for(int i = 0; i < objects.Count; ++i)
            {
                if (node.Value.x == _x && node.Value.y == _y) return true;
                node = node.Next;
            }
            return false;
        }

        public static void MoveObject(HeadDirection headDirection)
        {
            SnakeObject headObject = objects.First.Value;
            int x = headObject.x;
            int y = headObject.y;
            switch (headDirection)
            {
                case HeadDirection.Up:
                    {
                        y--;
                        if (y <= 0) y = PlayManager_Snake.Length;
                        break;
                    }
                case HeadDirection.Down:
                    {
                        y++;
                        if (y > PlayManager_Snake.Length) y = 1;
                        break;
                    }
                case HeadDirection.Left:
                    {
                        x--;
                        if (x <= 0) x = PlayManager_Snake.Length;
                        break;
                    }
                case HeadDirection.Right:
                    {
                        x++;
                        if (x > PlayManager_Snake.Length) x = 1;
                        break;
                    }
            }
            Vector2 nowPos = headObject.transform.localPosition;
            Vector2 targetPos = PlayManager_Snake.GetPosition(x, y);
            Collider2D col2D = Physics2D.OverlapPoint(targetPos);
            if (col2D) // 뭔가에 충돌
            {
                Debug.Log(col2D.tag);
                switch (col2D.tag)
                {
                    case "Body":
                    case "Tail":
                    case "Wall":
                        {
                            snake.isAlive = false;
                            break;
                        }
                    case "Apple":
                        {
                            // 1up
                            Destroy(col2D.gameObject);
                            headObject.SetCoordinate(x, y);
                            headObject.SetPosition();
                            CreateObject(SnakeObjectType.Body, headDirection);
                            PlayManager_Snake.CreateApple();
                            break;
                        }
                }
            }
            else // 그냥 움직임
            {
                LinkedListNode<SnakeObject> node = objects.Last;
                while (true)
                {
                    if (node.Previous == null) break;
                    SnakeObject prevObject = node.Previous.Value;
                    SnakeObject nowObject = node.Value;
                    nowObject.SetCoordinate(prevObject.x, prevObject.y);
                    nowObject.SetPosition();
                    node = node.Previous;
                }
                headObject.SetCoordinate(x, y);
                headObject.SetPosition();
            }
        }

        public static void CreateObject(SnakeObjectType type, HeadDirection headDirection)
        {
            GameObject newObject = Instantiate(prefab, parentTransform);
            newObject.name = System.Enum.GetName(typeof(SnakeObjectType), type);
            switch (type)
            {
                case SnakeObjectType.Head:
                    {
                        int center = PlayManager_Snake.Center;
                        newObject.tag = "Head";
                        SnakeObject newSnakeObject = new SnakeObject(center, center, type, newObject.transform, newObject.GetComponent<SpriteRenderer>());
                        newSnakeObject.SetSprite(headSprite);
                        newSnakeObject.SetScale();
                        newSnakeObject.SetPosition();
                        objects.AddFirst(newSnakeObject);
                        break;
                    }
                case SnakeObjectType.Tail:
                    {
                        LinkedListNode<SnakeObject> lastObjectNode = objects.Last;
                        SnakeObject prevObject = lastObjectNode.Value;
                        if (prevObject != null)
                        {
                            int x = prevObject.x;
                            int y = prevObject.y + 1;
                            newObject.tag = "Tail";
                            SnakeObject newSnakeObject = new SnakeObject(x, y, type, newObject.transform, newObject.GetComponent<SpriteRenderer>());
                            newSnakeObject.SetSprite(tailSprite);
                            newSnakeObject.SetScale();
                            newSnakeObject.SetPosition();
                            objects.AddLast(newSnakeObject);
                        }
                        break;
                    }
                case SnakeObjectType.Body:
                    {
                        LinkedListNode<SnakeObject> headObjectNode = objects.First;
                        SnakeObject headObject = headObjectNode.Value;
                        if (headObject != null)
                        {
                            int x = headObject.x;
                            int y = headObject.y;
                            switch (headDirection)
                            {
                                case HeadDirection.Up:
                                case HeadDirection.Stop:
                                    {
                                        y++;
                                        if (y > PlayManager_Snake.Length) y = 1;
                                        break;
                                    }
                                case HeadDirection.Down:
                                    {
                                        y--;
                                        if (y <= 0) y = PlayManager_Snake.Length;
                                        break;
                                    }
                                case HeadDirection.Left:
                                    {
                                        x++;
                                        if (x > PlayManager_Snake.Length) x = 1;
                                        break;
                                    }
                                case HeadDirection.Right:
                                    {
                                        x--;
                                        if (x <= 0) x = PlayManager_Snake.Length;
                                        break;
                                    }
                            }
                            newObject.tag = "Body";
                            SnakeObject newSnakeObject = new SnakeObject(x, y, type, newObject.transform, newObject.GetComponent<SpriteRenderer>());
                            newSnakeObject.SetSprite(bodySprite);
                            newSnakeObject.SetScale();
                            newSnakeObject.SetPosition();
                            objects.AddAfter(headObjectNode, newSnakeObject);
                        }
                        break;
                    }
            }
        }

        public int x;
        public int y;
        public SnakeObjectType type;
        public Transform transform;
        public SpriteRenderer renderer;

        public SnakeObject(int _x, int _y, SnakeObjectType _type, Transform _tr, SpriteRenderer _renderer)
        {
            x = _x;
            y = _y;
            type = _type;
            transform = _tr;
            renderer = _renderer;
        }

        public void SetSprite(Sprite _sprite)
        {
            if (renderer && _sprite)
            {
                renderer.sprite = _sprite;
            }
        }

        public void SetScale()
        {
            transform.localScale = PlayManager_Snake.SizeOfSlot;
        }

        public void SetPosition()
        {
            transform.position = PlayManager_Snake.GetPosition(x, y);
        }

        public void SetCoordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    int length; // "처음" 길이
    bool isAlive;
    [SerializeField]
    HeadDirection headDir;

    private void InitSnake()
    {
        snake.length = 3;
        snake.isAlive = true;
        snake.headDir = HeadDirection.Stop;
        SnakeObject.SetPrefab();
        SnakeObject.SetParent(transform);
        SnakeObject.SetSpriteData();
        for(int i = 0; i < snake.length; ++i)
        {
            if (i == 0)
            {
                SnakeObject.CreateObject(SnakeObjectType.Head, headDir);
            }
            else if (i == length - 1)
            {
                SnakeObject.CreateObject(SnakeObjectType.Tail, headDir);
            }  
            else
            {
                SnakeObject.CreateObject(SnakeObjectType.Body, headDir);
            }
        }
    }

    IEnumerator InputWhileDelay(float time)
    {
        float nowTime = 0f;
        HeadDirection tempValue = headDir;
        while (nowTime < time)
        {
            if (Input.GetKey(KeyCode.UpArrow) && headDir != HeadDirection.Down) tempValue = HeadDirection.Up;
            if (Input.GetKey(KeyCode.DownArrow) && headDir != HeadDirection.Up) tempValue = HeadDirection.Down;
            if (Input.GetKey(KeyCode.LeftArrow) && headDir != HeadDirection.Right) tempValue = HeadDirection.Left;
            if (Input.GetKey(KeyCode.RightArrow) && headDir != HeadDirection.Left) tempValue = HeadDirection.Right;
            nowTime += Time.deltaTime;
            yield return null;
        }
        headDir = tempValue;
    }

    IEnumerator Routine()
    {

        yield return new WaitUntil(() => PlayManager_Snake.IsInitalized);
        snake.InitSnake();
        PlayManager_Snake.CreateApple();

        if (headDir == HeadDirection.Stop)
        {
            yield return new WaitUntil(
                () =>
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow)
            );
            if (Input.GetKey(KeyCode.UpArrow)) headDir = HeadDirection.Up;
            if (Input.GetKey(KeyCode.DownArrow)) headDir = HeadDirection.Down;
            if (Input.GetKey(KeyCode.LeftArrow)) headDir = HeadDirection.Left;
            if (Input.GetKey(KeyCode.RightArrow)) headDir = HeadDirection.Right;
        }

        while (isAlive)
        {
            yield return StartCoroutine(InputWhileDelay(PlayManager_Snake.DelaySecond));
            SnakeObject.MoveObject(headDir);
            if (SnakeObject.Count >= (PlayManager_Snake.Length - 2) * (PlayManager_Snake.Length - 2)) break;
        }

        if (isAlive)
        {
            // 2김
            Debug.Log("2김");
        }
    }
}
