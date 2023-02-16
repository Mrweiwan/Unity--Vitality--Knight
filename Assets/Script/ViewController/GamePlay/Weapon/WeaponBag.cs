using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class WeaponBag : MonoBehaviour
    {
        
        GameObject playerGun;
        GameObject bullet;
        public Item Pistol;

        public int currrentGunNum;
        
        private void Start()
        {
            if (GameManager.Instance.weaponBag.itemList.Count == 0)
            {
                GameManager.Instance.weaponBag.itemList.Add(Pistol);
            }
            currrentGunNum = GameManager.Instance.LoadCurrGun();
            playerGun = transform.GetChild(0).gameObject;
            bullet = transform.GetChild(0).GetChild(0).gameObject;
            GameManager.Instance.IsWeapons(this);
            SetGun(currrentGunNum);
        }
        private void Update()
        {
            switchGun();
        }
        public void switchGun() 
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
               
                if (--currrentGunNum < 0)
                {
                    currrentGunNum = GameManager.Instance.weaponBag.itemList.Count - 1;
                }
                SetGun(currrentGunNum);
                
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (++currrentGunNum > GameManager.Instance.weaponBag.itemList.Count - 1)
                {
                    currrentGunNum = 0;
                }
                SetGun(currrentGunNum);
            }
        }
        public void SetGun(int num) 
        {
            playerGun.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.weaponBag.itemList[num].itemImage;
            playerGun.GetComponent<Gun>().consume = GameManager.Instance.weaponBag.itemList[num].cosume;
            bullet.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.weaponBag.itemList[num].bulletImage;
            bullet.GetComponent<Bullet>().bulletHurt = GameManager.Instance.weaponBag.itemList[num].hurt;
            playerGun.GetComponent<Gun>().thisItem = GameManager.Instance.weaponBag.itemList[num];
        }

    }
}
