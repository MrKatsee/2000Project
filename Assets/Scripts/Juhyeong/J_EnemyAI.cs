using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_EnemyAI : MonoBehaviour {
    float timer = 0,randomTimer = 0, goHomeTimer=0;
    public Transform HomePos;
    float OriginX;
    float OriginY;
    float w;
    float h;
    Vector2 ToPlayerVec,ToHomeVec;
    public bool moveCircle, moveToPlayer, moveToHomePos;
    // Use this for initialization
    void Start()
    {
        OriginX = HomePos.transform.position.x;
        OriginY = HomePos.transform.position.y;


        moveCircle = false;
        moveToPlayer = false;
        moveToHomePos = false;
        w = 1;
        h = 1;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ToPlayerVec = (J_PlayerManager.instance.player.transform.position - transform.position).normalized;
        ToHomeVec = (HomePos.transform.position - transform.position).normalized;
        randomTimer += Time.deltaTime;
        if (randomTimer >= 3)
        {
            int i = Random.Range(1, 21);
            if ((i == 5 ||i==2||i==3)&& moveToPlayer == false && moveToHomePos ==false)
            {
                moveCircle = true;
                i = 0;
            }
            if (i == 6 && moveCircle==false)
            {
                moveToPlayer = true;
                i = 0;
            }
            i = 0;
            randomTimer = 0;
        }

        if (moveCircle)
        {
            timer += 3f * Time.deltaTime;
            MoveCircle();
        } else {
            timer = 0;
        }
        
        if(moveToPlayer)
        {
            goHomeTimer += Time.deltaTime;
            MoveToPlayer();
            if (goHomeTimer > Random.Range(3, 5))
            {
                moveToHomePos = true;
                goHomeTimer = 0;
            }
        }
        if (moveToHomePos)
        {
            MoveToHomePos();

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerStay2D(other);
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<J_BulletDestroy>() != null)
        {
            J_GameManager.score += 1;
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveCircle()
    {

        transform.position = new Vector2(OriginX - 1f + w * Mathf.Cos(timer), OriginY + h * Mathf.Sin(timer));

        if (timer >= 6.28f)
        {
            transform.position = HomePos.position;
            moveCircle = false;
        }
    }

    private void MoveToPlayer()
    {
        GetComponent<Rigidbody2D>().velocity = 3*ToPlayerVec;
    }

    private void MoveToHomePos()
    {
        GetComponent<Rigidbody2D>().velocity = 3*ToHomeVec;
        
        if (Vector2.Distance(transform.position, HomePos.transform.position)<0.05f)
        {
            moveToHomePos = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = HomePos.position;
        }
    }

    private void fetchPosition()
    {
        OriginX = transform.position.x;
        OriginY = transform.position.y;
    }
}
