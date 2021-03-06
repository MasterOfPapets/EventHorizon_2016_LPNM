﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace EH.LPNM{
public class GameController : MonoBehaviour {
	#region
	public delegate void GameEvent();
	//eventi base del gioco.
	public static event GameEvent OnGameStart;
	public static event GameEvent OnGameEnd;
	public static event GameEvent OnLoadLevel;
	public static GameEvent OnLoadLevelComplete;
	public static event GameEvent OnPlayLevel;
	public static event GameEvent OnLevelEnd;

	public static event GameEvent OnGameWin;
	// evento che fa partire il gioco/livello

	public static event GameEvent OnGameOver;
	public static event GameEvent OnNextLevel;
	//eventi per i bonus
	public static event GameEvent OnBonusTaken;
    public static event GameEvent IsMagnetic;
    public static event GameEvent IsShield;

    //eventi per la collisione delle lettere
	public static event GameEvent OnPerfectCollision;
	public static event GameEvent OnGoodCollision;
	public static event GameEvent OnPoorCollision;
	public static event GameEvent OnWrongLetter;

    #endregion
	
	public bool GameIsFreeze;
    public static string LevelName;
    HudManager Hd;
    SoundController sc;
	Player p;
	Letter l;
	InputController iC;
	public int Level ;
	
//	public GameObject[] ObstacleLettersPrefabs;
//	public GameObject[] PlayerPrefabs;
	Vector3 posPlayer;
	float timeToStart= 0.0f;
	public float StartGame; // 
	public int scoreCounter; // Punteggio del gioco
	public int Score4NextLevel;
	private string BonusScore; 
	public float DistanceResult; // Distanza tra il punto di collisone e la lettera
	public int Multiplier;// moltiplicatore generico
    public float CountCollider; //Secondi di invulnerabilità dopo collisione col tubo 
    public string LoadLevel; //variabile nome scena livello successivo
    public bool StopInput = true; //variabile per fermare gli input
	public bool Complete = false; //livello superato o meno


        //public Transform[] LettersSpawnPoints;
        //public float CounterXObstacle;
        //float TimerXObstacle;
        // Use this for initialization

    
                void Awake(){
			Multiplier = 0;
			DontDestroyOnLoad(this.gameObject);

			if(p==null){
				p =FindObjectOfType<Player>();
			}
			if(l == null){
				l = FindObjectOfType<Letter>();
			}
	}

	void Start () {

			iC = FindObjectOfType<InputController>();
            Hd = FindObjectOfType <HudManager>();
			sc = FindObjectOfType<SoundController>();
            StartInputAndTime();
			iC.enabled = false;
		//	TimerXObstacle = 0;
		//  p = GetComponent<Player>();
		//	GameTimer = 0;
	}




	
	
	// Update is called once per frame
	void Update () {

            if (timeToStart <= StartGame){
				//StartGame = StartGame + Time.deltaTime;
				timeToStart = timeToStart + Time.deltaTime;
				Debug.Log("Inizio a contare" + timeToStart);

			}
			else {
				GameIsFreeze= false;
				iC.enabled=true;

			}
		}


    void FixedUpdate() {
            if (p.PlayerLife <= 0)
            {
               // EndLevelComplete();
                StopInputAndTime();
                CompleteLevelActive();
            }

    }


        //Spawn generale
    public static void Spawn(GameObject objectToSpawn, Vector3 positionToSpawn){
		Instantiate (objectToSpawn, positionToSpawn, objectToSpawn.transform.rotation);
			
	}
//	void RandomSpawnObstacle (){
//		// sceglie un indice a caso nell'array LettersPrefabs
//			int randomLetter = Random.Range(0, ObstacleLettersPrefabs.Length);
//		// assegna l'indice scelto al gameobject ItemToSpawn
//			GameObject LetterToSpawn = ObstacleLettersPrefabs [randomLetter];
//		// sceglie un indice a caso nell'array di spawnPoint
//		int randomIndex = Random.Range (0, LettersSpawnPoints.Length -1);
//		// assegna l'indice alla variabile spawnPosition
//		Vector3 spawnPosition = LettersSpawnPoints [randomIndex].transform.position;
//		// esegue lo spawn con i parametri ItemToSpawn e spawnPosition
//		Spawn (LetterToSpawn,spawnPosition);
//
//	}
    
    /// <summary>
    /// Setta la variabile Complete a true se il punteggio raggiunto è sufficiente per superare il livello
    /// </summary>
	public void EndLevelComplete () {
            //	if(p.PlayerLife<=0){
          if (scoreCounter >= Score4NextLevel)
          {
            Complete = true;
		//	SceneManager.LoadScene("LevelTwo");
          }
				//}
         /*   if (scoreCounter >= Score4NextLevel)
            {
                SceneManager.LoadScene("LevelTwo");
                // Application.LoadLevel("GameOver");
                Debug.Log("LevelTwo");
            }
            */
        }
        /// <summary>
        /// Ferma input mouse e tempo del gioco
        /// </summary>
        public void StopInputAndTime()
        {
            StopInput = false;
               if (Time.timeScale == 1.0F)
                     Time.timeScale = 0F;
        }
        /// <summary>
        /// Starta input mouse e tempo del gioco
        /// </summary>
        public void StartInputAndTime()
        {
            StopInput = true;
            Time.timeScale = 1.0F;
        }

       /// <summary>
        /// Visualizza Hud Game Over 
        /// </summary>
   /*     public void CompleteLevelActive()
        {
            Hd.HudGameOver.gameObject.SetActive(true);
        }
        */
		public void CompleteLevelActive()
		{
			if(Complete==true)
			Hd.HudCompleteLevel.gameObject.SetActive(true);
			else
				Hd.HudGameOver.gameObject.SetActive(true);
		
		}

        /// <summary>
        /// Attiva la pausa se inattiva, la disabilita se attiva
        /// </summary>
       public void PauseActive()
        {
            if (Hd.Pa.gameObject.activeSelf==false)
            {
                StopInputAndTime();
                Hd.Pa.gameObject.SetActive(true);
            }
            else
            {              
                Hd.Pa.gameObject.SetActive(false);
                StartInputAndTime();
            }
        }
        enum OnCollisionPoint
		{Perfect,
			Good,
			Ouch
		}
		/// <summary>
		/// Valuta il punteggio e lo assegna in base al voto
		/// </summary>
		public void OnPointsToAdd (CollisionController.Vote vote, float distancePoint){
			Bonus b;
			b = FindObjectOfType<Bonus>();
			switch (vote) {
			case CollisionController.Vote.Perfect :
				sc.GameController_OnPerfectCollision();
				Multiplier = Multiplier +2;
                MultiplierLimiter();
                scoreCounter = scoreCounter +1000* Multiplier;
				BonusScore = "PERFECT!";
				Hd.UpdateHud();

				break;
			case CollisionController.Vote.Good:
				sc.GameController_OnGoodCollision();
                Multiplier = Multiplier +1;
                MultiplierLimiter();
                scoreCounter = scoreCounter +500*Multiplier;
				BonusScore = "GOOD!";
				Hd.UpdateHud();
				break;
			case CollisionController.Vote.Poor:
				sc.GameController_OnPoorCollision();
				Multiplier = 0;
				BonusScore = "POOR!";
				Hd.UpdateHud();
				break;
			case CollisionController.Vote.wrongLetter:
				if(b.IsShield == false){
				sc.GameController_OnWrongLetter();
				Multiplier = 0;
				p.PlayerLife --;
				Hd.UpdateHud();
				BonusScore = "WRONG LETTER!";}
					break;
			default:
				break;
			}
			Hd.OnCollisionVote(BonusScore, DistanceResult);
		}

       public void MultiplierLimiter() {
            if (Multiplier >=10) {
                Multiplier = 10;
            }
        }
			

	}
	}

	
					
