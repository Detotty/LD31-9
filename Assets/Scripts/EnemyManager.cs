using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {


    public static EnemyManager Instance;

    public GameObject ElfPrefab;
    public GameObject SnowmanPrefab;
    public List<Transform> Doors = new List<Transform>(); 

    public List<Enemy> Enemies = new List<Enemy>();

    public Sprite WeaponSheet;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Enemy Spawn(EnemyType type)
    {
        Enemy e = null;

        switch (type)
        {
            case EnemyType.Elf:
                e = ((GameObject)Instantiate(ElfPrefab)).GetComponent<Elf>();
                break;
            case EnemyType.Snowman:
                e = ((GameObject)Instantiate(SnowmanPrefab)).GetComponent<Snowman>();
                break;
        }

        e.transform.parent = transform;
        e.transform.position = Doors[Random.Range(0, 3)].position;
        Enemies.Add(e);

        return e;
    }

    internal void Clear()
    {
        foreach (Enemy e in Enemies)
        {
            e.gameObject.SetActive(false);
            e.gameObject.name = "Inactive";
        }
    }
}
