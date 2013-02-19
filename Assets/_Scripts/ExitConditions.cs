using UnityEngine;
using System.Collections;

public class ExitConditions : MonoBehaviour {

	void Update()
	{
		if(!Application.isWebPlayer && Input.GetKeyDown("escape"))
		{
			Application.Quit();
		}
	}
}
