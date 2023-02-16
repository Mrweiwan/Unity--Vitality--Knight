using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueShooting
{
    
    public class Medicine : MonoBehaviour
    {
        public enum MedicineState
        {
            HealthMedicine,
            EnergyMedicine
        }
        int temp;
        public MedicineState medicineState;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().canPick = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().canPick = false;
            }
        }

        public void UseMedicine(PlayerController player) 
        {
            switch (medicineState) 
            {
                case MedicineState.HealthMedicine:
                    temp = player.hp + 2;
                    player.hp=Mathf.Min(temp, player.maxHp);
                    break;
                case MedicineState.EnergyMedicine:
                    temp = player.energy + 80;
                    player.energy = Mathf.Min(temp, player.maxEnergy);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
