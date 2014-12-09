using UnityEngine;
using System.Collections;
using System.Linq;

public class Spectator : MonoBehaviour
{

    public float ThrowTime = 1f;

    public bool enableThrow=false;

    private float currentThrowTime = 0f;

    private Transform arenaCenter;

    public Transform Sprite;
    private float turntarget = 5f;

    private float rotateTarget = 0f;
    

	// Use this for initialization
	void Start () {
        arenaCenter = GameObject.Find("Arena").transform.FindChild("Center");

        if (arenaCenter.transform.position.x > transform.position.x)
        {
            turntarget = 7;
           
        }
        if (arenaCenter.transform.position.x < transform.position.x)
        {
            turntarget = -7;
           
        }

        Sprite.localScale =  new Vector3(turntarget, 7, 1f);


	}
	
	// Update is called once per frame
	void Update ()
	{
        Sprite.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotateTarget), Time.deltaTime * 10f);

	    if (Random.Range(0, 50) == 0)
	    {
	        rotateTarget = Random.Range(-20f, 20f);
	    }

	    currentThrowTime += Time.deltaTime;
	    if (currentThrowTime >= ThrowTime&& enableThrow)
	    {
	        currentThrowTime = 0f;
	        if (Random.Range(0, 100) == 0)
	        {
                WeaponType newWT = WeaponType.Stick;

                if (Random.Range(0, 3) == 0) newWT = WeaponType.Cane;
                if (Random.Range(0, 10) == 0) newWT = WeaponType.Knife;
                if (Random.Range(0, 15) == 0) newWT = WeaponType.Molotov;
                if (Random.Range(0, 25) == 0) newWT = WeaponType.Flamethrower;

	            if (ItemManager.Instance.Items.Count(it => it.gameObject.activeSelf && it.WeaponType == newWT) < 1)
	            {
                    var tmpW = new Weapon(newWT);

	                Item i = ItemManager.Instance.SpawnWeapon(newWT, tmpW.BaseDurability);
	                if (i != null)
	                {
	                    i.transform.position = transform.position;
	                    Vector3 throwVelocity = ((arenaCenter.position + (Random.insideUnitSphere*8f)) - transform.position);
	                    // throwVelocity *= 0.6f;
	                    throwVelocity.y = 9f;
	                    i.rigidbody.velocity = throwVelocity;
	                }
	            }
	        }

	        if (Random.Range(0, 100) == 0)
	        {
	            if (ItemManager.Instance.Items.Count(it => it.gameObject.activeSelf && it.Type == ItemType.Food) < 2)
	            {
                    Item i = ItemManager.Instance.SpawnFood();
                    if (i != null)
                    {
                        i.transform.position = transform.position;
                        Vector3 throwVelocity = ((arenaCenter.position + (Random.insideUnitSphere * 8f)) - transform.position);
                        // throwVelocity *= 0.6f;
                        throwVelocity.y = 9f;
                        i.rigidbody.velocity = throwVelocity;
                    }
	            }
	        }
	    }
	}
}
