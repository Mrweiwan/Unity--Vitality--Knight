using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class HealthBar : MessageBar
    {
        private void Start()
        {
            maxMessage = TempDate.maxHp;
            maxMText.text = maxMessage.ToString();
        }
        private void Update()
        {
            currMessage = player.GetComponent<PlayerController>().hp;
            currMText.text = currMessage.ToString();
            UpdateImage(currMessage,maxMessage,messageSlider,orImage);
        }

    }
}
