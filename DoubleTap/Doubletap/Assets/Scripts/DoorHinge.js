#pragma strict
//var doorClip : AnimationClip;
//var Key : GameObject;
private var Door = false;
//var sound1 : AudioClip;
var delay : float = 3;
private var nextTime : float = 0;

//var target : GameObject;
//var eventName1 : String;
//var eventName2 : String;
var doorOpen = true;

function Start () 
{

}


//Called via message
function Interact () {
	if(Time.time > nextTime){ //if it has been long enough, and we are either not past our limit or not limited
		nextTime = Time.time + delay; //set next use time
		
		if(doorOpen)//close door
		{
			iTweenEvent.GetEvent(this.gameObject, "closeDoor").Play();
			doorOpen = false;
		}
		else //open door
		{
			iTweenEvent.GetEvent(this.gameObject, "openDoor").Play();
			doorOpen = true;
		}
		
		if(GetComponent.<AudioSource>() != null && GetComponent.<AudioSource>().clip != null)
			GetComponent.<AudioSource>().Play ();
	}
}