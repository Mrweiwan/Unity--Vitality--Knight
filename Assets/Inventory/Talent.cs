using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    [CreateAssetMenu(fileName = "New Talent", menuName = "Inventory/New Talent")]
    public class Talent : ScriptableObject
    {
        public int talentId;
        public Sprite talentImage;
        public string talentName;
        [TextArea(1,8)]
        public string talentDes;

    }
}
