using System.Threading;
using UnityEngine;
using System.Collections;

public enum ItemType
{
    Weapon
}



public class Item : MonoBehaviour
{
    public ItemType Type;
    public WeaponType WeaponType;

    private float lifeTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    lifeTime += Time.deltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        if (lifeTime < 1f) return;

        if (other.transform.name == "Kid")
        {
            if(other.GetComponent<Player>().Get(this)) 
                gameObject.SetActive(false);
        }
        if (other.GetComponent<Enemy>() != null)
        {
            if (other.GetComponent<Enemy>().Get(this))
                gameObject.SetActive(false);
        }
    }

    internal void Init(ItemType type, WeaponType w)
    {
        Type = type;
        WeaponType = w;

        switch (Type)
        {
            case ItemType.Weapon:

                //GetComponent<SpriteRenderer>().sprite = ItemManager.Instance.WeaponSheet;
                foreach (Sprite s in ItemManager.Instance.Sprites)
                    if (s.name == w.ToString())
                        GetComponent<SpriteRenderer>().sprite = s;
                break;
        }

        

        lifeTime = 0f;
        gameObject.SetActive(true);
    }
}
