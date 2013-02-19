using UnityEngine;
using System.Collections;

public class HookShot : MonoBehaviour
{
	public GameObject HookPrefab;
	
	public float HookSpeed = 1;
	public float MinHookDestroyDistance = 1;

    public AudioClip whipSound;
    public AudioClip pullSound;    
	
	public bool HookOut { get; private set; }
	public bool HookRetracting { get; private set; }
	public bool HookAttached { get; private set; }
	public GameObject Hook { get; private set; }
	
	private Hookable _target;
	private bool _firePressed;
	
	void FixedUpdate()
	{
		
//		if (Input.GetButtonDown("Fire1"))
//		{
//			Debug.LogError(Input.GetButtonDown("Fire1"));
//		}
//		else
//		{
////			Debug.LogWarning(Input.GetButtonDown("Fire1"));
//		}
//		
//		if (Input.GetAxis("Fire1") > 0.5f || Input.GetAxis("Fire1") < -0.5f)
//		{
//			Debug.LogError("STUFF WORKS! " + Input.GetAxis("Fire1"));
//		}
		
		float fire = 0;
		
		switch (GetComponent<PlayerControl>().playerID)
		{
		case PlayerID.Player1:
			fire = Input.GetAxis("Fire1");
			break;
		case PlayerID.Player2:
			fire = Input.GetAxis("Fire2");
			break;
		case PlayerID.Player3:
			fire = Input.GetAxis("Fire3");
			break;
		case PlayerID.Player4:
			fire = Input.GetAxis("Fire4");
			break;
		}
		
		
		if (fire <= 0.5f)
		{
			_firePressed = false;
		}
		
		if (fire > 0.5f && !HookRetracting && !_firePressed)
		{
			_firePressed = true;
			if (HookOut)
			{
				StartRetractingHook();
			}
			else
			{
                if (whipSound != null)
                {
                    audio.pitch = Random.Range(0.75f, 1.0f);
                    audio.PlayOneShot(whipSound);
                    //AudioSource.PlayClipAtPoint(whipSound, transform.position);
                }

				ArrowController arrow = transform.GetComponentInChildren<ArrowController>();
				if (arrow.showingArrow == true)
				{
					// Shoot out hook
					HookOut = true;
					HookRetracting = false;
					HookAttached = false;
//					ArrowController arrow = transform.GetComponentInChildren<ArrowController>();
					Hook = GameObject.Instantiate(HookPrefab, transform.position, arrow.transform.rotation) as GameObject;
					Hook.GetComponent<Hook>().SetHookShot(this);
				}
			}
		}
		else if (HookOut)
		{
			bool destroyHook = false;
			
			// Move Hook
			if (HookAttached)
			{
				Hook.rigidbody.velocity = Vector3.zero;
				Hook.transform.position = _target.transform.position;
			}
			
			if (HookRetracting)
			{
				if (!HookAttached)
				{
					Vector3 targetVelocity = (transform.position - Hook.transform.position).normalized * HookSpeed;
					Hook.rigidbody.velocity = targetVelocity;
				}
				
				if (Vector3.Distance(Hook.transform.position, transform.position) < MinHookDestroyDistance)
				{
					HookRetracting = false;
					HookOut = false;
					HookAttached = false;
					Destroy(Hook);
					destroyHook = true;
					
					if (HookAttached)
					{
						_target.CancelHook(this);
					}
				}
			}
			else if (!HookAttached)
			{
				Vector3 targetVelocity = Hook.transform.forward * HookSpeed;
				Hook.rigidbody.velocity = targetVelocity;
			}
			
			if (!destroyHook)
			{
				GameObject rope = Hook.transform.GetChild(0).gameObject;
				if (rope.name.Equals("Rope"))
				{
					rope.transform.position = (Hook.transform.position + transform.position) / 2;
					rope.transform.localScale = new Vector3(rope.transform.localScale.x, rope.transform.localScale.y, 2 * Vector3.Distance(Hook.transform.position, transform.position));
					rope.transform.LookAt(Hook.transform.position);
				}
				else
				{
					Debug.LogError("Hook should only have Rope as child!");
				}
			}
		}
	}
	
	public void AttachHook(Hookable target)
	{
		target.RegisterHook(this);
		HookAttached = true;
		_target = target;
		Hook.collider.enabled = false;
		
		if (HookRetracting)
			StopRetractingHook();
		
		if (!target.IAmPlayer && target.currentHooks.Count > 1)
			GameObject.Instantiate(ParticleLibrary.instance.monsterHook, new Vector3(-8.5f, 11, 0), Quaternion.identity);
	}
	
	public void DetachHook(Hookable target)
	{
		//Debug.Log("Detaching");
		HookRetracting = false;
		HookOut = false;
		HookAttached = false;
		_target = null;
		Destroy(Hook);
	}
	
	public void StopRetractingHook()
	{
		HookRetracting = false;
	}
	
	public void StartRetractingHook()
	{
		// Retract hook (including anything attached to it!)
		HookRetracting = true;
		
		if (HookAttached)
		{
			_target.ActivateHooking(this);
            if (pullSound != null)
            {
                audio.pitch = Random.Range(0.75f, 1.0f);
                audio.PlayOneShot(pullSound);
                //AudioSource.PlayClipAtPoint(pullSound, transform.position);
            }
		}
		else
		{
			Vector3 targetVelocity = (transform.position - Hook.transform.position).normalized * HookSpeed;
        	Hook.rigidbody.velocity = targetVelocity;
		}
	}
	
	public void PlayerDied()
	{
//		Debug.Log(_target + " " + HookOut + " " + HookAttached);
		if (_target != null && HookOut && HookAttached)
		{
			_target.CancelHook(this);
			DetachHook(_target);
		}
	}
	
	public Hookable GetTarget()
	{
		return _target;
	}
}
