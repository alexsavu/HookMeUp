using UnityEngine;
using System.Collections;

public class EnemyRotate : MonoBehaviour {
	
	public float RotationSpeed = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.up, RotationSpeed * Time.deltaTime);
		renderer.material.color = new Color(Random.value, Random.value, Random.value, 1);
	}
}
