using UnityEngine;
using System.Collections;

public class TorchAudioController : MonoBehaviour {
    private AudioSource torchAudioSource;
	// Use this for initialization
	void Start () {
        torchAudioSource = GetComponent<AudioSource>();
        StartCoroutine("startAudio");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator startAudio()
    {
       
        yield return new WaitForSeconds(Random.Range(0.0f,1.5f));
        torchAudioSource.loop = true;
       
        torchAudioSource.pitch=Random.Range(0.7f,1.3f);
        torchAudioSource.Play();
        
    }
}
