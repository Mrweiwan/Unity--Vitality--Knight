using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueShooting
{
    public class MessageBar : MonoBehaviour
    {
        protected Image messageSlider;
        public GameObject player;
        protected PlayerController TempDate;
        protected int currMessage, maxMessage;
        protected float orImage;
        protected Text currMText, maxMText;
        public virtual void Awake()
        {
            messageSlider = transform.GetChild(1).GetChild(1).GetComponent<Image>();
            TempDate = player.GetComponent<PlayerController>();
            orImage = messageSlider.rectTransform.rect.width;
            currMText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
            maxMText = transform.GetChild(2).GetChild(2).GetComponent<Text>();
            

        }
        public void UpdateImage(float currentValue, float maxValue, Image Obj, float ObjWidth)
        {
            
            float sliderPercent = currentValue / maxValue;
            Obj.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                ObjWidth * sliderPercent);
        }
    }
}
