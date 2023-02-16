using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class Portal : MonoBehaviour
    {
        bool canTrans;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && canTrans)
            {
                GameManager.Instance.NextLevel();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canTrans = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canTrans = false;
            }
        }
    }
}
