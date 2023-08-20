using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    Collider swordCollider;
    public int dps = 25;
    public bool attack;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if(hitInfo.gameObject.tag == "Enemy" && attack == true)
        {
            hitInfo.GetComponent<EnemyAi>().TakeDamage(dps);
        }
    }

    void Update()
    {
        //print(attack);
    }
}
