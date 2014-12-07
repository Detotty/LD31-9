using UnityEngine;
using System.Collections;

public class SnowMan : Enemy {

    internal override void Awake()
    {
        turntarget = actualSize.x;

        legsAnim = transform.FindChild("Legs").GetComponent<tk2dSpriteAnimator>();
        torsoAnim = transform.FindChild("Torso").GetComponent<tk2dSpriteAnimator>();
        armsAnim = transform.FindChild("Arms").GetComponent<tk2dSpriteAnimator>();
        headAnim = transform.FindChild("Head").GetComponent<tk2dSpriteAnimator>();
        
       

        GameObject soundsObject = transform.FindChild("Audio").gameObject;
        foreach (AudioSource a in soundsObject.GetComponents<AudioSource>())
        {
            Sounds.Add(a.clip.name, a);
        }

        p1 = GameObject.Find("Kid").GetComponent<Player>();
        //p2 = GameObject.Find("Kid").GetComponent<Player>();

        arena = GameObject.Find("Arena").transform;

       
    }


    internal override void ToggleWalk(bool walk)
    {
        if (walk)
        {
            legsAnim.Play("Legs_Walk");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_Walk");
           // hairAnim.Play("Hair_" + hairStyle);

            //if (!armsAnim.IsPlaying("Arms_Attack"))
            armsAnim.Play("Arms_Walk");
            //if (!clothesAnim.IsPlaying("Clothes_Attack"))
            //clothesAnim.Play("Clothes_Walk");

          /*  if (CurrentWeapon != null && !transform.FindChild("Weapon_Swipe").GetComponent<Animation>().isPlaying)
            {


                transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Walk");
            }
           * 
           */
        }
        else
        {
            legsAnim.Play("Legs_Idle");
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_Walk");
           // hairAnim.Play("Hair_" + hairStyle);

            //if (!armsAnim.IsPlaying("Arms_Attack"))
            armsAnim.Play("Arms_Idle");
            //if (!clothesAnim.IsPlaying("Clothes_Attack"))
           // clothesAnim.Play("Clothes_Idle");

            //transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Stop("Weapon_Walk");
        }
    }
}
