using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class EnergyBar : MessageBar
    {
        private void Start()
        {
            maxMessage = TempDate.maxEnergy;
            maxMText.text = maxMessage.ToString();
        }
        private void Update()
        {
            currMessage = player.GetComponent<PlayerController>().energy;
            currMText.text = currMessage.ToString();
            UpdateImage(currMessage, maxMessage, messageSlider, orImage);
        }
    }
}
