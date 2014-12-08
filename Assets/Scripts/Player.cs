using System;
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
    public Slider playerHealthSlider;
    public GameObject playerDurabilityObject;
        

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

    // Weapon Sprites
    public List<Sprite> Sprites = new List<Sprite>(); 

    // Stats
    public Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();
    public float playerHealth = 100f;
    private Slider playerDurabilitySlider;

    private float turntarget = 12f;
    public int faceDir = 1;
    private Vector2 actualSize = new Vector2(4f,4f);
    private float fstepTime = 0f;
    internal float attackCooldown = 0f;

    private tk2dSpriteAnimator legsAnim;
    private tk2dSpriteAnimator torsoAnim;
    private tk2dSpriteAnimator armsAnim;
    private tk2dSpriteAnimator headAnim;
    private tk2dSpriteAnimator hairAnim;
    private tk2dSpriteAnimator clothesAnim;



    void Start(){
     playerHealthSlider.maxValue = playerHealth;
    }
    void Awake()
    {
        turntarget = actualSize.x;

        legsAnim = transform.FindChild("Body/Legs").GetComponent<tk2dSpriteAnimator>();
        torsoAnim = transform.FindChild("Body/Torso").GetComponent<tk2dSpriteAnimator>();
        armsAnim = transform.FindChild("Body/Arms").GetComponent<tk2dSpriteAnimator>();
        headAnim = transform.FindChild("Body/Head").GetComponent<tk2dSpriteAnimator>();
        hairAnim = transform.FindChild("Body/Hair").GetComponent<tk2dSpriteAnimator>();
        clothesAnim = transform.FindChild("Body/Clothes").GetComponent<tk2dSpriteAnimator>();

        GameObject soundsObject = transform.FindChild("Audio").gameObject;
        foreach (AudioSource a in soundsObject.GetComponents<AudioSource>())
        {
            Sounds.Add(a.clip.name, a);
        }

        SetWeapon(WeaponType.Snowball);
        playerDurabilitySlider = playerDurabilityObject.transform.FindChild("DurabilitySlider").GetComponent<Slider>();

        if (playerDurabilitySlider != null)
        {
            playerDurabilitySlider.maxValue = CurrentWeapon.BaseDurability;
        }
        else
        {
            Debug.Log("Player.cs --> playerDurabilitySlider is null");
        }

       
    }

    private void SetWeapon(WeaponType type)
    {
        CurrentWeapon = new Weapon(type);

        transform.FindChild("Body/Weapon_Swipe").gameObject.SetActive(false);
        transform.FindChild("Body/Weapon_Throw").gameObject.SetActive(false);
        //transform.FindChild("Weapon_Use").gameObject.SetActive(false);
            
        switch (CurrentWeapon.Class)
        {
            case WeaponClass.Melee:

                transform.FindChild("Body/Weapon_Swipe").gameObject.SetActive(true);
                foreach (Sprite s in Sprites)
                    if (s != null && s.name == CurrentWeapon.Type.ToString())
                        transform.FindChild("Body/Weapon_Swipe").GetComponent<SpriteRenderer>().sprite = s;
                break;
            case WeaponClass.Throw:
                transform.FindChild("Body/Weapon_Throw").gameObject.SetActive(true);
                foreach (Sprite s in Sprites)
                    if (s != null && s.name == CurrentWeapon.Type.ToString())
                        transform.FindChild("Body/Weapon_Throw").GetComponent<SpriteRenderer>().sprite = s;
                break;
            case WeaponClass.Use:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (CurrentWeapon.BaseDurability > 0) {

            if (!playerDurabilityObject.activeSelf)
            {
                playerDurabilityObject.SetActive(true);
            }
        playerDurabilitySlider.maxValue = CurrentWeapon.BaseDurability;
        playerDurabilitySlider.value = CurrentWeapon.BaseDurability;
        }
        else
        {
            playerDurabilityObject.SetActive(false);
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
        if (Input.GetButtonDown("P1 Weapon") && CurrentWeapon!=null && CurrentWeapon.Class!= WeaponClass.Use && attackCooldown<=0f)
        {
            switch (CurrentWeapon.Class)
            {
                case WeaponClass.Melee:
                    transform.FindChild("Body/Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Swipe");
                    AttackAnim("Attack");
                    break;
                case WeaponClass.Throw:
                    transform.FindChild("Body/Weapon_Throw").GetComponent<Animation>().Play("Weapon_Throw");
                    AttackAnim("Attack");
                    break;
            }

            attackCooldown = CurrentWeapon.Cooldown;
            StartCoroutine("DoAttack");
        }

        if (Input.GetButton("P1 Weapon") && CurrentWeapon != null && CurrentWeapon.Class == WeaponClass.Use && attackCooldown <= 0f)
        {
            UseWeapon();
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

        UpdateHealthBar();

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

    void UseWeapon()
    {
        switch (CurrentWeapon.Type)
        {
            case WeaponType.Flamethrower:
                transform.FindChild("Body/Weapon_Use/FlameThrowerParticles").GetComponent<ParticleSystem>().Emit(5);
                break;
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
                        if (!transform.FindChild("Body/Weapon_Swipe").GetComponent<Animation>().isPlaying)
                        {
                            transform.FindChild("Body/Weapon_Swipe").GetComponent<Animation>().Play("Weapon_Walk");
                            startWalkingAudio(Sounds["Footsteps_Light_Snow"]);
                        }
                        break;
                    case WeaponClass.Throw:
                        if (!transform.FindChild("Body/Weapon_Throw").GetComponent<Animation>().isPlaying)
                        {
                            transform.FindChild("Body/Weapon_Throw").GetComponent<Animation>().Play("Weapon_Walk");
                            startWalkingAudio(Sounds["Footsteps_Light_Snow"]);
                        }
                        break;
                }
            startWalkingAudio(Sounds["Footsteps_Light_Snow"]);  
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

            transform.FindChild("Body/Weapon_Swipe").GetComponent<Animation>().Stop("Weapon_Walk");
            transform.FindChild("Body/Weapon_Throw").GetComponent<Animation>().Stop("Weapon_Walk");
            stopWalkingAudio(Sounds["Footsteps_Light_Snow"]);
        }
    }

    private void startWalkingAudio(AudioSource source)
    {

        if (!source.isPlaying)
        {
            source.loop = true;
            source.Play();
        }
    }

    private void stopWalkingAudio(AudioSource source)
    {

        if (rigidbody.velocity.sqrMagnitude < 0.01f)
        {
            //Debug.Log("Stopping Player Audio" + source.clip.name);
            source.loop = false;
        }
    }

    void AttackAnim(string anim)
    {
        armsAnim.PlayFromFrame("Arms_Attack",0);
        clothesAnim.PlayFromFrame("Clothes_Red_Attack",0);
        if (!"".Equals(CurrentWeapon.SwingSoundClip)){
            Sounds[CurrentWeapon.SwingSoundClip].Play();
        }

    }

    public void HitByMelee(Enemy e)
    {
        if (Knockback) return;

        Vector3 hitAngle = (transform.position - e.transform.position);
        hitAngle.y = Random.Range(0.5f, 1.5f);
        hitAngle.Normalize();

        rigidbody.AddForceAtPosition(hitAngle * 100f, transform.position);
        Knockback = true;
        Debug.Log(e.Type+ " Playing " + CurrentWeapon.Type);
        Sounds[e.CurrentWeapon.HitSoundClip].Play();
               
        StartCoroutine("PlayDamagedSound", Sounds[e.CurrentWeapon.HitSoundClip].clip.length + 0.05f);


        transform.FindChild("BloodParticles").GetComponent<ParticleSystem>().Emit(10);

        playerHealth -= e.CurrentWeapon.Damage;

    }

    internal void HitByProjectile(Projectile projectile)
    {

        if (Knockback) return;

        Vector3 hitAngle = (transform.position - projectile.transform.position);
        hitAngle.y = Random.Range(0.5f, 1.5f);
        hitAngle.Normalize();

        rigidbody.AddForceAtPosition(hitAngle * 100f, transform.position);
        Knockback = true;
        Debug.Log("Playing " + projectile.Type);
        Sounds[projectile.HitSoundClip].Play();
        StartCoroutine("PlayDamagedSound",Sounds[projectile.HitSoundClip].clip.length+0.05f);
       

        transform.FindChild("BloodParticles").GetComponent<ParticleSystem>().Emit(10);

        playerHealth -= projectile.Damage;


    }

    IEnumerator PlayDamagedSound(float delay)
    {
        yield return new WaitForSeconds(delay);

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

   

    private void UpdateHealthBar()
    {
        if (playerHealthSlider != null)
        {
            Debug.Log("Player Health: " + playerHealth);
            playerHealthSlider.value = playerHealth;
            Debug.Log("Bar value" + playerHealthSlider.value);
        }
        else
        {
            Debug.Log("playerHealthSlider is null");
        }

        if (playerDurabilitySlider != null)
        {
            //Debug.Log("Player Durability: " + CurrentWeapon.Durability);
            playerDurabilitySlider.value = CurrentWeapon.Durability;
        }
        else
        {
            Debug.Log("playerDurabilitySlider is null");
        }
    }



 
}
