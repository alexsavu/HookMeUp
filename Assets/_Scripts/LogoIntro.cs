using UnityEngine;
using System.Collections;

public class LogoIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		float alpha = Mathf.Sin(Time.time * 3);
		
		
		alpha = alpha / 4;
		alpha = alpha + 0.75f;
		Debug.Log(alpha);
		alpha = Mathf.Clamp(alpha, 0.5f, 1.0f);
		
		
		
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.r, renderer.material.color.r, alpha);
	
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("matScene");	
		}
	}
}
