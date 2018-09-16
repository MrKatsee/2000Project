using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽 만들기, 적 스포너 만들기

public class J_PlayerCtrl : MonoBehaviour {
    public GameObject bullet_Lv1;
    public GameObject bullet_Lv2;
    public GameObject bullet_Lv3;

    public GameObject bulletFolder;


    private int myLv = 1; 

    private Transform FirePos;
    private Rigidbody2D playerRB;
    // Use this for initialization
    void Start () {
        playerRB = GetComponent<Rigidbody2D>();
        FirePos = transform.GetChild(0);
        float initialXPos = (Wall.instance.wall1.transform.position.x + Wall.instance.wall2.transform.position.x)/2f; 
        gameObject.transform.position = new Vector2(initialXPos,transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
        //이동키 정의 (A,D , ->,<-)
		if(Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {
            
            playerRB.velocity=new Vector2(4, 0f);
            

        } 
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            playerRB.velocity = Vector2.zero;

        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerRB.velocity = new Vector2(-4, 0f);


        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            playerRB.velocity = Vector2.zero;

        }
        //Space 누르면 공격 정의
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject myBullet;
            myBullet =Instantiate(bullet_Lv1, new Vector3(FirePos.transform.position.x, FirePos.transform.position.y, FirePos.transform.position.z),transform.rotation) as GameObject;
            myBullet.transform.SetParent(bulletFolder.transform);
            myBullet.GetComponent<Rigidbody2D>().velocity= new Vector2(0,8);
        }


    }
}
