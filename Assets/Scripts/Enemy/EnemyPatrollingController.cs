using Assets.Scripts.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrollingController : MonoBehaviour
{

    public NavMeshAgent agent;

    public Path path;

    public float Speed = 3.5f;
    public float AngularSpeed = 120f;
    public float Acceleration = 8f;

    public bool IsPatrolling { get; private set; } = false;

    private PathNode targetNode = null;
    private float nodeReachedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPatrolling == false || targetNode == null)
        {
            return;
        }

        if ((Vector3.Distance(transform.position, new Vector3(targetNode.Position.x, transform.position.y, targetNode.Position.z)) < 0.1f))
        {
            // We reached the target node

            // If the node requires the enemy to look in a direction, rotate the enemy
            if (targetNode.lookInDirection && Quaternion.Angle(targetNode.Rotation, transform.rotation) > 0.1f)
            {
                // Rotate the enemy to look in the direction of the node, using the angular speed of the nav mesh agent
                Quaternion rotation = targetNode.Rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, agent.angularSpeed * Time.deltaTime);
                return;
            }

            // If the node requires the enemy to stop, wait for the stop time
            if (nodeReachedTime == 0f)
            {
                nodeReachedTime = Time.time;
            }

            if (Time.time - nodeReachedTime > targetNode.stopTime)
            {
                // We waited enough, go to the next node
                nodeReachedTime = 0f;
                targetNode = path.GetNextNode(targetNode);
                if (targetNode == null)
                {
                    return;
                }
                agent.SetDestination(targetNode.Position);
            }
        }
    }

    public void StartPatrolling()
    {
        // Set the nav mesh agent parameters
        agent.acceleration = Acceleration;
        agent.speed = Speed;
        agent.angularSpeed = AngularSpeed;

        // Start patrolling
        IsPatrolling = true;

        if (targetNode == null)
        {
            targetNode = path.GetNextNode(targetNode);
        }
        agent.SetDestination(targetNode.Position);
    }

    public void StopPatrolling()
    {
        IsPatrolling = false;
        agent.ResetPath();
    }
}
