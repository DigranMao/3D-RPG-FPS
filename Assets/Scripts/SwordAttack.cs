using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Collider swordColliders;
    public int dps = 25; // swordDamage
    public bool isSwinging;
    public bool hasHitEnemy = false;

    void Start()
    {
        swordColliders = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAi enemy = other.GetComponent<EnemyAi>();
        if (enemy != null && isSwinging && !hasHitEnemy)
        {
           // Bounds enemyBounds = enemy.GetComponent<Collider>().bounds;
            //Ray swordRay = new Ray(swordColliders.transform.position, swordColliders.transform.forward);

            //if(enemyBounds.IntersectRay(swordRay))
           // {
                enemy.TakeDamage(dps);
                hasHitEnemy = true;
           // }
            
           //Debug.DrawLine(swordColliders.transform.position, enemyBounds.center, Color.red, 0.1f);
        }
    }

}
