using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class ShieldBar : MessageBar
    {
        private void Start()
        {
            maxMessage = TempDate.maxShield;
            maxMText.text = maxMessage.ToString();
        }
        private void Update()
        {
            currMessage = player.GetComponent<PlayerController>().shield;
            currMText.text = currMessage.ToString();
            UpdateImage(currMessage, maxMessage, messageSlider, orImage);
        }
    }
}
