using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Transform HUD;
    public Transform Sprite;
    public Sprite WeaponSheet;
        

	// "Physics"
    public float walkSpeed = 0.000001f;
    public float SpeedLimit = 1f;
    public float Speed = 10;

    // Items
    public Weapon CurrentWeapon = null;

    // States

    // Stats
    public Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();  

    private float turntarget = 12f;
    private Vector2 actualSize = new Vector2(5f,5f);
    private float fstepTime = 0f;
    private tk2dSpriteAnimator spriteAnim;
    

    void Awake()
    {
        turntarget = actualSize.x;

        //anim = Sprite.GetComponent<tk2dSpriteAnimator>();

        

        GameObject soundsObject = transform.FindChild("Audio").gameObject;
        foreach (AudioSource a in soundsObject.GetComponents<AudioSource>())
        {
            Sounds.Add(a.clip.name, a);
        }
    }

    void Update()
    {
        //Input
        float h = Input.GetAxis("P1 Horizontal");
        float v = Input.GetAxis("P1 Vertical");

        if (h > 0f) turntarget = actualSize.x;
        if (h < 0f) turntarget = -actualSize.x;

        Speed = walkSpeed;
        rigidbody.velocity = transform.TransformDirection(new Vector3(h, 0, v).normalized) * Speed;

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
        //    if (rigidbody.velocity.magnitude > 0f) anim.Play("MonsterWalk");
        //    else anim.Play("MonsterIdle");
        //}
     
        Sprite.localScale = Vector3.Lerp(Sprite.transform.localScale, new Vector3(turntarget, actualSize.y, 1f), 0.25f);

        if (Input.GetButtonDown("P1 Weapon") && CurrentWeapon!=null)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Swipe:
                    transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
                    break;
            }
        }
        if (Input.GetButtonDown("P1 Throw") && CurrentWeapon != null)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Swipe:
                    transform.FindChild("Weapon_Swipe").gameObject.SetActive(false);
                    break;
            }
            CurrentWeapon = null;

            Item i = ItemManager.Instance.SpawnWeapon(WeaponType.Stick);
            if (i != null)
            {
                i.transform.position = transform.position + new Vector3(turntarget<0f?-1f:1f,1f,0f);
                Vector3 throwVelocity = new Vector3(turntarget<0f?-7f:7f,3f,0f);
                i.rigidbody.velocity = throwVelocity;
            }

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
                    case WeaponClass.Swipe:
                        transform.FindChild("Weapon_Swipe").gameObject.SetActive(true);
                        transform.FindChild("Weapon_Swipe").gameObject.GetComponent<SpriteRenderer>().sprite.name = CurrentWeapon.Type.ToString();
                        break;
                }

                break;
        }

        return true;
    }

    

  
}
