using UnityEditor;
using UnityEngine;
using System.Collections;

public class NGJMenu : MonoBehaviour
{
	[MenuItem ("NGJ 2013/Center Selected %g")]
    static void CenterSelected ()
	{
        Selection.activeGameObject.transform.position = new Vector3(0, 0, 0);
    }
}
