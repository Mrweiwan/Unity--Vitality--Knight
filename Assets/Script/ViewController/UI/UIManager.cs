using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueShooting
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public GameObject RawImageMap;
        public GameObject pauseMenu;
        public GameObject GameOverPanel;
        public GameObject GameWinPanel;
        bool map;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        void Update()
        {
            ShowMap();
            if (Input.GetKeyDown(KeyCode.P)) 
            {
                PauseGame();
            }
        }
        public void ShowMap()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                map = !map;
                RawImageMap.SetActive(map);
            }
        }
        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        public void ReturnMain() 
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        public void GameOver() 
        {
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
        public void GameWin()
        {
            GameWinPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }
}
