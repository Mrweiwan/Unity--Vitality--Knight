using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();
    public Transform creatPos;
    bool isCreat;
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& isCreat == false) 
        {
            anim.SetTrigger("Open");
            isCreat = true;
            int randomNum = Random.Range(0, weapons.Count);
            Instantiate(weapons[randomNum],creatPos.position,Quaternion.identity,transform);
        }
    }
}
