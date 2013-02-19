using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour
{
    public float timeToFinishAnim = 2f;
    public float timeToDestroy = 8f;
    public AudioClip soundToPlay;
    public bool punchText = false;
    bool doingAnim = false;
    private float timePassed = 0f;
    // Use this for initialization
    void Start()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash("scale", new Vector3(0, gameObject.transform.localScale.y, 0), "time", 0.7f, "easetype", iTween.EaseType.easeInOutExpo));
        iTween.RotateFrom(gameObject, iTween.Hash("rotation", new Vector3(0, Random.Range(-150, 150), 0), "time", 0.7f, "easetype", iTween.EaseType.easeInOutExpo));
        if (punchText) iTween.PunchRotation(gameObject, iTween.Hash("amount", new Vector3(0, Random.Range(50, 150), 0), "time", 0.7f, "delay", 0.5f));
        
        if (soundToPlay != null)
        {
            //print("lol?");
            audio.pitch = Random.Range(0.9f, 1.0f);
            audio.PlayOneShot(soundToPlay);
            //AudioSource.PlayClipAtPoint(whipSound, transform.position);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if ((timeToFinishAnim < timePassed) && !doingAnim)
        {
            FinishingAnim();
        }

        if (timeToDestroy < timePassed)
        {
            //print("destroying");
            Destroy(gameObject);
        }

        timePassed += Time.deltaTime;
    }

    void FinishingAnim()
    {
        //print("helolo");
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.7f, "easetype", iTween.EaseType.easeInOutExpo));
        iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(0, Random.Range(-150, 150), 0), "time", 0.7f, "easetype", iTween.EaseType.easeInOutExpo));
        doingAnim = true;
    }
}
