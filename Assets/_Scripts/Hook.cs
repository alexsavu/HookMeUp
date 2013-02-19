using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {
    public AudioClip snapSound;
    public AudioClip hitSound;
	public HookShot hookshotParent;
	
	/// <summary>
	/// Sets the parent of the hook, so it knows which collisions to ignore
	/// </summary>
	public void SetHookShot(HookShot hookshot)
	{
		hookshotParent = hookshot;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (hookshotParent != null && hookshotParent.collider != other)
		{
			if (!hookshotParent.HookAttached && other.GetComponent<Hookable>())
			{
				hookshotParent.AttachHook(other.GetComponent<Hookable>());
                if (snapSound != null)
                {
                    audio.pitch = Random.Range(0.75f, 1.0f);
                    audio.PlayOneShot(snapSound);
                    //AudioSource.PlayClipAtPoint(snapSound, other.transform.position);
                }
			}
			else
			{
				hookshotParent.StartRetractingHook();
                if (hitSound != null)
                {
                    audio.pitch = Random.Range(0.85f, 1.0f);
                    audio.PlayOneShot(hitSound);
                    //AudioSource.PlayClipAtPoint(hitSound, other.transform.position);
                }
			}
		}
	}
}