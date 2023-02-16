using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueShooting
{
    public class Gun : MonoBehaviour
    {
        Transform mBullet;

        public int consume;
        public Item thisItem;
        public bool canRotate;
        GameObject bullet;
        Vector3 target;
        
        private void Start()
        {
            mBullet = transform.Find("Bullet");
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            //if (canRotate)
            //{
            //    RotateGun();
            //}
            
        }
        public void RotateGun(Vector2 target) 
        {
            float Angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x)
                * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle));
        }

        public void Shoot() 
        {
            var bullet = Instantiate(mBullet, mBullet.transform.position, mBullet.transform.rotation);
            bullet.transform.localScale = mBullet.transform.lossyScale;
            bullet.gameObject.SetActive(true);
        }
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

    }
}
