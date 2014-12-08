using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {

    const int MAX_CAPACITY = 1000;

    public static ProjectileManager Instance;

    public GameObject ProjectilePrefab;
    public List<Projectile> Projectiles = new List<Projectile>();

    public GameObject SnowParticlesPrefab;

    public Sprite ProjectileSheet;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < MAX_CAPACITY; i++)
        {
            Projectile p = ((GameObject)Instantiate(ProjectilePrefab)).GetComponent<Projectile>();
            p.transform.parent = transform;
            p.name = "Inactive";
            Projectiles.Add(p);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Projectile Spawn(ProjectileType type, Vector3 pos, object owner)
    {
        Projectile i = Projectiles.FirstOrDefault(it => !it.gameObject.activeSelf);
        if (i == null) return null;

        i.Init(type, pos, owner);
        return i;
    }

    internal void Clear()
    {
        foreach (Projectile p in Projectiles)
        {
            p.gameObject.SetActive(false);
            p.gameObject.name = "Inactive";
        }
    }
}
