using UnityEngine;
using System.Collections;

public class CrowdAudioContoller : MonoBehaviour
{

    public AudioSource source;

    public AudioSource voiceSource;

    public AudioClip[] crowdRandoms;

    

    // Use this for initialization
    void Start()
    {

        source = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator queueCrowd()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.5f));

        //source.clip=="";
        source.pitch = Random.Range(0.7f, 1.3f);
        source.loop = true;
        source.Play();
    }


    void StartCrowd()
    {
        source.pitch = Random.Range(0.7f, 1.3f);
        source.loop = true;
        source.Play();


        StartCoroutine("queueCrowd)");
    }
}