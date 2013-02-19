using UnityEngine;
using System.Collections;

public class DiscoLightsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Lights1"), "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.easeInOutSine, "time", 5f));
        iTween.RotateAdd(gameObject, iTween.Hash("looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutSine, "time", 2f, "amount", new Vector3(0,320f,0)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
