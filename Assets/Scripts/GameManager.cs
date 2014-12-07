﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public int Wave = 1;
    public float WaveStartDelay = 5f;
    public bool WaveInProgress = false;

    List<EnemyType> spawnList = new List<EnemyType>();
    private float startTimer = 0f;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // dummy enemy list
	    for (int i = 0; i < 5; i++) spawnList.Add(EnemyType.Elf);
	}
	
	// Update is called once per frame
	void Update () {

	    if (!WaveInProgress)
	    {
	        startTimer += Time.deltaTime;
	        if (startTimer >= WaveStartDelay)
	        {
	            startTimer = 0f;
	            WaveInProgress = true;
	        }
	    }

	}
}