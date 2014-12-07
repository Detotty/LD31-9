using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour
{

    public float ThrowTime = 1f;

    public bool enableThrow=false;

    private float currentThrowTime = 0f;

    private Transform arenaCenter;

    public Transform Sprite;
    private float turntarget = 5f;
    

	// Use this for initialization
	void Start () {
        arenaCenter = GameObject.Find("Arena").transform.FindChild("Center");

        if (arenaCenter.transform.position.x > transform.position.x)
        {
            turntarget = 5;
           
        }
        if (arenaCenter.transform.position.x < transform.position.x)
        {
            turntarget = -5;
           
        }

        Sprite.localScale =  new Vector3(turntarget, 5, 1f);


	}
	
	// Update is called once per frame
	void Update ()
	{
	    currentThrowTime += Time.deltaTime;
	    if (currentThrowTime >= ThrowTime&& enableThrow)
	    {
	        currentThrowTime = 0f;
	        if (Random.Range(0, 10) == 0)
	        {
	            Item i = ItemManager.Instance.SpawnWeapon(WeaponType.Stick);
	            if (i != null)
	            {
	                i.transform.position = transform.position;
	                Vector3 throwVelocity = ((arenaCenter.position + (Random.insideUnitSphere*7f)) - transform.position);
	               // throwVelocity *= 0.6f;
	                throwVelocity.y = 7f;
                    i.rigidbody.velocity = throwVelocity;
	            }
	        }
	    }
	}
}
