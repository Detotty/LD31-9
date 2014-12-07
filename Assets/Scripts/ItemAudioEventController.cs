using UnityEngine;
using System.Collections;

public class ItemAudioEventController : MonoBehaviour {

    private AudioSource source;

	// Use this for initialization
	void Start () {

        source = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Playing Item Event Audio - Trigger");
        if (other.gameObject.name.Contains("Chunk"))
        {



            source.Play();

        }
    }
}
