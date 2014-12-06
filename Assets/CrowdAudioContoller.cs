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
        if (source != null && voiceSource != null)
        {
            queueCrowd();
        }
        else
        {
            Debug.Log("Source or VoiceSource is empty.");
        }




    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator queueCrowd()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));

        //source.clip=="";
        source.pitch = Random.Range(0.7f, 1.3f);
        source.loop = true;
        source.Play();
    }


    void StartCrowd()
    {
        source.pitch = Random.Range(0.7f, 1.3f);
        source.loop = true;

        voiceSource.clip = crowdRandoms[Random.Range(0, crowdRandoms.Length)];

        source.Play();


        StartCoroutine("queueCrowd");
    }
}