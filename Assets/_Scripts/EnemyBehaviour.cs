using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]

public class EnemyBehaviour : MonoBehaviour
{
    public AudioClip dieSound;
	public AudioClip attackSound;
	public AudioClip chargeUpSound;

    public GameObject mainCamera;
	
	public static float bonusSpeed;

	public class Target 
	{
		public Transform tran;	
		public Vector3 offset;
		public float   distance;
		public Hookable hookable;

		
		public Target (Transform newPosition)
		{
			tran = newPosition;
			hookable = tran.GetComponent<Hookable>();
		}
		
		public void setOffset(Vector3 enemyTransform)
		{
			if (tran != null)
			{
				offset   = tran.position - enemyTransform;
				distance = offset.magnitude;
			}
		}
	}
	
	public float moveSpeed = 2.0f;
	public float gravity = 10.0f;
	public float atackRange = 2.0f;
	public float maxVelocityChange = 0.5f;
	public float attackTime = 1.0f;
	
	private GameObject[] players = null;
	private List<Target> targets = null;
	private float dist;
	private Vector3 offset;
	
	private bool _isAttacking;
	private float _startAttackTime;
	
	void Awake ()
	{
		rigidbody.useGravity = false;
	}

	void Start ()
	{
        mainCamera = GameObject.Find("Main Camera");
		players = GameObject.FindGameObjectsWithTag("Player");
		if (targets == null)
		{
			targets = new List<Target>();
		}
	}
	
	void FixedUpdate ()
	{
		seekAndDestroy();
	}

	void seekAndDestroy()
	{
		targets.Clear();
		foreach (GameObject p in players)
		{
			if (p != null)
				targets.Add(new Target(p.transform));
		}
		
		if (targets.Count > 0)
		{
			if (_isAttacking)
			{
				if (_startAttackTime + attackTime <= Time.time)
					EndAttackingPlayer();
			}
			else
			{
				foreach (Target target in targets)
				{	
					target.setOffset(transform.position);
				}
				
				dist = targets[0].distance;
				offset = targets[0].offset;
				Target attackTarget = targets[0];
				foreach (Target t in targets)
				{
					if(dist > t.distance)
					{
						offset = t.offset;
						dist = t.distance;
						attackTarget = t;
					}
				}
			    
				Vector3 lookRotation;
				lookRotation = offset;
			    lookRotation.y = 0;
			    transform.rotation = Quaternion.LookRotation(lookRotation);
				if (offset.magnitude > atackRange)
				{
					offset.y = 0.0f;
			        Vector3 targetVelocity = Vector3.Normalize(offset) * (moveSpeed + bonusSpeed);
			        Vector3 velocity = rigidbody.velocity;
			        Vector3 velocityChange = (targetVelocity - velocity);
			        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			        velocityChange.y = 0;
			        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
					_isAttacking = false;
				}
				else
				{
					StartAttackingPlayer(attackTarget);
				}
			}
		}
		else
		{
			rigidbody.velocity = Vector3.zero;
		}
		
		if (_isAttacking == true)
		{
			Color color = transform.FindChild("ChargeMeter").particleSystem.startColor;
			
			float progress = (Time.time - _startAttackTime) / attackTime;
			
//			_startAttackTime + attackTime
			
			transform.FindChild("ChargeMeter").particleSystem.startColor = new Color(progress, color.g, color.b, color.a);
			transform.FindChild("ChargeMeter").particleSystem.renderer.enabled = true;
//			_startAttackTime + attackTime	
		}
		else
		{
			Color color = transform.FindChild("ChargeMeter").particleSystem.startColor;
			
			
			
//			_startAttackTime + attackTime
			
			transform.FindChild("ChargeMeter").particleSystem.startColor = new Color(0, color.g, color.b, color.a);	
			transform.FindChild("ChargeMeter").particleSystem.renderer.enabled = false;
		}
	}
	
	public void Died() 
	{
		GetComponent<Hookable>().Died();
		if (transform.FindChild("EnemyDeathParticle") != null)
		{
			Transform particleSystem = transform.FindChild("EnemyDeathParticle");
			particleSystem.parent = null;
			particleSystem.particleSystem.Play();
		}
        
        if (dieSound != null)
        {
            //print("lol?");
//            audio.pitch = Random.Range(0.75f, 1.0f);
//            audio.PlayOneShot(dieSound);
			AudioSource.PlayClipAtPoint(dieSound, Vector3.zero);
            //AudioSource.PlayClipAtPoint(whipSound, transform.position);

            if (mainCamera != null)
            {
                mainCamera.GetComponent<CameraController>().Wobble();
            }
        }

		//Debug.Log("Destroying itself");
		Destroy(gameObject);
	}
	
	void StartAttackingPlayer(Target target)
	{
		if (!_isAttacking)
		{
			audio.pitch = Random.Range(0.75f, 1.0f);
			audio.PlayOneShot(chargeUpSound);
		}
		_isAttacking = true;
		_startAttackTime = Time.time;
	}
	
	void EndAttackingPlayer()
	{
		List<Target> targetsToRemove = new List<Target>();
		foreach (Target t in targets)
		{
			t.setOffset(transform.position);
			if (t.offset.magnitude <= atackRange)
			{
				targetsToRemove.Add(t);
			}
		}
		foreach (Target t in targetsToRemove)
		{
			targets.Remove(t);
			t.tran.GetComponent<PlayerControl>().Died();
		}
		_isAttacking = false;
		
		audio.pitch = Random.Range(0.75f, 1.0f);
		audio.Stop();
		audio.PlayOneShot(attackSound);
		GameObject.Instantiate(ParticleLibrary.instance.enemyAttackExplosion, transform.position, Quaternion.identity);
	}
}