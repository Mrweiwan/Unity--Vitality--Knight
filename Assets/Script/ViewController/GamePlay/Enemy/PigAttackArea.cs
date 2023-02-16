using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{

    public class PigAttackArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.parent.GetComponent<PigMonster>().attackSuccess = true;
                collision.GetComponent<PlayerController>().GetHit(1);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.parent.GetComponent<PigMonster>().attackSuccess = false;
                
            }
        }
    }
}
