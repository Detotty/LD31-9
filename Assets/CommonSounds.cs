using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;



public class CommonSounds : MonoBehaviour {

	// Use this for initialization

    /**
     * Clips: 
     * 
     * Club,
     * Flame_Stereo,
     * Footsteps_Heavy_Snow,
     * Footsteps_Light_Snow,
     * Molotoc_break_burn,
     * Snow_Man_roar,
     * Grunt_Male_pain
     * 
     * Example call: CommonSoundDictionary["Footsteps_Light_Snow"].Play();
     * 
     */

    // Stats
    public Dictionary<string, AudioSource> CommonSoundDictionary = new Dictionary<string, AudioSource>();  

    void Awake()
    {
       
        foreach (AudioSource a in transform.GetComponents<AudioSource>())
        {
            CommonSoundDictionary.Add(a.clip.name, a);
        }
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
