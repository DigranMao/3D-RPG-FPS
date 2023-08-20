using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator anim;
    Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    //Patroling2
    private Vector3 currentWaypoint;
    public float waypointDelay = 3f;
    public float walkPointRange = 3f;
    private float delayTimer = 0f;  

    //Attacking
    public float timeAttacks;
    public bool already;
    public float offset = 20;
    
    float smooth;

    //States
    public float reviewRange, attackRange;
    public bool playerInReviewRange, playerInAttackRange;

    float health = 100;
    public Slider healthBar;

    public Transform pointDamage;
    public int dps = 25; // swordDamage
    public float rangeDamage = 1;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        //swordColliders =  GetComponentInChildren<CapsuleCollider>();

        SearchWalkPoint();
    }

    void Update()
    {
        healthBar.value = health / 100;
       
        playerInReviewRange = Physics.CheckSphere(transform.position, reviewRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInReviewRange && !playerInAttackRange)
            Patroling();
        if(playerInReviewRange && !playerInAttackRange)
            ChasePlayer();
        if(playerInReviewRange && playerInAttackRange)
            AttackPlayer();
        
        healthBar.value = health;       
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            print("DeathEnemy");
        }
    }

    public void DamagePlayer()
    {
        //if (Physics.CheckCapsule(swordColliders.bounds.center, 
        //swordColliders.bounds.center + transform.forward * swordColliders.height, 
        //swordColliders.radius, whatIsPlayer))

        if(Physics.CheckSphere(pointDamage.position, rangeDamage, whatIsPlayer))
        {
            // Если игрок находится внутри сферы, передаем ему урон
            player.gameObject.GetComponent<PlayerController>().TakeDamage(dps);
        }
    }

    void Patroling()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            delayTimer += Time.deltaTime;
            anim.SetFloat("Speed", 0f, 0.7f, Time.deltaTime); //0.5f задержка изменения параметра в анимации

            if (delayTimer >= waypointDelay)
            {
                SearchWalkPoint();     
                delayTimer = 0f;
            }
               
        }
        else
        {
            agent.speed = 2;
            anim.SetFloat("Speed", 0.5f, 0.5f, Time.deltaTime); 
        }
    }

    void SearchWalkPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * walkPointRange;
        Vector3 waypoint = new Vector3(randomPoint.x, 0, randomPoint.y) + transform.position;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(waypoint);

        waypoint.y = terrainHeight + 1f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(waypoint, out hit, 1f, NavMesh.AllAreas))
        {
            currentWaypoint = hit.position;
            agent.SetDestination(currentWaypoint);
        }
    }

    void ChasePlayer()
    {   
        if(!already)
        {
            agent.SetDestination(player.position);
            agent.speed = 4;
            anim.SetFloat("Speed", 1, 0.3f, Time.deltaTime);
        }
        
    }

    void AttackPlayer()
    {
        
        Vector3 dir = player.position - transform.position;
        float rotateAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle + offset, ref smooth, 0.2f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if(!already)
        {
            agent.enabled = false;
            offset = -20;             
            
            anim.SetFloat("Speed", 0f, 0.05f, Time.deltaTime);
            anim.SetBool("Attack", true);
            already = true;
           StartCoroutine(ResetAttack());
        }

    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeAttacks);
        already = false;
        anim.SetBool("Attack", false);

        offset = 0;

        if(health > 0)
        agent.enabled = true;
    }

    /*void ResetAttack()
    {
        already = false;
        anim.SetBool("Attack", false);

        offset = 0;

        if(health > 0)
        agent.enabled = true;
    }*/

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reviewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointDamage.position, rangeDamage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            anim.SetTrigger("death");
            healthBar.gameObject.SetActive(false);
            agent.enabled = false;
            enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            anim.SetTrigger("damage");
        }
    }
}
