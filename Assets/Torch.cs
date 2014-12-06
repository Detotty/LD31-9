using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour
{

    public float MaxIntensity = 3f;
    public float MinIntensity = 0f;

    public float TargetIntensity;
    private float speed;

	// Use this for initialization
	void Start ()
	{
	    GetComponent<Light>().intensity = Random.Range(MaxIntensity, MinIntensity);

        NewIntensity();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, TargetIntensity,speed);

	    if (Mathf.Abs(GetComponent<Light>().intensity - TargetIntensity) < 0.025f) NewIntensity();

	}

    void NewIntensity()
    {
        TargetIntensity = Random.Range(MaxIntensity, MinIntensity);
        speed = Random.Range(0.2f, 0.3f);
    }
}
