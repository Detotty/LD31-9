using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum EnemyType
{
    Elf,
    Snowman
}

public class Enemy : MonoBehaviour
{
    public EnemyType Type;

    public Transform HUD;
    public Transform Sprite;
    public Sprite WeaponSheet;

    public Vector3 Target;

	// "Physics"
    public float walkSpeed = 0.000001f;
    public float SpeedLimit = 1f;
    public float Speed = 10;

    // Items
    public Weapon CurrentWeapon = null;

    // States
    public bool Knockback = false;

    // Stats
    public Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();

    internal float turntarget = 12f;
    internal Vector2 actualSize = new Vector2(4f,4f);
    internal float fstepTime = 0f;

    internal Transform arena;

    internal tk2dSpriteAnimator legsAnim;
    internal tk2dSpriteAnimator torsoAnim;
    internal tk2dSpriteAnimator armsAnim;
    internal tk2dSpriteAnimator headAnim;
    internal tk2dSpriteAnimator hairAnim;
    internal tk2dSpriteAnimator clothesAnim;

    internal int headStyle = 0;
    internal int hairStyle = 0;

    internal Player p1;
    internal Player p2;

    internal virtual void Awake()
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

        p1 = GameObject.Find("Kid").GetComponent<Player>();
        //p2 = GameObject.Find("Kid").GetComponent<Player>();

        arena = GameObject.Find("Arena").transform;
    }

    internal virtual void Update()
    {


        if (p1.transform.position.x > transform.position.x) turntarget = actualSize.x;
        if (p1.transform.position.x < transform.position.x) turntarget = -actualSize.x;

        Speed = walkSpeed;

        //if (Vector3.Distance(Target, transform.position) > 0.05f)
        //{
        if (Vector3.Distance(Target, transform.position) < 0.1f)
            Target = transform.position;
        else
            rigidbody.velocity = transform.TransformDirection((Target-transform.position).normalized) * Speed;

         
        //}

        

        if (rigidbody.velocity.magnitude > 0f)
        {
            fstepTime += Time.deltaTime;
            if (fstepTime > 0.1f)
            {
                //Sounds["fstep"].Play();
                fstepTime = 0f;
            }
        }

        //if (!anim.IsPlaying("MonsterLadderTransferOn") && !anim.IsPlaying("MonsterLadderTransferOff"))
        //{
        ToggleWalk(rigidbody.velocity.magnitude > 0.5f);
        //}
     
        Sprite.localScale = Vector3.Lerp(Sprite.transform.localScale, new Vector3(turntarget, actualSize.y, 1f), 0.25f);

        //if (Input.GetButtonDown("P1 Weapon") && CurrentWeapon!=null)
        //{
        //    switch (CurrentWeapon.Class)
        //    {
        //        case WeaponClass.Swipe:
        //            transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
        //            break;
        //    }
        //}
        //if (Input.GetButtonDown("P1 Throw") && CurrentWeapon != null)
        //{
        //    switch (CurrentWeapon.Class)
        //    {
        //        case WeaponClass.Swipe:
        //            transform.FindChild("Weapon_Swipe").gameObject.SetActive(false);
        //            break;
        //    }
        //    CurrentWeapon = null;

        //    Item i = ItemManager.Instance.SpawnWeapon(WeaponType.Stick);
        //    if (i != null)
        //    {
        //        i.transform.position = transform.position + new Vector3(turntarget<0f?-1f:1f,1f,0f);
        //        Vector3 throwVelocity = new Vector3(turntarget<0f?-7f:7f,3f,0f);
        //        i.rigidbody.velocity = throwVelocity;
        //    }

        //}


    }

    public void HitByMelee(Player p)
    {
        //Vector3 hitAngle = (p.transform.position - transform.position);
        //hitAngle.y = Random.Range(0.1f, 0.8f);
        //hitAngle.Normalize();
        
        rigidbody.AddExplosionForce(100f,p.transform.position,100f,Random.Range(5f, 10f));
        Knockback = true;
    }

    internal virtual void ToggleWalk(bool walk)
    {
        if (walk)
        {
            legsAnim.Play("Legs_Walk");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_" + headStyle);
            hairAnim.Play("Hair_" + hairStyle);

            if (!armsAnim.IsPlaying("Arms_Attack"))
                armsAnim.Play("Arms_Walk");
            if (!clothesAnim.IsPlaying("Clothes_Attack"))
                clothesAnim.Play("Clothes_Walk");
        }
        else
        {
            legsAnim.Play("Legs_Idle");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_" + headStyle);
            hairAnim.Play("Hair_" + hairStyle);

            if (!armsAnim.IsPlaying("Arms_Attack"))
                armsAnim.Play("Arms_Idle");
            if (!clothesAnim.IsPlaying("Clothes_Attack"))
                clothesAnim.Play("Clothes_Idle");
        }
    }

    public bool Get(Item item)
    {
        switch (item.Type)
        {
            case ItemType.Weapon:
                if(CurrentWeapon!=null) return false;

                CurrentWeapon = new Weapon(item.WeaponType);

                switch (CurrentWeapon.Class)
                {
                    case WeaponClass.Melee:
                        transform.FindChild("Weapon_Swipe").gameObject.SetActive(true);
                        transform.FindChild("Weapon_Swipe").gameObject.GetComponent<SpriteRenderer>().sprite.name = CurrentWeapon.Type.ToString();
                        break;
                }

                break;
        }

        return true;
    }

    

  
}
