using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class PigMonster : EnemyController
    {
        [Header("��ʱ��")]

        float persistTime;//cd
        float startTime;//��ʼʱ��

        Vector2 targetPos;

        public GameObject attackArea;

        public bool attackSuccess;
        public bool timeOver;
        bool isAttack;
        bool one;
        Animator anim;

        private void Awake()
        {
            persistTime = Random.Range(1, 3);
            anim = GetComponent<Animator>();

        }
        private void Update()
        {
            if (HP <= 0)
            {
                anim.SetBool("Dead",true);
                Dead();
            }
            else if (canAttack)
            {
                SetLookAt();
                anim.SetBool("Attack", isAttack);
                if (one == false)
                {
                    if (timeOver)
                    {
                        one = true;
                        GetNewPos();
                    }
                    else
                    {
                        TimeDemo();
                    }
                }
                if (one == true)
                {
                    if (attackSuccess || (transform.position.x == targetPos.x && transform.position.y == targetPos.y))
                    {
                        if (timeOver==false)
                        {
                            isAttack = false;
                            
                        }
                    }
                    
                    if (timeOver && ((attackSuccess || (transform.position.x == targetPos.x && transform.position.y == targetPos.y))))
                    {
                        GetNewPos();
                        startTime = Time.time;
                        persistTime = Random.Range(2, 4);
                        timeOver = false;
                    } 
                    else
                    {
                        
                        GotoTarget(targetPos);
                        TimeDemo();
                    }
                }

            }
            else
            {
                startTime = Time.time;
            }
        }
        public void TimeDemo()
        {

            if (Time.time - startTime > persistTime)
            {
                timeOver = true;
            }

        }

        public void GetNewPos() //����µ�Ŀ��λ��
        {
            isAttack = true;
            targetPos = attackTarget.position;
        }
        public void GotoTarget(Vector2 targetPos) //ȥ��Ŀ��
        {
            
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
        }
        public void SetLookAt()//���ý�ɫ����
        {
            if (transform.position.x < targetPos.x && transform.localScale.x < 0 ||
                transform.position.x > targetPos.x && transform.localScale.x > 0)
            {
                //���Ľ�ɫ����
                var localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;

            }

        }

    }
}
