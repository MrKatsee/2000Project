using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽 만들기, 적 스포너 만들기

public class J_PlayerCtrl : MonoBehaviour {
    public GameObject bullet_Lv1;
    public GameObject bullet_Lv2;
    public GameObject bullet_Lv3;

    public GameObject bulletFolder;

	private bool OutsideR;
	private bool OutsideL;
    private int bulletLv = 1; 

    private Transform FirePos;
    private Rigidbody2D playerRB;
    // Use this for initialization
    void Start () {
        playerRB = GetComponent<Rigidbody2D>();
        FirePos = transform.GetChild(0);
		OutsideR = false; OutsideL = false;
        float initialXPos = (Wall.instance.wallR.transform.position.x + Wall.instance.wallL.transform.position.x)/2f; 
        gameObject.transform.position = new Vector2(initialXPos,transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		OutsideR = IsOutsideWallR ();
		OutsideL = IsOutsideWallL ();
		Movement (!OutsideR,!OutsideL);
		Attack (bulletLv);
    }


	//키 정의 (A,D , ->,<- , SPACE)
	private void Movement(bool RMovable, bool LMovable){
		//움직일 수 있을 때(벽 사이에 있음)
		if (RMovable) {
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				playerRB.velocity = new Vector2 (8, 0f);
			}
		} else {
			if(playerRB.velocity.x>0){// If Try to go Outside Wall then Stop
				playerRB.velocity = Vector2.zero; 
			}
		}

		if (LMovable) {
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				playerRB.velocity = new Vector2 (-8, 0f);
			}
		} else {
			if(playerRB.velocity.x<0){// If Try to go Outside Wall then Stop
				playerRB.velocity = Vector2.zero;
			}
		}

		//KeyUp이면 멈춤
		if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D)) 
		{
			playerRB.velocity = Vector2.zero;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A)) {
			playerRB.velocity = Vector2.zero;
		}
	}

	//오른쪽벽 밖으로 나가면 false
	private bool IsOutsideWallR(){ 
		if (transform.position.x >= Wall.instance.wallR.transform.position.x) {
			return true;
		} else {
			return false;
		}
	}

	//왼쪽벽 밖으로 나가면 false
	private bool IsOutsideWallL(){ 
		if (transform.position.x <= Wall.instance.wallL.transform.position.x) {
			return true;
		} else {
			return false;
		}
	}

	//Attack On SpaceKeyDown
	private void Attack(int myBLv){
		//Space 누르면 공격 정의
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject myBullet;
			switch (myBLv) {
			case 1:
				myBullet =Instantiate(bullet_Lv1, new Vector3(FirePos.transform.position.x, FirePos.transform.position.y, FirePos.transform.position.z),transform.rotation) as GameObject;
				//Bullet을 불렛 폴더에 이동
				myBullet.transform.SetParent(bulletFolder.transform);
				//Bullet Fire
				myBullet.GetComponent<Rigidbody2D>().velocity= new Vector2(0,8);
				break;
			case 2:
				myBullet = Instantiate (bullet_Lv2, new Vector3 (FirePos.transform.position.x, FirePos.transform.position.y, FirePos.transform.position.z), transform.rotation) as GameObject;
				//Bullet을 불렛 폴더에 이동
				myBullet.transform.SetParent(bulletFolder.transform);
				//Bullet Fire
				myBullet.GetComponent<Rigidbody2D>().velocity= new Vector2(0,8);
				break;
			case 3: 
				myBullet = Instantiate (bullet_Lv1, new Vector3 (FirePos.transform.position.x, FirePos.transform.position.y, FirePos.transform.position.z), transform.rotation) as GameObject;
				//Bullet을 불렛 폴더에 이동
				myBullet.transform.SetParent(bulletFolder.transform);
				//Bullet Fire
				myBullet.GetComponent<Rigidbody2D>().velocity= new Vector2(0,8);
				break;

			default:
				Debug.Log ("No Bullet Corresponding This Bullet Lv");

				break;
			}

		}

	}
}
