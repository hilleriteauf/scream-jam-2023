using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Tooltip("The angle of the enemy's field of view")]
    public float FieldOfViewAngle = 110f;

    public bool PlayerInSight { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        bool newPlayerInSight = getPlayerInSight();
        if (newPlayerInSight != PlayerInSight)
        {
            PlayerInSight = newPlayerInSight;
            Debug.Log("PlayerInSight : " + PlayerInSight);
        }
        
    }

    private bool getPlayerInSight()
    {
        // Get PlayerLightDetector
        PlayerLightDetector playerLightDetector = FindObjectOfType<PlayerLightDetector>();

        if (playerLightDetector == null)
        {
            return false;
        }

        // Calculates the direction
        Vector3 direction = playerLightDetector.transform.position - transform.position;

        // Calculates the angle between the enemy's forward vector and the direction
        float angle = Vector3.Angle(direction, transform.forward);

        // If the angle is greater than half the field of view, the player is not in the enemy's sight
        if (angle > FieldOfViewAngle * 0.5f)
        {
            return false;
        }

        // Cast a ray from the enemy to the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
        {
            // If the ray hit the player and the player is in the light, the player is in the enemy's sight
            if (hit.collider.gameObject == playerLightDetector.gameObject && playerLightDetector.IsInLight)
            {
                return true;
            }
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        // Draw the field of view bounds
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.rotation * Quaternion.Euler(0, -FieldOfViewAngle * 0.5f, 0) * Vector3.forward * 10);
        Gizmos.DrawRay(transform.position, transform.rotation * Quaternion.Euler(0, FieldOfViewAngle * 0.5f, 0) * Vector3.forward * 10);
    }
}
