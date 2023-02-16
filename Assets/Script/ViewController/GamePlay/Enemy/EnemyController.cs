using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class EnemyController : MonoBehaviour
    {
        public int HP;

        public bool isDead;
        public bool canAttack;
        public Transform attackTarget;

        public virtual void Update()
        {
            if (HP<=0)
            {
                isDead = true;
                Dead();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                var mHurt = collision.GetComponent<Bullet>().bulletHurt;
                HP -= mHurt;
            }
        }

        public virtual void Dead() 
        {
            var demo = transform.parent.GetComponent<EnemyGenerator>().enemies;
            demo.Remove(gameObject);
            
            transform.GetComponent<Collider2D>().enabled = false;
            isDead = true;
        }
    }
}
