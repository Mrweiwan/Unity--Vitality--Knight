using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    [CreateAssetMenu(fileName ="New Item",menuName ="Inventory/New Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite itemImage;
        public Sprite bulletImage;
        public int cosume;
        public int hurt;
    }
}
