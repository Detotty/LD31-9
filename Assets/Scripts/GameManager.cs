using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public int Wave = 1;
    public float WaveStartDelay = 5f;
    public bool WaveInProgress = false;
    public float SpawnDelay = 0.5f;
    public Text WaveText;
    public Text WaveStartCounterText;
    public Slider playerOneHealthSlider;
    public Slider playerTwoHealthSlider;
    private string waveDefaultText = "Wave: {0}";
    public GameObject gameOverObject;
    public int PlayerOneBodyCount;
    public int PlayerTwoBodyCount; 

  

    List<EnemyType> spawnList = new List<EnemyType>();
    private float startTimer = 0f;
    private float spawnTimer = 0f;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        CreateSpawnList();

        playerOneHealthSlider.maxValue = 1000;
        playerOneHealthSlider.value = 1000;
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
           
	        return;
	    }
	   

	    if (spawnList.Count > 0)
	    {
	        spawnTimer += Time.deltaTime;
	        if (spawnTimer >= SpawnDelay)
	        {
	            spawnTimer = 0f;
	            EnemyManager.Instance.Spawn(spawnList[spawnList.Count-1]);
                spawnList.RemoveAt(spawnList.Count - 1);
                
	        }
            waveDefaultText = "Wave: {0}";
	    }
	    else
	    {
            if (getPlayerOneHealth() > 0) { 
            // Reset for Next Wave
            if (getEnemyCount() == 0)
            {
                StartWave();
               
                if (Wave > 0)
                {
                    waveDefaultText = "You have have won the Round !";
                }
                
                
            }
            }
            else
            {
                gameOverObject.SetActive(true);
            }
        
	    }
	}

    public void Restart()
    {
        Debug.Log("Restart pressed");
       
        //Application.LoadLevel(Application.loadedLevel);

        

    }




    private void StartWave()
    {
        CreateSpawnList();
        WaveInProgress = false;
        Wave++;
        startTimer = 0f;
        
    }

    float getPlayerOneHealth()
    {
        return playerOneHealthSlider.value;
    }



    int getEnemyCount()
    {
        return EnemyManager.Instance.Enemies.Count(en => !en.Dead);

    }



    void FixedUpdate()
    {

        WaveStartCounterText.text = getStartingCount();

       /** if (Wave-1 == 0)
        {
         WaveText.text = string.Format(waveDefaultText,Wave) ;
        }
        else
        {
            WaveText.text = string.Format(waveDefaultText, Wave-1);
        }
        */
        WaveText.text = string.Format(waveDefaultText, Wave);
       
    }

    string getStartingCount()
    {
        if (!WaveInProgress)
        {
         return " Next Wave in: "+Mathf.Floor(WaveStartDelay - startTimer).ToString();
        }
        else return "";

        
    }

    void CreateSpawnList()
    {
        for (int i = 0; i < 5 + Wave; i++)
        {
            EnemyType newE = EnemyType.Elf;
            if(Wave>=1)
                if(Random.Range(0,3)==0) newE = EnemyType.Snowman;

            spawnList.Add(newE);
        }
    }
}
