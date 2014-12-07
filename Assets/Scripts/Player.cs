﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {

    public Transform HUD;
    public Transform Sprite;
    public Sprite WeaponSheet;
        

	// "Physics"
    public float walkSpeed = 0.000001f;
    public float SpeedLimit = 1f;
    public Vector3 Speed;
    public float Accel = 0.1f;
    public float Gravity = 0.1f;

    // Items
    public Weapon CurrentWeapon = null;

    // States
    public bool Knockback = false;

    // Stats
    public Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();  

    private float turntarget = 12f;
    private int faceDir = 1;
    private Vector2 actualSize = new Vector2(4f,4f);
    private float fstepTime = 0f;
    internal float attackCooldown = 0f;

    private tk2dSpriteAnimator legsAnim;
    private tk2dSpriteAnimator torsoAnim;
    private tk2dSpriteAnimator armsAnim;
    private tk2dSpriteAnimator headAnim;
    private tk2dSpriteAnimator hairAnim;
    private tk2dSpriteAnimator clothesAnim;


    void Awake()
    {
        turntarget = actualSize.x;

        legsAnim = transform.FindChild("Legs").GetComponent<tk2dSpriteAnimator>();
        torsoAnim = transform.FindChild("Torso").GetComponent<tk2dSpriteAnimator>();
        armsAnim = transform.FindChild("Arms").GetComponent<tk2dSpriteAnimator>();
        headAnim = transform.FindChild("Head").GetComponent<tk2dSpriteAnimator>();
        hairAnim = transform.FindChild("Hair").GetComponent<tk2dSpriteAnimator>();
        clothesAnim = transform.FindChild("Clothes").GetComponent<tk2dSpriteAnimator>();

        GameObject soundsObject = transform.FindChild("Audio").gameObject;
        foreach (AudioSource a in soundsObject.GetComponents<AudioSource>())
        {
            Sounds.Add(a.clip.name, a);
        }

        SetWeapon(WeaponType.Snowball);
    }

    private void SetWeapon(WeaponType type)
    {
        CurrentWeapon = new Weapon(type);

        transform.FindChild("Weapon_Swipe").gameObject.SetActive(false);
        transform.FindChild("Weapon_Throw").gameObject.SetActive(false);
        //transform.FindChild("Weapon_Use").gameObject.SetActive(false);

        switch (CurrentWeapon.Class)
        {
            case WeaponClass.Melee:
                transform.FindChild("Weapon_Swipe").gameObject.SetActive(true);
                transform.FindChild("Weapon_Swipe").gameObject.GetComponent<SpriteRenderer>().sprite.name = CurrentWeapon.Type.ToString();
                break;
            case WeaponClass.Throw:
                transform.FindChild("Weapon_Throw").gameObject.SetActive(true);
                transform.FindChild("Weapon_Throw").gameObject.GetComponent<SpriteRenderer>().sprite.name = CurrentWeapon.Type.ToString();
                break;
            case WeaponClass.Use:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    void Update()
    {
        //Input
        float h = Input.GetAxis("P1 Horizontal");
        float v = Input.GetAxis("P1 Vertical");

        if (h > 0f)
        {
            turntarget = actualSize.x;
            faceDir = 1;
        }
        if (h < 0f)
        {
            turntarget = -actualSize.x;
            faceDir = -1;
        }

        //Speed = walkSpeed;
        //rigidbody.velocity = transform.TransformDirection(new Vector3(h, 0, v).normalized) * walkSpeed;

        if (!Knockback && new Vector3(h, 0, v).normalized.magnitude>0f)
            rigidbody.velocity = transform.TransformDirection(new Vector3(h, 0, v).normalized) * walkSpeed;

        if (Knockback && rigidbody.velocity.magnitude < 0.3f)
        {
            Knockback = false;
        }

        //rigidbody.AddForce(new Vector3(h, 0, v).normalized);

        //Speed += new Vector3(h, 0, v).normalized * Accel;
        //Speed = Vector3.ClampMagnitude(Speed, SpeedLimit);
        //transform.position += Speed;
        //Speed = Vector3.Lerp(Speed, Vector3.zero, 0f)



        if (rigidbody.velocity.magnitude > 0f)
        {
            fstepTime += Time.deltaTime;
            if (fstepTime > 0.1f)
            {
                //Sounds["fstep"].Play();
                fstepTime = 0f;
            }
        }

        ToggleWalk(rigidbody.velocity.magnitude > 0.01f);
     
        Sprite.localScale = Vector3.Lerp(Sprite.transform.localScale, new Vector3(turntarget, actualSize.y, 1f), 0.25f);

        attackCooldown -= Time.deltaTime;
        if (Input.GetButtonDown("P1 Weapon") && CurrentWeapon!=null && attackCooldown<=0f)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Melee:
                    transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
                    AttackAnim("Attack");
                    break;
                case WeaponClass.Throw:
                    transform.FindChild("Weapon_Throw").GetComponent<Animation>().Play("Weapon_Throw");
                    AttackAnim("Attack");
                    break;
            }

            attackCooldown = CurrentWeapon.Cooldown;
            StartCoroutine("DoAttack");
        }
        if (Input.GetButtonDown("P1 Throw") && CurrentWeapon != null)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Melee:
                    //transform.FindChild("Weapon_Swipe").gameObject.SetActive(false);
                    break;
            }
            SetWeapon(WeaponType.Snowball);

            Item i = ItemManager.Instance.SpawnWeapon(WeaponType.Stick);
            if (i != null)
            {
                i.transform.position = transform.position + new Vector3((float)faceDir*1f,1f,0f);
                Vector3 throwVelocity = new Vector3((float)faceDir*7f,3f,0f);
                i.rigidbody.velocity = throwVelocity;
            }

          

        }


        if (CurrentWeapon != null && CurrentWeapon.Durability == 0)
        {
            SetWeapon(WeaponType.Snowball);
        }

    }

    private IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(0.1f);

        switch (CurrentWeapon.Class)
        {
            case WeaponClass.Melee:
                Vector3 testPos = transform.position + new Vector3((float)faceDir * 0.5f, 1f, 0f);
                foreach(Enemy e in EnemyManager.Instance.Enemies)
                    if (Vector3.Distance(testPos, e.transform.position) < CurrentWeapon.Range)
                    {
                        e.HitByMelee(this);
                        CurrentWeapon.Durability--;
                    }
                break;
            case WeaponClass.Throw:
                Projectile p = ProjectileManager.Instance.Spawn(CurrentWeapon.ProjectileType, transform.position + new Vector3((float)faceDir * 0.3f, 1f, 0f), this);
                if (p != null)
                {
                    Vector3 throwVelocity = (transform.position + new Vector3((float)faceDir * (CurrentWeapon.Range * 0.5f), 0f, 0f) - transform.position);
                    throwVelocity *= CurrentWeapon.Range * 0.5f;
                    throwVelocity.y = CurrentWeapon.Range;
                    p.rigidbody.velocity = throwVelocity;

                    CurrentWeapon.Durability--;
                }
                break;
            case WeaponClass.Use:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void ToggleWalk(bool walk)
    {
        if (walk)
        {
            legsAnim.Play("Legs_Walk");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_1");
            hairAnim.Play("Hair_1");

            if (!armsAnim.IsPlaying("Arms_Attack"))
                armsAnim.Play("Arms_Walk");
            if (!clothesAnim.IsPlaying("Clothes_Red_Attack"))
                clothesAnim.Play("Clothes_Red_Walk");

            if (CurrentWeapon != null)
                switch (CurrentWeapon.Class)
                {
                    case WeaponClass.Melee:
                        if (!transform.FindChild("Weapon_Swipe").GetComponent<Animation>().isPlaying)
                        {
                            transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Walk");
                            Sounds["Footsteps_Light_Snow"].Play();
                        }
                        break;
                    case WeaponClass.Throw:
                        if (!transform.FindChild("Weapon_Throw").GetComponent<Animation>().isPlaying)
                        {
                            transform.FindChild("Weapon_Throw").GetComponent<Animation>().Play("Weapon_Walk");
                            Sounds["Footsteps_Light_Snow"].Play();
                        }
                        break;
                }
            Sounds["Footsteps_Light_Snow"].Play();   
        }
        else
        {
            legsAnim.Play("Legs_Idle");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_1");
            hairAnim.Play("Hair_1");

            if (!armsAnim.IsPlaying("Arms_Attack"))
                armsAnim.Play("Arms_Idle");
            if (!clothesAnim.IsPlaying("Clothes_Red_Attack"))
                clothesAnim.Play("Clothes_Red_Idle");

            transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Stop("Weapon_Walk");
            transform.FindChild("Weapon_Throw").GetComponent<Animation>().Stop("Weapon_Walk");
        }
    }

    void AttackAnim(string anim)
    {
        armsAnim.PlayFromFrame("Arms_Attack",0);
        clothesAnim.PlayFromFrame("Clothes_Red_Attack",0);
        Sounds["Club"].Play();
    }

    public void HitByMelee(Enemy e)
    {
        if (Knockback) return;

        Vector3 hitAngle = (transform.position - e.transform.position);
        hitAngle.y = Random.Range(0.5f, 1.5f);
        hitAngle.Normalize();

        //Vector3 forceHit = (transform.position - p.transform.position).normalized * 20f;
        rigidbody.AddForceAtPosition(hitAngle * 100f, transform.position);
        Knockback = true;

        Sounds["Grunt_Male_pain"].Play();

        //rigidbody.AddExplosionForce(100f,p.transform.position,100f,Random.Range(5f, 10f));
        //Knockback = true;
    }

    internal void HitByProjectile(Projectile projectile)
    {
        Sounds["Grunt_Male_pain"].Play();
    }

    public bool Get(Item item)
    {
        switch (item.Type)
        {
            case ItemType.Weapon:
                if((CurrentWeapon!=null && CurrentWeapon.Type!=WeaponType.Snowball)) return false;

                SetWeapon(item.WeaponType);
                //CurrentWeapon = new Weapon(item.WeaponType);


                //switch (CurrentWeapon.Class)
                //{
                //    case WeaponClass.Melee:
                //        SetWeapon(WeaponType.Snowball);
                //        break;
                //}

                break;
        }

        return true;
    }





 
}
