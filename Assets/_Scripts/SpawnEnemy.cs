using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
	public GameObject enemy;
	public float enemyTimeSpawnInterval = 5.0f; 
	public float decreaseSpanwTime = 0.2f;
	public float minimumSpawnTime = 1.0f;
	private float timeSinceLastEnemySpanwed = 0.0f;
	private float elapsedTime = 0.0f;
	public float thresholdSpanwTime = 0.5f;
	public float delaySpawning = 6.0f;
	private float gameStart = 0.0f;
	
	void Awake()	
	{
		gameStart = Time.time;
	}
	
	void Update ()
	{
		if (Time.time - gameStart  > delaySpawning )	
		{
		    timeSinceLastEnemySpanwed = Time.time - elapsedTime;
		    if( timeSinceLastEnemySpanwed >= enemyTimeSpawnInterval - thresholdSpanwTime)
			{
				showSpanwParicles();	
			}
			if( timeSinceLastEnemySpanwed >= enemyTimeSpawnInterval)
			{
				elapsedTime = Time.time;
				Instantiate(enemy, transform.position, Quaternion.identity);
				timeSinceLastEnemySpanwed = 0.0f;
				if (enemyTimeSpawnInterval > minimumSpawnTime)
				{
					enemyTimeSpawnInterval -= decreaseSpanwTime;
					enemyTimeSpawnInterval = Mathf.Max (enemyTimeSpawnInterval, minimumSpawnTime);
					stopSpawnParticles();
				}
			}
		}
	}
	
	void showSpanwParicles()
	{
		if (!particleSystem.isPlaying)
			particleSystem.Play();
	}
	
	void stopSpawnParticles()
	{
		particleSystem.Stop();
	}
}
