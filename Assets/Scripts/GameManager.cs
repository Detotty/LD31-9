using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;

public enum GameState
{
    Title,
    InGame,
    GameOver
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameState State;

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
    public Text TeamBodyCountText;
    public int TeamBodyCount;



    public Transform P1;
    public Transform P2;

    public Transform P1UI;
    public Transform P2UI;

    public Image Title;

    List<EnemyType> spawnList = new List<EnemyType>();
    private float startTimer = 0f;
    private float spawnTimer = 0f;

    private float gameOverTimer = 0f;
    
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

        State = GameState.Title;

        CreateSpawnList();

        playerOneHealthSlider.maxValue = 1000;
        playerOneHealthSlider.value = 1000;

       
	}
	
	// Update is called once per frame
	void Update ()
	{
        Title.gameObject.SetActive(false);
        P1UI.gameObject.SetActive(false);
        P2UI.gameObject.SetActive(false);
        gameOverObject.SetActive(false);
        WaveText.gameObject.SetActive(false);
        WaveStartCounterText.gameObject.SetActive(false);

	    switch (State)
	    {
	        case GameState.Title:
                Title.gameObject.SetActive(true);
                P1.gameObject.SetActive(false);
                P2.gameObject.SetActive(false);
	            if (Input.GetButtonDown("P1 Weapon"))
	            {
	                P1.gameObject.SetActive(true);
                    State = GameState.InGame;
	            }
                if (Input.GetButtonDown("P2 Weapon"))
                {
                    P2.gameObject.SetActive(true);
                    State = GameState.InGame;
                }
	            break;
	        case GameState.InGame:
                if (P1.gameObject.activeSelf) P1UI.gameObject.SetActive(true);
                else P1UI.gameObject.SetActive(false);
                if (P2.gameObject.activeSelf) P2UI.gameObject.SetActive(true);
                else P2UI.gameObject.SetActive(false);

                WaveText.gameObject.SetActive(true);
                WaveStartCounterText.gameObject.SetActive(true);

                if (Input.GetButtonDown("P1 Weapon")) P1.gameObject.SetActive(true);
                if (Input.GetButtonDown("P2 Weapon")) P2.gameObject.SetActive(true);

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
                    bool go = true;

                    if (P1.gameObject.activeSelf && !P1.GetComponent<Player>().Dead) go = false;
                    if (P2.gameObject.activeSelf && !P2.GetComponent<Player>().Dead) go = false;

                    if (go == false) 
                    { 
                        // Reset for Next Wave
                        if (getEnemyCount() == 0)
                        {
                            StartWave();
               
                            if (Wave > 0)
                            {
                                waveDefaultText = "You win this wave!";
                            }
                
                
                        }
                    }
                    else
                    {
                        State = GameState.GameOver;
                        


                    }
        
	            }

                if (TeamBodyCountText != null)
                {

                    TeamBodyCountText.text = "BodyCount: " + TeamBodyCount;


                }
	            break;
	        case GameState.GameOver:
                gameOverObject.SetActive(true);

	            gameOverTimer += Time.deltaTime;
	            if (gameOverTimer >= 5f)
	            {
	                Restart();
	            }

	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }

	    
	   

	   


	}

    public void Restart()
    {
        Debug.Log("Restart pressed");
       
        Application.LoadLevel(Application.loadedLevel);

        

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
            if(Wave>=2)
                if(Random.Range(0,3)==0) newE = EnemyType.Snowman;
            if (Wave >= 3)
                if (Random.Range(0, 5) == 0) newE = EnemyType.IceQueen;

            spawnList.Add(newE);
        }
    }
}
