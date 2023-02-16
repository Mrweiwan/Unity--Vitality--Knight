using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public interface IDamageable
    {
        void GetHit(int damage);
    }
}
