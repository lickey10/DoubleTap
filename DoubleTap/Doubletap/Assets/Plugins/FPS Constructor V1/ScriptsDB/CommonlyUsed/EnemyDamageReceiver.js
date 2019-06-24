#pragma strict
#pragma downcast

var hitPoints : float = 100.0;
var deathEffect : Transform;
var effectDelay = 0.0;
private var gos : GameObject[];
var multiplier : float = 1;
var deadReplacement : Rigidbody;
var DamageTarget : GameObject; //target to apply damage to
@HideInInspector
var playerObject : GameObject;
var useHitEffect : boolean = true;
var TakeDamageSound : AudioClip; //the sound to play when we take damage

@HideInInspector
var isEnemy : boolean = false;

private var nextTime : float = 0;
var delay : float = 20;

private var currentShader : Shader;// = ((Renderer)GetComponent.<Renderer>()).sh;
public var outlineSize : float = 0.01f;
public var outlineColor : Color = Color.red;

//function Start()
//{
//if(TakeDamageSound != null)
//	{
//		
//		audioSource = GetComponentsInChildren(AudioSource);// GetComponent.<AudioSource>();
//		audioSource.clip = TakeDamageSound;
//	}
//}

function ApplyDamage(Arr : Object[]){
	//Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
	//Find the player if we haven't
	if(Arr[1] == true){
		if(!playerObject){
			playerObject = GameObject.FindWithTag("Player");
		}
		if(useHitEffect){
			playerObject.BroadcastMessage("HitEffect");
		}
	}
	
	// We already have less than 0 hitpoints, maybe we got killed already?
	if (hitPoints <= 0.0)
		return;
	var tempFloat : float;
	tempFloat = Arr[0];
	//float.TryParse(Arr[0], tempFloat);
	hitPoints -= tempFloat*multiplier;
	if (hitPoints <= 0.0) {
		// Start emitting particles
		var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
		if (emitter)
			emitter.emit = true;

		Invoke("DelayedDetonate", effectDelay);
	}
}

function ApplyDamagePlayer (damage : float){
	//Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
	//Find the player if we haven't
	if(!playerObject){
		playerObject = GameObject.FindWithTag("Player");
	}
	if(useHitEffect){
		playerObject.BroadcastMessage("HitEffect");
	}
	
	// We already have less than 0 hitpoints, maybe we got killed already?
	if (hitPoints <= 0.0)
		return;
	//float.TryParse(Arr[0], tempFloat);
	hitPoints -= damage*multiplier;
	if (hitPoints <= 0.0) {
		// Start emitting particles
		var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
		if (emitter)
			emitter.emit = true;

		Invoke("DelayedDetonate", effectDelay);
	}
}

function ApplyDamage (damage : float){
	//Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
	//Find the player if we haven't
	var audioSource : AudioSource;
	
	// We already have less than 0 hitpoints, maybe we got killed already?
	if (hitPoints <= 0.0)
		return;
	
	if(TakeDamageSound != null)
	{
		if(Time.time > nextTime){ //if it has been long enough, and we are either not past our limit or not limited
			nextTime = Time.time + delay; //set next use time
			
			audioSource = GetComponentInChildren.<AudioSource>();
		
			if(audioSource.isPlaying)
				audioSource.Stop();
				
			audioSource.clip = TakeDamageSound;
			audioSource.loop = false;
			audioSource.Play();
		}		
	}
	
	//HighlightGameObject();
	
	//float.TryParse(Arr[0], tempFloat);
	hitPoints -= damage*multiplier;
	if (hitPoints <= 0.0) {
		// Start emitting particles
		var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
		if (emitter)
			emitter.emit = true;

		Invoke("DelayedDetonate", effectDelay);
	}
}

function HighlightGameObject()
{
	GetComponent.<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
	GetComponent.<Renderer>().material.SetFloat("_Outline", outlineSize);
 	GetComponent.<Renderer>().material.SetColor("_OutlineColor", outlineColor);
	
	Invoke("HighlightGameObjectRemove",3);
}

function HighlightGameObjectRemove()
{
	GetComponent.<Renderer>().material.shader = currentShader;
}

function DelayedDetonate(){
	BroadcastMessage ("Detonate");
}

function Detonate(){
	if(isEnemy)
		EnemyMovement.enemies--;
	// Create the deathEffect
	if (deathEffect)
		Instantiate (deathEffect, transform.position, transform.rotation);

	// If we have a dead replacement then replace ourselves with it!
	if (deadReplacement) {
		var dead : Rigidbody;
		
		if(DamageTarget == null)
		{
			dead = Instantiate(deadReplacement, transform.position, transform.rotation);

			// For better effect we assign the same velocity to the exploded gameObject
			dead.GetComponent.<Rigidbody>().velocity = GetComponent.<Rigidbody>().velocity;
			dead.angularVelocity = GetComponent.<Rigidbody>().angularVelocity;
		}
		else //there is a damage target
		{
			dead = Instantiate(deadReplacement, DamageTarget.transform.position, DamageTarget.transform.rotation);

			// For better effect we assign the same velocity to the exploded gameObject
			dead.GetComponent.<Rigidbody>().velocity = DamageTarget.GetComponent.<Rigidbody>().velocity;
			dead.angularVelocity = DamageTarget.GetComponent.<Rigidbody>().angularVelocity;
		}
	}
	
	// If there is a particle emitter stop emitting and detach so it doesnt get destroyed right away
	var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
	if (emitter){
		emitter.emit = false;
		emitter.transform.parent = null;
	}
	BroadcastMessage ("Die", SendMessageOptions.DontRequireReceiver);
	
	if(DamageTarget == null)
		Destroy(gameObject);
	else
		Destroy(DamageTarget);

}