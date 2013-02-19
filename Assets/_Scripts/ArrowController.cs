using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	public float MoveSpeed = 1;
	public float RotateSpeed = 100;
	private float previousMagnitude;
	private float currentMagnitude;
	
	public bool showingArrow;
	
	
	
	void Update ()
	{
		//float dir =  Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		//transform.RotateAround(Vector3.up, Input.GetAxis("Horizontal") * Time.deltaTime * RotateSpeed);
		
//		transform.rotation = Quaternion.LookRotation(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), Vector3.up);
//		Debug.Log(transform.position.ToString() + "  " + transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		
//		transform.rotation = Quaternion.Euler(new Vector3(0, Time.time * 30, 0));
//		Debug.Log(Input.GetAxis("Horizontal2") + " " + Input.GetAxis("Vertical2"));
		
		float horizontal = 0;
		float vertical = 0;
		switch (transform.root.GetComponent<PlayerControl>().playerID)
		{
		case PlayerID.Player1:
			horizontal = Input.GetAxisRaw("ArrowHorizontal1");
			vertical = Input.GetAxisRaw("ArrowVertical1");
			break;
		case PlayerID.Player2:
			horizontal = Input.GetAxisRaw("ArrowHorizontal2");
			vertical = Input.GetAxisRaw("ArrowVertical2");
			break;
		case PlayerID.Player3:
			horizontal = Input.GetAxisRaw("ArrowHorizontal3");
			vertical = Input.GetAxisRaw("ArrowVertical3");
			break;
		case PlayerID.Player4:
			horizontal = Input.GetAxisRaw("ArrowHorizontal4");
			vertical = Input.GetAxisRaw("ArrowVertical4");
			break;
		}
		
		
		
		currentMagnitude = new Vector2(horizontal, vertical).magnitude;
	
//		if (Vector3(Input.GetAxis("Horizontal2") + " " + Input.GetAxis("Vertical2")).magnitude < 
		
		if (currentMagnitude - previousMagnitude > -0.2f && currentMagnitude > 0.5f && transform.root.GetComponent<HookShot>().HookOut == false)
		{
			transform.rotation = Quaternion.LookRotation(new Vector3(horizontal, 0, -vertical));
			GetComponentInChildren<MeshRenderer>().enabled = true;
			showingArrow = true;
		}
		else
		{
			GetComponentInChildren<MeshRenderer>().enabled = false;	
			showingArrow = false;
		}
		
		previousMagnitude = currentMagnitude;
	}
}
