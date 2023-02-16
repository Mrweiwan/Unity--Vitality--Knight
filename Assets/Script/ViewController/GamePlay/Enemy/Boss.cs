using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class Boss : EnemyController
    {
        [Header("计时器")]
        float persistTime = 3;
        float startTime;

        float x, y;//生成坐标
        Vector2 targetPos;
        bool timeOver;

        public GameObject fire;
        Animator anim;
        public bool bossIsDead;

        delegate void TestDelegate();
        TestDelegate testDelegate;
        TestDelegate[] skills = new TestDelegate[3];

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void Start()
        {
            startTime = Time.time;
            skills[0] = SkillOne;
            skills[1] = SkillTwo;
            skills[2] = SkillThree;
        }

        public override void Update()
        {
            anim.SetBool("Dead", isDead);
            base.Update();
            if (isDead == false && attackTarget.GetComponent<PlayerController>().isDead == false)
            {
                
                SetLookAt();
                
                Timer();
                if (timeOver) 
                {
                    int num = Random.Range(0, skills.Length);

                    testDelegate = skills[num];
                    testDelegate();
                    startTime = Time.time;
                    timeOver = false;
                }
            }
            

            
        }

        public void SkillOne()
        {
            anim.SetTrigger("Trans");
        }
        void SkillTwo() 
        {
            anim.SetTrigger("SkillTwo");
        }
        void SkillThree() 
        {
            anim.SetTrigger("SkillThree");
        }
        public void SetLookAt()//设置角色朝向
        {
            if (transform.position.x < attackTarget.position.x && transform.localScale.x < 0 ||
                transform.position.x > attackTarget.position.x && transform.localScale.x > 0)
            {
                //更改角色朝向
                var localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;

            }


        }
        public void CreatFire() 
        {
            
            for (int i = 0; i < 3; i++) 
            {
                x = Random.Range(-7, 7);//规定x轴方向上的范围
                y = Random.Range(-3, 3);//规定y轴方向上的范围
                if (targetPos != new Vector2(x, y))
                {
                    targetPos = new Vector2(x, y);
                    Instantiate(fire, targetPos, Quaternion.identity);
                }
                else 
                {
                    i--;
                }
            }
        }
        public void TransToTarget( ) 
        {
            transform.position = attackTarget.position;
        }
        public override void Dead()
        {
            bossIsDead = true;
            UIManager.instance.GameWin();
        }

        public void Timer() 
        {
            if (Time.time - startTime > persistTime)
            {
                timeOver = true;
            }
        }

        public void FireBall() 
        {
            testDelegate = transform.GetChild(1).GetComponent<FireBall>().Attack;
            testDelegate();
        }

    }
}
