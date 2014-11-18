using UnityEngine;
using System.Collections;

public class aimingControler : MonoBehaviour {

	[Range(0.0F, 0.5F)]
	public float xOffsite,yOffsite;
	[Range(0.0F, 500.0F)]
	public float xSpeed,ySpeed;
	[Range(0.0F, 2.0F)]
	public float timeToBalance;
	public Transform pointToCover;
	public Transform pointToRotate;
	public Transform playerWraper;
	private Vector3 mousePosition;
	private float timer;
	// Use this for initialization
	void Start () {
		mousePosition=Input.mousePosition;
		Screen.showCursor=false;
	}

	private void targetFinder(){
		Ray finder = Camera.main.ScreenPointToRay(transform.position);
		RaycastHit contact;
		if(Physics.Raycast(finder,out contact,gameData.aiActivation)&&contact.transform.gameObject.GetComponent<motionEnemy>()!=null)
		{
			GetComponent<UnityEngine.UI.RawImage>().color=Color.green;
			gameData.aimPoint = contact.transform.position;
            gameData.aimNavigation = contact.transform;
		}else
		{ 
			gameData.aimPoint= Vector3.zero;
            gameData.aimNavigation = null;
			GetComponent<UnityEngine.UI.RawImage>().color=Color.white;
		}
	}

	// Update is called once per frame
	void Update () {
		if(!gameData.pausedGame&&Input.mousePresent ){
			if(mousePosition!=Input.mousePosition){
				timer=0;
				Vector3 tmp_x=new Vector3(mousePosition.x-Input.mousePosition.x,0,0);
				Vector3 tmp_y=new Vector3(0,mousePosition.y-Input.mousePosition.y,0);
				if(tmp_x.x<0 && playerWraper.rotation.y<=xOffsite )
					pointToRotate.RotateAround(gameData.playerPosition,Vector3.up,xSpeed*Time.deltaTime);
				if(tmp_x.x>0 && playerWraper.rotation.y>=-xOffsite)
					pointToRotate.RotateAround(gameData.playerPosition,Vector3.up,-xSpeed*Time.deltaTime);
				if(tmp_y.y<0 && playerWraper.rotation.x>=-yOffsite )
					pointToRotate.RotateAround(gameData.playerPosition,Vector3.left,ySpeed*Time.deltaTime);
				if(tmp_y.y>0 && playerWraper.rotation.x<=yOffsite)
					pointToRotate.RotateAround(gameData.playerPosition,Vector3.left,-ySpeed*Time.deltaTime);
			}else if(timer>=timeToBalance){
				balanceX();
				balanceY();
			}
			if(pointToCover.position!=pointToRotate.position)
				playerWraper.rotation=Quaternion.LookRotation(Vector3.Normalize(pointToRotate.position-gameData.playerPosition),Vector3.up);
			transform.position=Camera.main.WorldToScreenPoint(pointToRotate.position);
			mousePosition=Input.mousePosition;
			timer+=Time.deltaTime;
			targetFinder();
		}
	}
	
	private void balanceX(){
		if(playerWraper.rotation.y>0.02f)
			pointToRotate.RotateAround(gameData.playerPosition,Vector3.up,-xSpeed*Time.deltaTime);
		if(playerWraper.rotation.y<-0.02f)
			pointToRotate.RotateAround(gameData.playerPosition,Vector3.up,xSpeed*Time.deltaTime);
	}

	private void balanceY(){
		if(playerWraper.rotation.x>0.02f)
			pointToRotate.RotateAround(gameData.playerPosition,Vector3.left,ySpeed*Time.deltaTime);
		if(playerWraper.rotation.x<-0.02f)
			pointToRotate.RotateAround(gameData.playerPosition,Vector3.left,-ySpeed*Time.deltaTime);
	}

}
