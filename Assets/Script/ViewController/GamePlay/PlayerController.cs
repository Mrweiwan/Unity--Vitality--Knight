using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueShooting
{
    public class PlayerController : MonoBehaviour,IDamageable
    {
        [Header("角色信息")]
        public int hp;
        public int shield;
        public int energy;

        public int maxShield;
        public int maxEnergy;
        public int maxHp;

        Animator anim;
        Rigidbody2D mRigidbody2D;

        public float speed;

        private Vector3 target;

        private Gun mGun;//角色持有的枪

        public Transform gunPoint;

        public bool canPick;
        bool isRun;
        bool isHit;
        public bool isDead;

        Item tempGun;

        [Header("计时器")]
        float persistTime;
        float startTime;
        float hitStartTIme;
        bool timeOver;
        bool hitTImeOver;

        GameObject mPickGun;
        GameObject pickItem;
        WeaponBag weaponBag;

        enum PickItemState 
        {
            weapon,
            medicine
        }
        private PickItemState pickItemState;
        Vector2 mousePosition;
        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mGun = transform.Find("GunPoint").GetChild(0).GetComponent<Gun>();
            mGun.canRotate = true;
            anim = GetComponent<Animator>();
            
            maxHp = hp;
            maxShield = shield;
            maxEnergy = energy;

            persistTime = 2;
            startTime = Time.time;
            
            

        }
        private void Start()
        {
            GameManager.Instance.IsPlayer(this);
            hp = GameManager.Instance.LoadHealth();
            energy = GameManager.Instance.LoadEnergy();
            weaponBag = transform.GetChild(0).GetComponent<WeaponBag>();
        }
        private void Update()
        {
            if (isDead == false)
            {
                EscapeAttack();
                AutoRecoverShield();
                if (mRigidbody2D.velocity != Vector2.zero)
                {
                    isRun = true;
                }
                else
                {
                    isRun = false;
                }
                anim.SetBool("Run", isRun);
                SetLookAt();
                //输入监听
                if (Input.GetMouseButtonDown(0))
                {
                    if (energy > 0 || mGun.consume == 0)
                    {

                        mGun.Shoot();
                        energy -= mGun.consume;
                        if (energy < 1)
                        {
                            energy = 0;
                        }
                    }
                }
                //监听键盘E键，并且在可拾取枪械状态下按下，执行拾取枪械的操作
                if (Input.GetKeyDown(KeyCode.F) && canPick)
                {
                    switch (pickItemState) 
                    {
                        case PickItemState.weapon:
                            PickGun(mPickGun);
                            break;
                        case PickItemState.medicine:
                            pickItem.GetComponent<Medicine>().UseMedicine(this);
                            break;
                    }
                    
                }
                if (mGun.canRotate)
                {
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mGun.RotateGun(mousePosition);
                }
            }
            
        }
        private void FixedUpdate()
        {
            if (isDead == false)
            {
                Move();
            }
        }
        /// <summary>
        /// 角色移动
        /// </summary>
        public void Move()
        {
            //获取移动输入
            var horizontalMovement = Input.GetAxis("Horizontal");
            var verticalMovement = Input.GetAxis("Vertical");

            mRigidbody2D.velocity = new Vector2(horizontalMovement, verticalMovement) * speed * Time.fixedDeltaTime;
        }

        /// <summary>
        /// 设置角色看向的方向
        /// </summary>
        public void SetLookAt() 
        {

            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (transform.position.x < target.x && transform.localScale.x < 0 ||
                transform.position.x > target.x && transform.localScale.x > 0)
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
        /// <summary>
        /// 拾取枪械
        /// </summary>
        /// <param name="gun">枪械对象</param>
        void PickGun(GameObject gun)
        {
            if (gun!=null)
            {
                if (!gun.transform.parent.CompareTag("Enemy"))
                {
                    tempGun = gun.GetComponent<Gun>().thisItem;
                    if (!GameManager.Instance.weaponBag.itemList.Contains(tempGun))
                        
                        if (GameManager.Instance.weaponBag.itemList.Count < 2)
                        {
                            GameManager.Instance.weaponBag.itemList.Add(tempGun);
                            weaponBag.currrentGunNum++;
                            weaponBag.SetGun(weaponBag.currrentGunNum);
                            Destroy(gun);
                        }
                        else 
                        {
                            
                            gun.GetComponent<Gun>().thisItem = mGun.thisItem;
                            
                            gun.GetComponent<SpriteRenderer>().sprite = mGun.thisItem.itemImage;
                            gun.GetComponent<Gun>().consume = mGun.thisItem.cosume;
                            gun.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mGun.thisItem.bulletImage;
                            gun.transform.GetChild(0).GetComponent<Bullet>().bulletHurt = mGun.thisItem.hurt;
                            GameManager.Instance.weaponBag.itemList[weaponBag.currrentGunNum] = tempGun;
                            
                            weaponBag.SetGun(weaponBag.currrentGunNum);
                            

                        }
                   
                }
            }
            
        }
        public void AutoRecoverShield() 
        {
            if (isHit == false && (shield != maxShield))
            {
                if (timeOver)
                {
                    startTime = Time.time;
                    timeOver = false;
                    shield = recover(shield, maxShield, 1);
                }
                else
                {
                    if (Time.time - startTime > persistTime)
                    {
                        timeOver = true;
                    }
                }
            }
            else 
            {
                startTime = Time.time;
            }
        }

        public int recover(int currentValue,int maxValue,int recoverValue) 
        {
            currentValue += recoverValue;
            if (currentValue > maxValue) 
            {
                currentValue = maxValue;
            }

            return currentValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Gun"))
            {
                mPickGun = collision.gameObject;
                pickItemState = PickItemState.weapon;
                
            }
            if (collision.gameObject.CompareTag("Medicine"))
            {
                pickItem = collision.gameObject;
                pickItemState = PickItemState.medicine;

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Gun"))
            {
                mPickGun = null;
            }
        }

        void EscapeAttack() //脱战
        {
            if (hitTImeOver)
            {
                isHit = false;
                hitTImeOver = false;
            }
            else 
            {
                if (Time.time - hitStartTIme > 3)
                {
                    hitTImeOver = true;
                }
            }

        }
        public void Dead() 
        {
            mGun.gameObject.SetActive(false);
            transform.GetComponent<Collider2D>().enabled = false;
            mRigidbody2D.bodyType = RigidbodyType2D.Static;
            UIManager.instance.GameOver();
        }
        /// <summary>
        /// 有护盾先扣护盾，没护盾再扣血
        /// </summary>
        /// <param name="damage"></param>
        public void GetHit(int damage)//受击
        {
            isHit = true;
            hitStartTIme = Time.time;
            if (shield > 0)
            {
                shield -= damage;
                if (shield < 1) 
                {
                    shield = 0;
                }
            }
            else if (shield == 0) 
            {
                hp -= damage;
                if (hp < 1) 
                {
                    hp = 0;
                    anim.SetBool("Dead", true);
                    isDead = true;
                    Dead();
 
                }
            }
        }
    }
}
