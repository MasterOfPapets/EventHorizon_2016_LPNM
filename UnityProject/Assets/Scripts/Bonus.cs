﻿using UnityEngine;
using System.Collections;
namespace EH.LPNM
{
    public class Bonus : MonoBehaviour
    {
        Player p;
        HudManager hd;
        GameController gc;
        public int BonusPoints;
        public int BonusMultiplier;
	    SoundController sc;
        public int DistancePoint = 1;
        public int DistanceSound = 2;
        private int TakeBonus = 1;
        private int NearlyBonus = 2;
        private int Nothing = 0;
        // Use this for initialization
        void Awake(){
			//GameController.OnBonusTaken += GameController_OnBonusTaken;
            p = FindObjectOfType <Player>();
            gc = FindObjectOfType<GameController>();
            hd = FindObjectOfType<HudManager>();
			sc= FindObjectOfType<SoundController>();
        }

		void Start(){
			
		}

//        void GameController_OnBonusTaken ()
//        {
//			
//        }

       

        // Update is called once per frame
        void Update()
        {
        }
        
		void OnTriggerEnter(Collider other) {
            float Distance = CalculateDistance(Vector3.Distance(this.transform.position, p.transform.position));

            if (p != null && (Distance == TakeBonus))
            {
                Debug.Log("Preso! "+ Vector3.Distance(this.transform.position, p.transform.position));
                sc.HandleOnBonusTaken();
                gc.scoreCounter = gc.scoreCounter + BonusPoints;//aumento lo score
                gc.Multiplier = gc.Multiplier + BonusMultiplier;//aumento del moltiplicatore
                hd.UpdateHud();//aggiorna l'hud
                gc.MultiplierLimiter();//non deve superare il 10
                this.gameObject.SetActive(false);
            }
            else if (Distance==NearlyBonus)
            {
                  sc.OnNearBonus();
                Debug.Log("Quasi vicino "+ Vector3.Distance(this.transform.position, p.transform.position));
            }               

        }

        int CalculateDistance(float DistanceResult)
        {
            if (DistanceResult <= DistancePoint)
            {
                return TakeBonus;
            }
            else if (DistanceResult <= DistanceSound)
            {
                return NearlyBonus;
            }
            else return Nothing;
        }
    }
}
