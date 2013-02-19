using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hookable : MonoBehaviour
{
	public List<HookShot> currentHooks { get; private set; }
	
	public bool beingPulledIn { get; private set; }
	public HookShot hookedBy { get; private set; }
	public AnimationCurve hookPullStrength;
	
	public bool IAmPlayer { get; private set; }
	
	public HookShot[] _allPlayersHooks;
	
	void Awake()
	{
		currentHooks = new List<HookShot>();
	}
	
	// Use this for initialization
	void Start () {
//		RegisterHook(GameObject.Find("HookShotTest1").GetComponent<HookShot>());
//		RegisterHook(GameObject.Find("HookShotTest2").GetComponent<HookShot>());
//			
//		ActivateHooking(currentHooks[0]);
		_allPlayersHooks = FindObjectsOfType(typeof(HookShot)) as HookShot[];
		
		PlayerControl pc = GetComponent<PlayerControl>();
		if (pc != null)
		{
			IAmPlayer = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
//		if (name == "Player1")
//		{
//			
//			Debug.Log(hookedBy.ToString());	
//		}
		
//		if (name == "HookableTest")
//		{
//			if (Input.GetKeyDown(KeyCode.K))
//			{
//				ActivateHooking(currentHooks[0]);
//			}
//			else if (Input.GetKeyDown(KeyCode.L))
//			{
//				ActivateHooking(currentHooks[1]);	
//			}
//				
//		}
//		
//		if (beingPulledIn == true)
//		{
////			rigidbody.AddForce(
////			transform.position = transform.position - (transform.position - hookedBy.transform.position).normalized * 8 * Time.deltaTime;	
//		}
	
	}
	
	void FixedUpdate()
	{
		if (beingPulledIn)
		{
			if (transform.FindChild("ShockwaveCharacter") != null)
			{
				transform.FindChild("ShockwaveCharacter").particleSystem.renderer.enabled = true;
			}
			
			float distance = Vector3.Distance(transform.position, hookedBy.transform.position);
			
			if (distance < hookedBy.MinHookDestroyDistance)
			{
				hookedBy.DetachHook(this);
				CancelHook(hookedBy);
				rigidbody.velocity = Vector3.zero;
			}
			else
			{
				
				float force = hookPullStrength.Evaluate(distance);
//				if (force < 60)
//				{
//					force = 60;	
//				}
//				
				
				force = 1500;
				rigidbody.velocity = Vector3.zero;
				rigidbody.AddForce(-(transform.position - hookedBy.transform.position).normalized * force);
			
			
//				rigidbody.AddForce(-(transform.position - hookedBy.transform.position) * force);
			}
		}
		else
		{
			if (transform.FindChild("ShockwaveCharacter") != null)
			{
				transform.FindChild("ShockwaveCharacter").particleSystem.renderer.enabled = false;
			}	
		}
//		transform.position = transform.position - (transform.position - hookedBy.transform.position).normalized * 8 * Time.deltaTime;	
	}
				
	
	
	
	/// <summary>
	/// Registers the hook, meaning that you hit this target.
	/// </summary>
	public void RegisterHook(HookShot hookshot)	
	{
		currentHooks.Add(hookshot);
		GameObject.Instantiate(ParticleLibrary.instance.hookAttached, transform.position, Quaternion.identity);
		
		int hookCount = 0;
		foreach (HookShot hs in _allPlayersHooks)
		{
			if (hs.HookOut && hs.HookAttached && hs.GetTarget() != null && hs.GetTarget().IAmPlayer)
				hookCount++;
		}
		
		if (hookCount > 2)
			GameObject.Instantiate(ParticleLibrary.instance.megaHook, new Vector3(-8.5f, 11, 0), Quaternion.identity);
		else if (hookCount > 1)
			GameObject.Instantiate(ParticleLibrary.instance.trippleHook, new Vector3(-8.5f, 11, 0), Quaternion.identity);
		else if (hookCount > 0)
			GameObject.Instantiate(ParticleLibrary.instance.doubleHook, new Vector3(-8.5f, 11, 0), Quaternion.identity);
	}
	
	/// <summary>
	/// Attempts to activate the hook, pulling the target in.
	/// </summary>
	public void ActivateHooking(HookShot hookshot)
	{
		if (currentHooks.Contains(hookshot) == false)
		{
			Debug.LogError("Trying to activate hook while not registered");	
			return;
		}
					
		if (beingPulledIn == true && hookedBy == hookshot)
		{
			Debug.LogWarning("Already pulling in, don't do anything");
			return;
		}
		
//		if (transform.FindChild("ShockwaveCharacter") != null)
//		{
//			transform.FindChild("ShockwaveCharacter").particleEmitter.renderer.enabled = true;
//		}	
		
		if (beingPulledIn == true)
		{
			hookedBy.StopRetractingHook();
			hookedBy = hookshot;
		}
		else
		{
			hookedBy = hookshot;
			beingPulledIn = true;
		}
		
		PlayerControl pc = GetComponent<PlayerControl>();
		if (pc != null)
		{
			pc.enabled = false;
		}
		
		EnemyBehaviour eb = GetComponent<EnemyBehaviour>();
		if (eb != null)
		{
			eb.enabled = false;
		}
	}	
	
	/// <summary>
	/// The hook was cancelled so that it's no longer hitting this target. This also means that it won't be pulling this in anymore, if it was active.
	/// </summary>
	public void CancelHook(HookShot hookshot)
	{
		if (currentHooks.Contains(hookshot) == false)
		{
			Debug.LogError("Trying to activate hook while not registered");
			return;
		}
		//Debug.Log("Point A");
		currentHooks.Remove(hookshot);
		
//		if (transform.FindChild("ShockwaveCharacter") != null)
//		{
//			transform.FindChild("ShockwaveCharacter").particleEmitter.renderer.enabled = false;
//		}
		
		if (beingPulledIn == true && hookedBy == hookshot)
		{
			beingPulledIn = false;
			hookedBy = null;
			
			PlayerControl pc = GetComponent<PlayerControl>();
			if (pc != null)
			{
				pc.enabled = true;
			}
			
			EnemyBehaviour eb = GetComponent<EnemyBehaviour>();
			if (eb != null)
			{
				eb.enabled = true;
			}
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Hookable hookableTarget = collision.gameObject.GetComponent<Hookable>();
		if (beingPulledIn && IAmPlayer && hookableTarget != null && hookableTarget.beingPulledIn && hookableTarget.IAmPlayer)
		{
			GameObject.Instantiate(ParticleLibrary.instance.bodySlamExplosion, (transform.position + hookableTarget.transform.position) / 2, Quaternion.identity);
		}
		else if (beingPulledIn == true && collision.gameObject.GetComponent<EnemyBehaviour>() != null)
		{
			if (!hookableTarget.beingPulledIn)
				collision.gameObject.GetComponent<EnemyBehaviour>().Died();
		}
	}
	
	public void Died()
	{
		for (int i = 0; i < currentHooks.Count; i++)
		{
			if (currentHooks[i] != null)
			{
				currentHooks[i].DetachHook(this);
			}
		}
//		Debug.Log(hookedBy);
//		if (hookedBy != null)
//		{
//			CancelHook(hookedBy);
//		}
	}
}