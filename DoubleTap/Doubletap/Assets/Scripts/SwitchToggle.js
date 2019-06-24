#pragma strict
var delay : float = 3;
private var nextTime : float = 0;
private var switchUp = true;
var Fire : GameObject;

//Called via message
function Interact () {
	if(Time.time > nextTime){ //if it has been long enough, and we are either not past our limit or not limited
		nextTime = Time.time + delay; //set next use time
		
		if(switchUp)
		{
			iTweenEvent.GetEvent(this.gameObject, "moveDown").Play();
			switchUp = false;
		}
		else //flip switch
		{
			iTweenEvent.GetEvent(this.gameObject, "moveUp").Play();
			switchUp = true;
		}
		
//		GetComponent.<AudioSource>().clip = sound1;
//		GetComponent.<AudioSource>().Play ();

	}
}

function ITweenComplete()
{
	//var complete = "yes";
}
