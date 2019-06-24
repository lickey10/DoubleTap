var doorClip : AnimationClip;
var Key : GameObject;
private var Door = false;
var sound1 : AudioClip;

var target : GameObject;
var eventName1 : String;
var eventName2 : String;

function Start () 
{

}

function Update () 
{
	if (Input.GetKeyDown(KeyCode.RightControl))// && Door == true && Key.active == false)
	{
		//GameObject.Find("leftGate").animation.Play("gateOpenLeft");
		//gameObject.GetComponent.<Animation>().Play(doorClip.name);
		
		iTweenEvent.GetEvent(target, eventName1).Play();
		GetComponent.<AudioSource>().clip = sound1;
		GetComponent.<AudioSource>().Play ();
	}
}

function OnTriggerEnter (theCollider : Collider)
{
	if (theCollider.tag == "Player")
	{
		Door = true;
	}
	
	
}

function OnTriggerExit (theCollider : Collider)
{
	if (theCollider.tag == "Player")
	{
		Door = false;
	}
}