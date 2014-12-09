using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

    const int MAX_CAPACITY = 100;

    public static ItemManager Instance;

    public GameObject ItemPrefab;
    public List<Item> Items = new List<Item>();

    public List<Sprite> Sprites;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < MAX_CAPACITY; i++)
        {
            Item p = ((GameObject)Instantiate(ItemPrefab)).GetComponent<Item>();
            p.transform.parent = transform;
            p.name = "Inactive";
            Items.Add(p);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Item SpawnWeapon(WeaponType type, int durability)
    {
        Item i = Items.FirstOrDefault(it => !it.gameObject.activeSelf);
        if (i == null) return null;

        i.Init(ItemType.Weapon, type, durability);
        return i;
    }

    public Item SpawnFood()
    {
        Item i = Items.FirstOrDefault(it => !it.gameObject.activeSelf);
        if (i == null) return null;

        i.Init(ItemType.Food, WeaponType.Snowball, 0);
        return i;
    }

    internal void Clear()
    {
        foreach (Item p in Items)
        {
            p.gameObject.SetActive(false);
            p.gameObject.name = "Inactive";
        }
    }
}
