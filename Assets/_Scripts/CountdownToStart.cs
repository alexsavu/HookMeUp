using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountdownToStart : MonoBehaviour
{
	public float CountdownTime = 5;
	
	private OTTextSprite _spriteText;
	private bool _gameOver;
	private OTTextSprite _spriteScore;
	private float startTime;	
	private float _survivalTime;
	
	// Use this for initialization
	void Start ()
	{
		_spriteText = GetComponent<OTTextSprite>();
		_spriteScore = GameObject.FindGameObjectWithTag("Score").GetComponent<OTTextSprite>();
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_gameOver)
		{
			_spriteText.text = "GAME OVER!";
			if (Input.GetKeyDown("space"))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		else
		{
			if (Time.time < startTime + CountdownTime)
			{
				if (Time.time < startTime + CountdownTime - 4)
					_spriteText.text = "READY!?";
				else if (Time.time < startTime + CountdownTime - 3)
				{
					_spriteText.text = (startTime + CountdownTime - Time.time).ToString("0.0");
					if (!Camera.mainCamera.GetComponentInChildren<AudioSource>().isPlaying)
						Camera.mainCamera.GetComponentInChildren<AudioSource>().Play();
				}
				else if (Time.time < startTime + CountdownTime - 2)
					_spriteText.text = (startTime + CountdownTime - Time.time).ToString("0.00");
				else if (Time.time < startTime + CountdownTime - 1)
					_spriteText.text = (startTime + CountdownTime - Time.time).ToString("0.000");
				else if (Time.time < startTime + CountdownTime - 0.5f)
					_spriteText.text = (startTime + CountdownTime - Time.time).ToString("0.00000");
				else
					_spriteText.text = (startTime + CountdownTime - Time.time).ToString("0.0000000");
			}
			else if (Time.time < startTime + CountdownTime + 1.5f)
			{
				_spriteText.text = "GOOO!!";
			}
			else
			{
				_spriteText.text = "";
				
				_survivalTime += Time.deltaTime;
				_spriteScore.text = _survivalTime.ToString("0");
			}
		}
	}
	
	public void EndGame()
	{
		_gameOver = true;
		_spriteText.wordWrap = 4;
		
		EnemyBehaviour[] enemies = (EnemyBehaviour[])GameObject.FindObjectsOfType(typeof(EnemyBehaviour));
		foreach (EnemyBehaviour enemy in enemies)
		{
			enemy.audio.volume = 0.2f;	
		}
		
		SpawnEnemy[] spawnEnemies = FindObjectsOfType(typeof(SpawnEnemy)) as SpawnEnemy[];
		
		foreach (SpawnEnemy se in spawnEnemies)
		{
			se.enabled = false;
		}
	}
}
