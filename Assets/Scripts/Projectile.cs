using System;
using System.Threading;
using UnityEngine;
using System.Collections;

public enum ProjectileType
{
    Snowball
}



public class Projectile : MonoBehaviour
{
    public ProjectileType Type;
    public object Owner;
    public float Knockback;
    public float Damage;

    private ParticleSystem snowParticles;

    private float lifeTime = 0f;

	// Use this for initialization
	void Start ()
	{
	    //snowParticles = GameObject.Find("SnowParticles").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    lifeTime += Time.deltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        if (lifeTime < 0.05f) return;
        if (other.GetComponent<Item>() != null) return;

        switch (Type)
        {
            case ProjectileType.Snowball:
                if (transform.FindChild("SnowParticles") != null)
                {
                    transform.FindChild("SnowParticles").GetComponent<ParticleSystem>().Emit(10);
                    Destroy(transform.FindChild("SnowParticles").gameObject,
                        transform.FindChild("SnowParticles").GetComponent<ParticleSystem>().duration);
                    transform.DetachChildren();
                }

                //snowParticles.Emit(transform.position,Vector3.zero,1f,10f,Color.white);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        gameObject.SetActive(false);

        if (other.transform.name == "Kid" && !(Owner is Player))
        {
            other.GetComponent<Player>().HitByProjectile(this);
                
        }
        if (other.GetComponent<Enemy>() != null && !(Owner is Enemy))
        {
            other.GetComponent<Enemy>().HitByProjectile(this);
        }
    }

    internal void Init(ProjectileType type, Vector3 pos, object owner)
    {
        Type = type;
        Owner = owner;

        if (owner is Enemy)
        {
            Knockback = ((Enemy)owner).CurrentWeapon.Knockback;
            Damage = ((Enemy)owner).CurrentWeapon.Damage;
        }

        if (owner is Player)
        {
            Knockback = ((Player) owner).CurrentWeapon.Knockback;
            Damage = ((Player)owner).CurrentWeapon.Damage;
        }

        transform.position = pos;

        switch (Type)
        {
            case ProjectileType.Snowball:
                GetComponent<SpriteRenderer>().sprite = ProjectileManager.Instance.ProjectileSheet;
                GetComponent<SpriteRenderer>().sprite.name = type.ToString();
                var pm = (GameObject)Instantiate(ProjectileManager.Instance.SnowParticlesPrefab, transform.position, Quaternion.identity);
                pm.transform.parent = transform;
                pm.transform.name = "SnowParticles";
                break;
        }

        lifeTime = 0f;
        gameObject.SetActive(true);
    }
}
