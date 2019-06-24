//var doorClip : AnimationClip;
var Key : GameObject;
//private var Door = false;
//var sound1 : AudioClip;
var delay : float = 3;
private var nextTime : float = 0;

//var target : GameObject;
//var eventName1 : String;
//var eventName2 : String;
var drawerOpen = false;

function Start () 
{

}

function Update () 
{
	
}

//Called via message
function Interact () {
	if(Time.time > nextTime){ //if it has been long enough, and we are either not past our limit or not limited
		nextTime = Time.time + delay; //set next use time
		
		if(drawerOpen)//close drawer
		{
			iTweenEvent.GetEvent(this.gameObject, "close").Play();
			drawerOpen = false;
		}
		else //open drawer
		{
			iTweenEvent.GetEvent(this.gameObject, "open").Play();
			drawerOpen = true;
		}
		
		if(GetComponent.<AudioSource>() != null && GetComponent.<AudioSource>().clip != null)
			GetComponent.<AudioSource>().Play ();
	}
}

//function OnTriggerEnter (theCollider : Collider)
//{
//	if (theCollider.tag == "Player")
//	{
//		Door = true;
//	}
//	
//	
//}
//
//function OnTriggerExit (theCollider : Collider)
//{
//	if (theCollider.tag == "Player")
//	{
//		Door = false;
//	}
//}