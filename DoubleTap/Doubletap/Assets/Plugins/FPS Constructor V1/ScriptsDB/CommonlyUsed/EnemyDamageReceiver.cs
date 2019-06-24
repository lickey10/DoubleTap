using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    float hitPoints = 100.0f;
    Transform deathEffect;
float effectDelay = 0.0f;
    private GameObject[] gos;
    float multiplier = 1;
    Rigidbody deadReplacement;
    GameObject DamageTarget; //target to apply damage to
//@HideInInspector
GameObject playerObject;
bool useHitEffect = true;
    AudioClip TakeDamageSound; //the sound to play when we take damage

//@HideInInspector
public bool isEnemy = false;

private float nextTime = 0;
    float delay = 20;

private Shader currentShader;// = ((Renderer)GetComponent.<Renderer>()).sh;
public float outlineSize = 0.01f;
public Color outlineColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyDamage(Object[]  Arr)
    {
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        if (Arr[1] == true)
        {
            if (!playerObject)
            {
                playerObject = GameObject.FindWithTag("Player");
            }
            if (useHitEffect)
            {
                playerObject.BroadcastMessage("HitEffect");
            }
        }

        // We already have less than 0 hitpoints, maybe we got killed already?
        if (hitPoints <= 0.0)
            return;
        float tempFloat;
        tempFloat = float.Parse(Arr[0].ToString());
        //float.TryParse(Arr[0], tempFloat);
        hitPoints -= tempFloat * multiplier;
        if (hitPoints <= 0.0)
        {
            // Start emitting particles
            //var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
            //if (emitter)
            //    emitter.emit = true;

            Invoke("DelayedDetonate", effectDelay);
        }
    }

    public void ApplyDamagePlayer(float damage)
    {
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        if (!playerObject)
        {
            playerObject = GameObject.FindWithTag("Player");
        }
        if (useHitEffect)
        {
            playerObject.BroadcastMessage("HitEffect");
        }

        // We already have less than 0 hitpoints, maybe we got killed already?
        if (hitPoints <= 0.0)
            return;
        //float.TryParse(Arr[0], tempFloat);
        hitPoints -= damage * multiplier;
        if (hitPoints <= 0.0)
        {
            // Start emitting particles
            //var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
            //if (emitter)
            //    emitter.emit = true;

            Invoke("DelayedDetonate", effectDelay);
        }
    }

    private void ApplyDamage(float damage)
    {
        //Info array contains damage and value of fromPlayer boolean (true if the player caused the damage)
        //Find the player if we haven't
        AudioSource audioSource;

        // We already have less than 0 hitpoints, maybe we got killed already?
        if (hitPoints <= 0.0)
            return;

        if (TakeDamageSound != null)
        {
            if (Time.time > nextTime)
            { //if it has been long enough, and we are either not past our limit or not limited
                nextTime = Time.time + delay; //set next use time

                audioSource = GetComponentInChildren<AudioSource>();

                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = TakeDamageSound;
                audioSource.loop = false;
                audioSource.Play();
            }
        }

        //HighlightGameObject();

        //float.TryParse(Arr[0], tempFloat);
        hitPoints -= damage * multiplier;
        if (hitPoints <= 0.0)
        {
            // Start emitting particles
            //var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
            //if (emitter)
            //    emitter.emit = true;

            Invoke("DelayedDetonate", effectDelay);
        }
    }

    private void HighlightGameObject()
    {
        GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        GetComponent<Renderer>().material.SetFloat("_Outline", outlineSize);
        GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);

        Invoke("HighlightGameObjectRemove", 3);
    }

    private void HighlightGameObjectRemove()
    {
        GetComponent<Renderer>().material.shader = currentShader;
    }

    private void DelayedDetonate()
    {
        BroadcastMessage("Detonate");
    }

    private void Detonate()
    {
        if (isEnemy)
            EnemyMovement.enemies--;
        // Create the deathEffect
        if (deathEffect)
            Instantiate(deathEffect, transform.position, transform.rotation);

        // If we have a dead replacement then replace ourselves with it!
        if (deadReplacement)
        {
            Rigidbody dead;

            if (DamageTarget == null)
            {
                dead = Instantiate(deadReplacement, transform.position, transform.rotation);

                // For better effect we assign the same velocity to the exploded gameObject
                dead.GetComponent< Rigidbody > ().velocity = GetComponent< Rigidbody > ().velocity;
                dead.angularVelocity = GetComponent< Rigidbody > ().angularVelocity;
            }
            else //there is a damage target
            {
                dead = Instantiate(deadReplacement, DamageTarget.transform.position, DamageTarget.transform.rotation);

                // For better effect we assign the same velocity to the exploded gameObject
                dead.GetComponent< Rigidbody > ().velocity = DamageTarget.GetComponent< Rigidbody > ().velocity;
                dead.angularVelocity = DamageTarget.GetComponent< Rigidbody > ().angularVelocity;
            }
        }

        // If there is a particle emitter stop emitting and detach so it doesnt get destroyed right away
        //var emitter : ParticleEmitter = GetComponentInChildren(ParticleEmitter);
        //if (emitter)
        //{
        //    emitter.emit = false;
        //    emitter.transform.parent = null;
        //}
        BroadcastMessage("Die", SendMessageOptions.DontRequireReceiver);

        if (DamageTarget == null)
            Destroy(gameObject);
        else
            Destroy(DamageTarget);

    }
}
