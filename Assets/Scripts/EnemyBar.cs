using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBar : MonoBehaviour
{
    int health = 100;
    Animator anim;
    public Slider healthBar;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            anim.SetTrigger("death");
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            anim.SetTrigger("damage");
        }
    }
}
