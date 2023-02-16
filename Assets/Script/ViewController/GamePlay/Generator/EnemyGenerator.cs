using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class EnemyGenerator : MonoBehaviour
    {
        public GameObject[] enemy;
        public int minNum, maxNum;

        public List<GameObject> enemies = new List<GameObject>();
        void Start()
        {
            if (transform.parent.GetComponent<Room>().isFistRoom==false && transform.parent.GetComponent<Room>().isEndRoom == false)
            {
                CreateEnemy();
            }
           
        }

        void Update()
        {
            
        }
        void CreateEnemy()
        {
            int enemyNum = Random.Range(3,5);
            List<Vector2> creatPoint = new List<Vector2>();
            while (creatPoint.Count < enemyNum) 
            {
                int num = 0;
                float x = Random.Range(transform.position.x - 6, transform.position.x + 6);//规定x轴方向上的范围
                float y = Random.Range(transform.position.y - 2, transform.position.y + 2);//规定y轴方向上的范围
                Vector2 newPoint = new Vector2(x, y);
                if (creatPoint.Count == 0)
                {
                    creatPoint.Add(newPoint);
                }
                else
                {
                    for (int i = 0; i < creatPoint.Count; i++)
                    {
                        float posLength = (newPoint - creatPoint[i]).magnitude;
                        if (posLength < 2) 
                        {
                            num++;
                        }
                    }
                    if (num == 0) 
                    {
                        creatPoint.Add(newPoint);
                    }
                }
                
            }
            for (int i = 0; i < creatPoint.Count; i++) 
            {
                GameObject enemyObj = null;
                int num = Random.Range(0, enemy.Length);
                enemyObj = Instantiate(enemy[num], creatPoint[i], Quaternion.identity, transform);
                enemies.Add(enemyObj);
            }
        }
    }
}
