using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    LayerMask whatIsPlayer;
    Transform attackPoint;
    public float attackRange = 0.5f;
    public int dps = 25; // swordDamage
    public bool isSwinging;
    public PlayerController player;
    Collider hitPlayer;
    
    //Collider swordColliders;
    //public EnemyAi enemyAi;
   
    void Start()
    {
        //swordColliders = GetComponent<Collider>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        //hitPlayer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        //player = other.GetComponent<PlayerController>();
        /*if (player != null && !isSwinging)
        {
            player.TakeDamage(dps);
            isSwinging = true;
        }*/
    }

    void DamagePlayer()
    {
        if (Physics.CheckCapsule(GetComponent<CapsuleCollider>().bounds.center, 
        GetComponent<CapsuleCollider>().bounds.center + Vector3.up * GetComponent<CapsuleCollider>().height, 
        GetComponent<CapsuleCollider>().radius, whatIsPlayer))
        {
            // Если игрок находится внутри сферы, передаем ему урон
            player.TakeDamage(dps);
        }
    }
}
