using UnityEngine;
using System.Collections;

public class Spotlight : MonoBehaviour
{

    public Transform Target;

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Target.gameObject.activeSelf) GetComponent<Light>().enabled = true;
        else GetComponent<Light>().enabled = false;

        Quaternion lookTarget = Quaternion.LookRotation((Target.position+new Vector3(0f,0f,1f)) - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, Time.deltaTime * 3f);
	}
}
