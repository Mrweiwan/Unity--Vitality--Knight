using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class LongDistanceEnemy : EnemyController
    {
        Vector2 targetPos;
        Transform room;
        bool timeOver = false;
        bool isRun;

        [Header("计时器")]
        
        float persistTime;
        float startTime;


        [Header("移动")]
        public float moveRange;

        Gun mGun;
        Animator anim;
        private void Awake()
        {
            mGun = transform.GetChild(0).GetChild(0).GetComponent<Gun>();
            persistTime = Random.Range(1, 4);
            anim = GetComponent<Animator>();
        }
        private void Start()
        {
            room = transform.parent.parent;
            GetNewPoint();
            
        }
        private void Update()
        {
            
            if (HP <= 0)
            {
                anim.SetBool("Dead", true);
                mGun.gameObject.SetActive(false);
                Dead();
                
            }
            else if (canAttack)
            {
                anim.SetBool("Run", isRun);
                SetLookAt();
                Attack();
                mGun.RotateGun(attackTarget.position);


                if (transform.position.x == targetPos.x && transform.position.y == targetPos.y)
                {
                    GetNewPoint();
                    isRun = false;
                }
                else
                {
                    GotoTarget();
                    isRun = true;
                }
            }
            else 
            {
                startTime = Time.time;
            }
            
        }
        void GotoTarget() 
        { 
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 3 * Time.deltaTime);
        }
        void GetNewPoint() 
        {
            float x = Random.Range(room.position.x - 6, room.position.x + 6);//规定x轴方向上的范围
            float y = Random.Range(room.position.y - 2, room.position.y + 2);//规定y轴方向上的范围
            targetPos = new Vector2(x, y);
        }
        public  void SetLookAt()
        {
            if (transform.position.x < attackTarget.position.x && transform.localScale.x < 0 ||
                transform.position.x > attackTarget.position.x && transform.localScale.x > 0)
            {
                //更改角色朝向
                var localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;


                //更改枪械朝向
                var GunScale = mGun.transform.localScale;
                GunScale.y = -GunScale.y;
                GunScale.x = -GunScale.x;
                mGun.transform.localScale = GunScale;

            }

        }
        public void TimeDemo() 
        {
            
            if (Time.time - startTime > persistTime) 
            {
                timeOver = true;
            }
        }
        public void Attack() 
        {
            if (timeOver)
            {
                mGun.Shoot();
                timeOver = false;
                startTime = Time.time;
                persistTime = Random.Range(2, 4);
                
            }
            else 
            {
                TimeDemo();
            }

        }
       
        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireCube(room.position, new Vector3(15, 7, 0));
        //}
    }
}
