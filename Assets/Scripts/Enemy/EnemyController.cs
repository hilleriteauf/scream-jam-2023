using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyVision EnemyVision;

    public EnemyPatrollingController EnemyPatrol;

    public EnemyChasingController EnemyChasingController;

    // Start is called before the first frame update
    void Start()
    {
        EnemyPatrol.StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnemyChasingController.Enabled && EnemyVision.GetPlayerInSight())
        {
            EnemyPatrol.StopPatrolling();
            EnemyChasingController.StartChasing();
        }
    }

    // Called by EnemyChasingController when the enemy stops chasing the player
    public void ChaseEnded()
    {
        // The enemy restarts its patrol
        EnemyPatrol.StartPatrolling();
    }
}
