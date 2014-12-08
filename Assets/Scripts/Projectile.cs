using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.Collections;

public enum ProjectileType
{
    Snowball,
    Carrot,
    Molotov
}



public class Projectile : MonoBehaviour
{
    public ProjectileType Type;
    public object Owner;
    public float Knockback;
    public float Damage;
    public string HitSoundClip;
    public string FiringSoundClip;

    public List<Sprite> Sprites;

    private ParticleSystem snowParticles;

    private float lifeTime = 0f;

    private int faceDir;

	// Use this for initialization
	void Start ()
	{
	    //snowParticles = GameObject.Find("SnowParticles").GetComponent<ParticleSystem>();
        
	}
	
	// Update is called once per frame
	void Update ()
	{
	    lifeTime += Time.deltaTime;

	    transform.localScale = new Vector3(faceDir*5f, 5f, 5f);

        if(Type!= ProjectileType.Carrot) transform.Rotate(0,0,20f);
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
            case ProjectileType.Carrot:
                //if (transform.FindChild("SnowParticles") != null)
                //{
                //    transform.FindChild("SnowParticles").GetComponent<ParticleSystem>().Emit(10);
                //    Destroy(transform.FindChild("SnowParticles").gameObject,
                //        transform.FindChild("SnowParticles").GetComponent<ParticleSystem>().duration);
                //    transform.DetachChildren();
                //}
                break;
            case ProjectileType.Molotov:
                if (transform.FindChild("FireParticles") != null)
                {
                    transform.FindChild("FireParticles").GetComponent<ParticleSystem>().Emit(10);
                    Destroy(transform.FindChild("FireParticles").gameObject,
                        transform.FindChild("FireParticles").GetComponent<ParticleSystem>().duration);
                    transform.DetachChildren();
                }
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
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (owner is Enemy)
        {
            Knockback = ((Enemy)owner).CurrentWeapon.Knockback;
            Damage = ((Enemy)owner).CurrentWeapon.Damage;
            faceDir = ((Enemy) owner).faceDir;
            HitSoundClip = ((Enemy)owner).CurrentWeapon.HitSoundClip;
            FiringSoundClip = ((Enemy)owner).CurrentWeapon.SwingSoundClip;
        }

        if (owner is Player)
        {
            Knockback = ((Player) owner).CurrentWeapon.Knockback;
            Damage = ((Player)owner).CurrentWeapon.Damage;
            faceDir = ((Player)owner).faceDir;
            HitSoundClip = ((Player)owner).CurrentWeapon.HitSoundClip;
            FiringSoundClip = ((Player)owner).CurrentWeapon.SwingSoundClip;
        }

        foreach(Sprite s in Sprites)
            if (s.name == Type.ToString())
                GetComponent<SpriteRenderer>().sprite = s;

        transform.position = pos;

        GameObject pm;
        switch (Type)
        {
            case ProjectileType.Snowball:
                pm = (GameObject)Instantiate(ProjectileManager.Instance.SnowParticlesPrefab, transform.position, Quaternion.identity);
                pm.transform.parent = transform;
                pm.transform.name = "SnowParticles";
                break;
            case ProjectileType.Molotov:
                pm = (GameObject)Instantiate(ProjectileManager.Instance.FireParticlesPrefab, transform.position, Quaternion.identity);
                pm.transform.parent = transform;
                pm.transform.name = "FireParticles";
                break;
        }

        lifeTime = 0f;
        gameObject.SetActive(true);
    }
}
