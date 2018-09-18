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
        private static Sprite body2Sprite;
        private static LinkedList<SnakeObject> objects;
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
            objects = new LinkedList<SnakeObject>();
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
            body2Sprite = Resources.Load<Sprite>("Sprites/SnakeBody2");
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

        public static void SortObject(LinkedListNode<SnakeObject> node)
        {
            SnakeObject nowObject = node.Value;
            SnakeObject prevObject = node.Previous.Value;
            if (node != objects.Last)
            {
                SnakeObject nextObject = node.Next.Value;
                if (nextObject.y == nowObject.y && prevObject.x == nowObject.x)
                {
                    nowObject.SetSprite(body2Sprite);
                    if (nextObject.x > nowObject.x)
                    {
                        if (prevObject.y > nowObject.y)
                        {
                            // ┌
                            // 0, 0, -90
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, -90f);

                        }
                        else
                        {
                            // └
                            // 0, 0, 0
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                        }
                    }
                    else
                    {
                        if (prevObject.y > nowObject.y)
                        {
                            //  ┐
                            // 0, 0, 180
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                        }
                        else
                        {
                            //  ┘
                            // 0, 0, 90
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        }
                    }
                }
                else if (prevObject.y == nowObject.y && nextObject.x == nowObject.x)
                {
                    nowObject.SetSprite(body2Sprite);
                    if (prevObject.x > nowObject.x)
                    {
                        if (nextObject.y > nowObject.y)
                        {
                            // ┌
                            // 0, 0, -90
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, -90f);

                        }
                        else
                        {
                            // └
                            // 0, 0, 0
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                        }
                    }
                    else
                    {
                        if (nextObject.y > nowObject.y)
                        {
                            //  ┐
                            // 0, 0, 180
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                        }
                        else
                        {
                            //  ┘
                            // 0, 0, 90
                            nowObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        }
                    }
                }
                else
                {
                    nowObject.SetSprite(bodySprite);
                    if (prevObject.y != nowObject.y)
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                    }
                    else
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                    }
                }
            }
            else // 꼬리일 때
            {
                if (prevObject.x == nowObject.x)
                {
                    if (prevObject.y > nowObject.y)
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                    }
                    else
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                    }
                }
                else
                {
                    if (prevObject.x > nowObject.x)
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, -90f);
                    }
                    else
                    {
                        nowObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                    }
                }
            }
        }

        public static void MoveObject(HeadDirection headDirection)
        {
            snake.turn++;
            SnakeObject headObject = objects.First.Value;
            int x = headObject.x;
            int y = headObject.y;
            switch (headDirection)
            {
                case HeadDirection.Up:
                    {
                        y--;
                        if (y <= 0) y = PlayManager_Snake.Length;
                        headObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }
                case HeadDirection.Down:
                    {
                        y++;
                        if (y > PlayManager_Snake.Length) y = 1;
                        headObject.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                        break;
                    }
                case HeadDirection.Left:
                    {
                        x--;
                        if (x <= 0) x = PlayManager_Snake.Length;
                        headObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        break;
                    }
                case HeadDirection.Right:
                    {
                        x++;
                        if (x > PlayManager_Snake.Length) x = 1;
                        headObject.transform.localRotation = Quaternion.Euler(0, 0, -90f);
                        break;
                    }
            }
            Vector2 nowPos = headObject.transform.localPosition;
            Vector2 targetPos = PlayManager_Snake.GetPosition(x, y);
            Collider2D col2D = Physics2D.OverlapPoint(targetPos);
            if (col2D && col2D.tag != "Tail") // 뭔가에 충돌
            {
                switch (col2D.tag)
                {
                    case "Body":
                    case "Wall":
                        {
                            LinkedListNode<SnakeObject> node = objects.Last;
                            while (true)
                            {
                                if (node.Previous == null) break; // 머리의 좌표는 이 루프문에서 수정하지 않는다.
                                SnakeObject nowObject = node.Value;
                                SnakeObject prevObject = node.Previous.Value;
                                nowObject.SetCoordinate(prevObject.x, prevObject.y);
                                nowObject.SetPosition();
                                node = node.Previous;
                            }
                            headObject.SetCoordinate(x, y); // 머리의 좌표를 수정한다.
                            headObject.SetPosition();
                            node = objects.Last;
                            while (true)
                            {
                                if (node.Previous == null) break; // 머리의 스프라이트는 이 루프문에서 수정하지 않는다.
                                SortObject(node);
                                node = node.Previous;
                            }
                            snake.isAlive = false;
                            
                            break;
                        }
                    case "Apple": // 먹었을 때는 머리 바로 다음 칸의 스프라이트만을 수정한다.
                        {
                            // 1up
                            Destroy(col2D.gameObject);
                            headObject.SetCoordinate(x, y);
                            headObject.SetPosition();
                            SortObject(CreateObject(SnakeObjectType.Body, headDirection));
                            snake.isAppleIsNull = true;
                            UIManager_Snake.PlayEffect();
                            snake.Score += 50;
                            if (snake.turn == 1) snake.Score += 150;
                            else if (snake.turn == 2) snake.score += 125;
                            else if (snake.turn >= 3 && snake.turn <= 4) snake.score += 100;
                            else if (snake.turn >= 5 && snake.turn <= 8) snake.Score += 75;
                            else if (snake.turn >= 9 && snake.turn <= 16) snake.Score += 50;
                            snake.turn = 0;
                            break;
                        }
                }
            }
            else // 그냥 움직일 때는 모든 스프라이트를 수정한다.
            {
                LinkedListNode<SnakeObject> node = objects.Last;
                while (true)
                {
                    if (node.Previous == null) break; // 머리의 좌표는 이 루프문에서 수정하지 않는다.
                    SnakeObject nowObject = node.Value;
                    SnakeObject prevObject = node.Previous.Value;
                    nowObject.SetCoordinate(prevObject.x, prevObject.y);
                    nowObject.SetPosition();
                    node = node.Previous;
                }
                headObject.SetCoordinate(x, y); // 머리의 좌표를 수정한다.
                headObject.SetPosition();
                node = objects.Last;
                while (true)
                {
                    if (node.Previous == null) break; // 머리의 스프라이트는 이 루프문에서 수정하지 않는다.
                    SortObject(node);
                    node = node.Previous;
                }
            }
        }

        public static LinkedListNode<SnakeObject> CreateObject(SnakeObjectType type, HeadDirection headDirection)
        {
            GameObject newObject = Instantiate(prefab, parentTransform);
            newObject.name = System.Enum.GetName(typeof(SnakeObjectType), type);
            switch (type)
            {
                case SnakeObjectType.Head:
                    {
                        int center = PlayManager_Snake.Center;
                        newObject.tag = "Head";
                        newObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        SnakeObject newSnakeObject = new SnakeObject(center, center, type, newObject.transform, newObject.GetComponent<SpriteRenderer>());
                        newSnakeObject.SetSprite(headSprite);
                        newSnakeObject.SetScale();
                        newSnakeObject.SetPosition();
                        return objects.AddFirst(newSnakeObject);
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
                            return objects.AddLast(newSnakeObject);
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
                            return objects.AddAfter(headObjectNode, newSnakeObject);
                        }
                        break;
                    }
            }
            return null;
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
    int turn = 0;
    int score = 0;

    int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UIManager_Snake.SetScoreUI(score);
        }
    }
    bool isAlive;
    bool isAppleIsNull;
    [SerializeField]
    HeadDirection headDir;

    private void InitSnake()
    {
        snake.length = 3;
        snake.isAlive = true;
        snake.isAppleIsNull = false;
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
            if (Input.GetKey(KeyCode.DownArrow) && (headDir != HeadDirection.Up && headDir != HeadDirection.Stop)) tempValue = HeadDirection.Down;
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
        UIManager_Snake.StartUI(true);
        PlayManager_Snake.CreateApple();
        

        if (headDir == HeadDirection.Stop)
        {
            yield return new WaitUntil(
                () =>
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow)
            );
            if (Input.GetKey(KeyCode.UpArrow)) headDir = HeadDirection.Up;
            if (Input.GetKey(KeyCode.LeftArrow)) headDir = HeadDirection.Left;
            if (Input.GetKey(KeyCode.RightArrow)) headDir = HeadDirection.Right;
            UIManager_Snake.StartUI(false);
            SnakeObject.MoveObject(headDir);
            if (isAppleIsNull)
            {
                PlayManager_Snake.CreateApple();
                isAppleIsNull = false;
            }
            
        }

        while (isAlive)
        {
            yield return StartCoroutine(InputWhileDelay(PlayManager_Snake.DelaySecond));
            SnakeObject.MoveObject(headDir);
            if (SnakeObject.Count >= (PlayManager_Snake.Length - 2) * (PlayManager_Snake.Length - 2)) break;
            else
            {
                if (isAppleIsNull)
                {
                    PlayManager_Snake.CreateApple();
                    isAppleIsNull = false;
                }
            }
        }

        if (isAlive)
        {
            // 2김
            Score += 50000;
            UIManager_Snake.EndUI(true, score);
        }
        else
        {
            // 짐
            UIManager_Snake.EndUI(false, score);
        }
    }

    private void OnDestroy()
    {
        isInitalized = false;
        snake = null;
    }
}
