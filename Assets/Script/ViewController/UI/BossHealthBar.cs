using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueShooting
{
    public class BossHealthBar : MessageBar
    {
        Boss boss;
        public override void Awake()
        {
            messageSlider = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            orImage = messageSlider.rectTransform.rect.width;
            boss = player.GetComponent<Boss>();
            maxMessage = boss.HP;
        }
        private void Update()
        {
            currMessage = boss.HP;
            UpdateImage(currMessage, maxMessage, messageSlider, orImage);
        }
    }
}
