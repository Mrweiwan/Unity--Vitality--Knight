using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueShooting
{
    public class Bullet : MonoBehaviour
    {
        Rigidbody2D mRigidbody2D;
        Vector3 mousePos;
        Vector3 dir;

        public int bulletHurt = 1;
        
        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            dir = mousePos - transform.position;

            Destroy(gameObject, 5);

        }

        private void Start()
        {
            mRigidbody2D.AddForce(dir.normalized*20, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag ("Wall") )
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Door"))
            {
                Destroy(gameObject);
            }
        }      
    }
}
