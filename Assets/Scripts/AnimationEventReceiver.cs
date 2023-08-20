using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public EnemyAi enemyAi;

    private void Awake()
    {
        
    }

    public void DamagePlayerEvent()
    {
        enemyAi.DamagePlayer();
    }
}
