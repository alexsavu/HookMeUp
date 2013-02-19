using UnityEngine;
using System.Collections;

public class AxisTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(Input.GetAxisRaw("Horizontal") + " " + Input.GetAxisRaw("Vertical") + " " + Input.GetAxisRaw("Horizontal2") + " " + Input.GetAxisRaw("Vertical2"));
	}
}
