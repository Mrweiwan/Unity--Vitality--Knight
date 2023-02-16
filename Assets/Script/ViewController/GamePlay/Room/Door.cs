using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class Door : MonoBehaviour
    {
       
        public void OpenDoor()
        {
            gameObject.SetActive(false);
        }
        public void CloseDoor()
        {
            gameObject.SetActive(true);
        }
    }
}
