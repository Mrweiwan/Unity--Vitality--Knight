using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class Burst : MonoBehaviour
    {
        public Animator anim;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().GetHit(2);
            }
        }
        
        public void Complete() 
        {
            gameObject.SetActive(false);
        }
        
    } 
}
