using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueShooting
{
    public class Room : MonoBehaviour
    {
        public GameObject doorLeft, doorRight, doorUp, doorDown;
        public bool roomLeft, roomRight, roomUp, roomDown;

        public Text text;

        public int stepToStart;//到初始点的距离

        public int doorNumber;

        public bool isFistRoom;
        public bool isEndRoom;

        Transform treasure;

        Transform enemies;

        private void Start()
        {
            doorLeft.SetActive(roomLeft);
            doorRight.SetActive(roomRight);
            doorUp.SetActive(roomUp);
            doorDown.SetActive(roomDown);

            enemies = transform.Find("Enemies");
            
        }
        public void UpdateRoom(float xOffset, float yOffset) 
        {
            stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
            text.text = stepToStart.ToString();

            if (roomUp)
                doorNumber++;
            if (roomRight)
                doorNumber++;
            if (roomDown)
                doorNumber++;
            if (roomLeft)
                doorNumber++;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CameraController.instance.ChangeTarget(transform);
                
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var doors = transform.GetChild(4).GetComponent<DoorManager>().doors;
                foreach (var item in enemies.GetComponent<EnemyGenerator>().enemies) 
                {
                    item.GetComponent<EnemyController>().attackTarget = collision.transform;
                    if (item.name == "LongDistanceEnemy(Clone)")
                    {
                        item.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<EnemyBullet>().target = collision.transform;
                    }            
                    item.GetComponent<EnemyController>().canAttack = true;
                }
                foreach (var item in doors)
                {
                    if (enemies.GetComponent<EnemyGenerator>().enemies.Count == 0)
                        item.OpenDoor();
                    else 
                        item.CloseDoor();  
                }
                if (enemies.GetComponent<EnemyGenerator>().enemies.Count == 0) 
                {
                    treasure = transform.Find("Treasure(Clone)");
                    
                    if (treasure != null) 
                    {
                        treasure.gameObject.SetActive(true);
                    }
                }


            }
        }
    }
    
}
