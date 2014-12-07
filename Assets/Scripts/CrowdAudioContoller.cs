using UnityEngine;
using System.Collections;

public class CrowdAudioContoller : MonoBehaviour
{

    public AudioSource source;

    public AudioSource voiceSource;
    public AudioSource musicSource;

    public AudioClip[] crowdRandoms;
    public bool backgroundNoise;
    public bool backgroundMusic;




    // Use this for initialization
    void Start()
    {
        if (source != null && voiceSource != null&&musicSource!=null)
        {
            StartCrowd();
        }
       




    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator QueueCrowd()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 5.0f));

        voiceSource.clip = crowdRandoms[Random.Range(0, crowdRandoms.Length)];
        voiceSource.pitch = Random.Range(0.8f, 1.3f);
        Debug.Log("Playing Random");
        voiceSource.Play();
        StartCoroutine("QueueCrowd");
    }


    void StartCrowd()
    {
       // source.pitch = Random.Range(0.7f, 1.3f);
        source.loop = true;


        if (backgroundNoise)
        {
            source.Play();
        }

        if (backgroundMusic)
        {

            Debug.Log("Starting BG Music");
            musicSource.Play();
        }



        StartCoroutine("QueueCrowd");
    }
}