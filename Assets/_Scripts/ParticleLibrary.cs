using UnityEngine;
using System.Collections;

public class ParticleLibrary : MonoBehaviour {
	
	public GameObject enemyDeath;
	public GameObject hookAttached;
	public GameObject enemyAttackExplosion;
	public GameObject bodySlamExplosion;
	
	public GameObject doubleHook;
	public GameObject trippleHook;
	public GameObject megaHook;
	public GameObject monsterHook;
	
	public static ParticleLibrary instance;
	
	void Awake()
	{
		instance = this;	
	}
}
