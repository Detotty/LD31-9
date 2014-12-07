using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour
{

    public float ThrowTime = 1f;

    private float currentThrowTime = 0f;

    private Transform arenaCenter;

	// Use this for initialization
	void Start () {
        arenaCenter = GameObject.Find("Arena").transform.FindChild("Center");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    currentThrowTime += Time.deltaTime;
	    if (currentThrowTime >= ThrowTime)
	    {
	        currentThrowTime = 0f;
	        if (Random.Range(0, 10) == 0)
	        {
	            Item i = ItemManager.Instance.SpawnWeapon(WeaponType.Stick);
	            if (i != null)
	            {
	                i.transform.position = transform.position;
	                Vector3 throwVelocity = ((arenaCenter.position + (Random.insideUnitSphere*7f)) - transform.position);
	                throwVelocity *= 0.6f;
	                throwVelocity.y = 6f;
                    i.rigidbody.velocity = throwVelocity;
	            }
	        }
	    }
	}
}
