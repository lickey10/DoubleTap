using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager : MonoBehaviour {
	bool displayGameOver = false;
	bool displayNextLevel = false;
	GameObject[] friends;
	GameObject[] aliveFriends;
	GameObject[] enemies;
	GameObject[] guiBars;
	float[] guiBarTempValues;
	GUIBarScript guiBarScript;
	Slider zombieSlider;
	Text zombieText;
	public float MaxEnemies = 10;
	public GameObject LeftJoystick;
	public GameObject RightJoystick;
	public GameObject FriendsHealth;
	public GameObject ZombiesHealth;
	//public Camera GUICamera;
	//public Font HealthFont;
	public int HealthBarXOffset = 65;
	public int HealthBarYOffset = 485;
	public int HealthBarHeight = 30;

	// Use this for initialization
	void Start () {
		//check for friends that are alive
		friends = GameObject.FindGameObjectsWithTag ("Friend");
		aliveFriends = GameObject.FindGameObjectsWithTag ("Friend");
		guiBars = new GameObject[friends.Length];
		guiBarTempValues = new float[friends.Length];
		
		//check for zombies that are alive
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		//if this is android add the GUI joysticks
		#if UNITY_ANDROID
		//GameObject theWheel  = Instantiate(LeftJoystick, transform.position, transform.rotation) as GameObject;
		//theWheel.transform.parent = transform;

		//theWheel  = Instantiate(RightJoystick, transform.position, transform.rotation) as GameObject;
		//theWheel.transform.parent = transform;
		#endif

		//create friend health graphics
		for(int x = 0; x < friends.Length; x++)
		{
			guiBars[x] = (GameObject)Object.Instantiate (FriendsHealth, new Vector3 (10, 0, 1), new Quaternion ());
			guiBars[x].name = "health_"+ ((GameObject)friends[x]).name;
//			guiBars[x].GetComponent<GUIBarScript> ().ScaleSize = .3f;
//			//guiBars[x].GetComponent<GUIBarScript> ().Position = new Vector2 (10, Screen.height - 30 - (x*30));
//			guiBars[x].GetComponentInChildren<GUIBarScript> ().Position = new Vector2 (10, 150 - (x*HealthBarHeight));
//			//guiBars[x].GetComponent<GUIBarScript> ().Value =x/5;
//			guiBars[x].GetComponent<GUIBarScript> ().DisplayText = false;
//			guiBars[x].GetComponent<GUIBarScript> ().TextSize = 10f;

			Slider slider = guiBars[x].GetComponentInChildren<Slider> ();
			slider.value = 1;

			Text text = guiBars[x].GetComponentInChildren<Text> ();
			text.text = ((GameObject)friends[x]).name;


			Transform panel = guiBars[x].transform.Find("Panel");

			//panel.position = new Vector3(panel.position.x,panel.position.y + Screen.height - 30 - (x*30));
			panel.position = new Vector3(HealthBarXOffset,Screen.height - HealthBarYOffset + (x*HealthBarHeight));
		}

		//create zombies display
		ZombiesHealth = (GameObject)Object.Instantiate (ZombiesHealth, new Vector3 (10, 0, 1), new Quaternion ());
		ZombiesHealth.name = "health_Zombies";		

		ZombiesHealth.GetComponent<GUIBarScript> ().ScaleSize = .6f;
		ZombiesHealth.GetComponent<GUIBarScript> ().Position = new Vector2 (Screen.width - 160, 0);
		ZombiesHealth.GetComponent<GUIBarScript> ().DisplayText = true;
		ZombiesHealth.GetComponent<GUIBarScript> ().TextSize = 35f;
		ZombiesHealth.GetComponent<GUIBarScript> ().TextOffset = new Vector2 (100, 40);
	}
	
	// Update is called once per frame
	void Update () {
		//check for friends that are alive
		friends = GameObject.FindGameObjectsWithTag ("Friend");

		//check for zombies that are alive
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		//update health for friends
//		for(int x = 0; x < guiBars.Length; x++)
//		{
//			float tempValue = 0f;
//
//			for(int y = 0; y < friends.Length; y++)
//			{
//				if(friends[y].name == guiBars[x].name.Replace("health_",""))
//				{
//					tempValue = friends[y].GetComponent<EnemyDamageReceiver>().hitPoints/50f;
//					break;
//				}
//			}
//
//			guiBars[x].GetComponent<GUIBarScript> ().Value = tempValue;
//			tempValue = 0f;
//		}

		//update health for friends
		for(int x = 0; x < guiBars.Length; x++)
		{
			float tempValue = 0f;
			
			for(int y = 0; y < friends.Length; y++)
			{
				if(friends[y].name == guiBars[x].name.Replace("health_",""))
				{
					tempValue = friends[y].GetComponent<EnemyDamageReceiver>().hitPoints/50;
					break;
				}
			}
			
			guiBars[x].GetComponentInChildren<Slider> ().value = tempValue;
			
//			if(guiBars[x].GetComponentInChildren<GUIBarScript> ())
//				guiBars[x].GetComponentInChildren<GUIBarScript> ().Value = tempValue;
		}

		if (friends.Length == 0)//friends are all dead - game over you lose
			gameOver ();
		else if (enemies.Length == 0)//all zombies are dead - start next level
			nextLevel ();

		ZombiesHealth.GetComponent<GUIBarScript> ().Value = enemies.Length / MaxEnemies;
		ZombiesHealth.GetComponent<GUIBarScript> ().TextString = enemies.Length.ToString() +"/"+ MaxEnemies.ToString();
	}

	private void nextLevel()
	{
		displayNextLevel = true;
	}

	private void gameOver()
	{
		displayGameOver = true;
	}

	private void OnGUI(){
		if(displayGameOver)
			GUI.Box(new Rect((Screen.width - 100)/2,Screen.height-50,100,50),"GAME OVER!!!");

		if(displayNextLevel)
			GUI.Box(new Rect((Screen.width - 100)/2,Screen.height-50,100,50),"YOU WIN!!!");

		//GUI.Box(new Rect(10,Screen.height-30,200,20),"Friends: "+ friends.Length.ToString() +"/5   Zombies: "+ enemies.Length.ToString() +"/10");





	}
}
