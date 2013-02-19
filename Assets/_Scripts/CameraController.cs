using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float maxWobble = 0.50f;
    public float minWobble = 0.25f;
    Vector3 startPosition;
	// Use this for initialization
	void Start () {
        //Wobble();
        startPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonUp("Fire1")){
            Wobble();
        }
	}

    public void Wobble()
    {
        //print("Wobble!");
        iTween.Stop(gameObject);
        gameObject.transform.position = startPosition;
        iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(Random.Range(minWobble, maxWobble), Random.Range(minWobble, maxWobble), 0), "time", 0.5f));
        //iTween.PunchPosition(gameObject, new Vector3(0.75f, 0.75f, 0), 2f);

    }
}
