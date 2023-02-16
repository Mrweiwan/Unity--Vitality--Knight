using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueShooting
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private PlayerController player;
        public Inventory weaponBag;
        private WeaponBag weapons;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        public void IsPlayer(PlayerController controller)
        {
            player = controller;
        }
        public void IsWeapons(WeaponBag temp)
        {
            weapons = temp;
        }
        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            weaponBag.itemList.Clear();
            SceneManager.LoadScene(1);
        }
        public void NextLevel()
        {
            SaveDate();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public int LoadHealth()
        {
            if (!PlayerPrefs.HasKey("PlayerHealth"))
            {
                PlayerPrefs.SetInt("PlayerHealth", player.maxHp);
            }
            int currentHealth = PlayerPrefs.GetInt("PlayerHealth");
            return currentHealth;
        }
        public int LoadCurrGun()
        {
            if (!PlayerPrefs.HasKey("PlayerCurrGun"))
            {
                PlayerPrefs.SetInt("PlayerCurrGun", 0);
            }
            int currGun = PlayerPrefs.GetInt("PlayerCurrGun");
            return currGun;
        }
        public int LoadEnergy()
        {
            if (!PlayerPrefs.HasKey("PlayerEnergy"))
            {
                PlayerPrefs.SetInt("PlayerEnergy", player.maxEnergy);
            }
            int currentEnergy = PlayerPrefs.GetInt("PlayerEnergy");
            return currentEnergy;
        }
        public void ContinueGame()
        {
            if (PlayerPrefs.HasKey("sceneIndex"))
                SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
            else
                NewGame();
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public void SaveDate()
        {
            PlayerPrefs.SetInt("PlayerHealth", player.hp);
            PlayerPrefs.SetInt("PlayerEnergy", player.energy);
            PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("PlayerCurrGun", weapons.currrentGunNum);
            PlayerPrefs.Save();
        }
    }
}
