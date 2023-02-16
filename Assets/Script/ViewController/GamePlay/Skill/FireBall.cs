using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class FireBall : EnemyController
    {
        public GameObject fireBullet;
        public Transform[] direction;
        public GameObject BulletPool;
        GameObject bullet;

        [Header("¼ÆÊ±Æ÷")]

        float persistTime = 1.5f;
        float startTime;

        bool timeOver;
        public bool skillInBoss;

        private void Start()
        {
            startTime = Time.time;
        }

        private void Update()
        {
            base.Update();
            if (skillInBoss == false)
            {
                Timer();
                if (timeOver)
                {
                    timeOver = false;
                    Attack();
                    startTime = Time.time;
                }
            }
        }
        
        public void Attack()
        {
            foreach (var item in direction)
            {
                bullet = Instantiate(fireBullet, transform.position, Quaternion.identity, BulletPool.transform);
                bullet.GetComponent<EnemyBullet>().target = item;
                
                bullet.SetActive(true);
                
            }
        }
        public void Timer()
        {
            if (Time.time - startTime > persistTime)
            {
                timeOver = true;
            }
        }
        public override void Dead()
        {
            Destroy(gameObject);
        }
    }
}
