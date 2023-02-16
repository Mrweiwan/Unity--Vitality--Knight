using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]
    public class Inventory : ScriptableObject
    {
        public List<Item> itemList = new List<Item>();
    }
}
