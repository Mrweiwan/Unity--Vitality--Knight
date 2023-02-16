using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class EnemyBullet : MonoBehaviour
    {
        Rigidbody2D mRigidbody2D;
        public Transform target;
        Vector3 dir;
        public int bulletSpeed;
        //public int bulletHurt = 1;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();


            dir = target.position - transform.position;
            mRigidbody2D.AddForce(dir.normalized * bulletSpeed, ForceMode2D.Impulse);
            Destroy(gameObject, 5);

        }
        private void Start()
        {
            RotateObj(transform, target.position);
        }
        public void RotateObj(Transform Obj, Vector2 target)
        {
            float Angle = Mathf.Atan2(target.y - Obj.position.y, target.x - Obj.position.x)
                * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle));
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Door"))
            {
                Destroy(gameObject);
            }
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().GetHit(1);
                Destroy(gameObject);
            }
        }
    }
}
