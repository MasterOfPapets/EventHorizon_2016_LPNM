﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EH.LPNM
{
    public class HudStart : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadFirstScene()
        {
            SceneManager.LoadScene("ProjectLPNM");

        }
        

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}