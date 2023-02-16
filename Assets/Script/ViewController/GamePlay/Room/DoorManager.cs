using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class DoorManager : MonoBehaviour
    {
        public List<Door> doors = new List<Door>();
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.active == true)
                {
                    doors.Add(transform.GetChild(i).GetComponent<Door>());
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                
            }
        }
        private void Update()
        {
            
        }
    }
}
