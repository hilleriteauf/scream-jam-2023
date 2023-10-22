using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyVision EnemyVision;

    public EnemyPatrollingController EnemyPatrol;

    public EnemyChasingController EnemyChasingController;

    private bool isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        EnemyPatrol.StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled && !EnemyChasingController.Enabled && EnemyVision.PlayerInSight)
        {
            Debug.Log("Player in sight, starting chase");
            EnemyPatrol.StopPatrolling();
            EnemyChasingController.StartChasing();
        }
    }

    // Called by EnemyChasingController when the enemy stops chasing the player
    public void ChaseEnded()
    {
        // The enemy restarts its patrol
        if (isEnabled)
        {
            EnemyPatrol.StartPatrolling();
        }
    }

    public void DisableEnemy()
    {
        Debug.Log("Enemy disabled");
        isEnabled = false;
        EnemyPatrol.StopPatrolling();
        EnemyChasingController.StopChasing();
    }
}
