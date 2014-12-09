using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

class IceQueen : Enemy
{
    internal override void Awake()
    {
        BaseHealth = 20f;
        Health = BaseHealth;
        CanUseWeapons = false;

        base.Awake();

        Target = arena.FindChild("Center").position + (Random.insideUnitSphere * 6f);
        Target.y = 0f;
        CurrentWeapon = new Weapon(WeaponType.IceQueenMelee);
       

    }

    internal override void ToggleWalk(bool walk)
    {
        if (walk && !armsAnim.IsPlaying("Arms_Attack"))
        {
            torsoAnim.Play("Torso_Walk");
            headAnim.Play("Head_Walk");
            headAnim.Play("Arms_Walk");

        }
        else
        {
            if (!armsAnim.IsPlaying("Arms_Attack"))
            {
                torsoAnim.Play("Torso_Idle");
                headAnim.Play("Head_Idle");
                armsAnim.Play("Arms_Idle");
            }
           
        }
    }

    internal override void DoAI()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Melee:

                    if (Vector3.Distance(transform.position, p1.transform.position) < 2f)
                    {
                        //transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
                        AttackAnim("Attack");
                        attackCooldown = CurrentWeapon.Cooldown * CooldownModifier;
                        StartCoroutine("DoAttack");
                    }
                    if (Vector3.Distance(transform.position, p2.transform.position) < 2f)
                    {
                        //transform.FindChild("Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
                        AttackAnim("Attack");
                        attackCooldown = CurrentWeapon.Cooldown * CooldownModifier;
                        StartCoroutine("DoAttack");
                    }
                    break;
            }
        }




        // Movement

        //reset target to avoid stuckness
        if (Vector3.Distance(Target, arena.FindChild("Center").position) > 6f && Random.Range(0, 1000) == 0)
            Target = arena.FindChild("Center").position + (Random.insideUnitSphere * 6f);

        if (Vector3.Distance(transform.position, Target) < 0.01f)
        {
            if (Random.Range(0, 100) == 0)
            {
                Target = arena.FindChild("Center").position + (Random.insideUnitSphere * 6f);
                Target.y = 0f;
            }
            else if (Random.Range(0, 200) == 0)
            {
                int p = Random.Range(0, 2);
                if (p == 0 && p1.gameObject.activeSelf)
                {
                    Target = p1.transform.position + (Random.insideUnitSphere * 1.5f);
                    Target.y = 0f;
                }
                else if (p == 1 && p2.gameObject.activeSelf)
                {
                    Target = p2.transform.position + (Random.insideUnitSphere * 1.5f);
                    Target.y = 0f;
                }

            }
        }

        
    }

    internal override void IdleAnim()
    {
        torsoAnim.Play("Torso_Idle");
        headAnim.Play("Head_Idle");
        armsAnim.Play("Arms_Idle");
    }

    internal override void AttackAnim(string anim)
    {
        armsAnim.PlayFromFrame("Arms_" + anim, 0);
        headAnim.PlayFromFrame("Head_" + anim, 0);
        torsoAnim.PlayFromFrame("Torso_" + anim, 0);
    }


    /**
        * Override these with creature specific sounds 
        * 
        */
    internal override void playPain()
    {
        if (Sounds.ContainsKey("Snow_man_roar"))
            Sounds["Snow_man_roar"].Play();

    }

    internal override void playThrowingWeapon()
    {
        if (Sounds.ContainsKey("Footsteps_Heavy_Snow"))
            Sounds["Footsteps_Heavy_Snow"].Play();
    }

    internal override void playMeleeWeapon()
    {
        if (Sounds.ContainsKey("Club"))
            Sounds["Club"].Play();
    }

    internal override void startWalkingAudio()
    {
        if (Sounds.ContainsKey("Footsteps_Heavy_Snow"))
            playWalkAudio(Sounds["Footsteps_Heavy_Snow"]);
    }

    internal override void stopWalkingAudio()
    {
        if (Sounds.ContainsKey("Footsteps_Heavy_Snow"))
            stopWalkAudio(Sounds["Footsteps_Heavy_Snow"]);
    }
}
