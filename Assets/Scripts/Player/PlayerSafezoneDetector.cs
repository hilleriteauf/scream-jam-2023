using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSafezoneDetector : MonoBehaviour
{
    public bool IsInSafezone { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsInSafezone = getInSafeZone();
    }

    private bool getInSafeZone()
    {
        Safezone[] safezones = FindObjectsOfType<Safezone>();
        foreach (Safezone safezone in safezones)
        {
            if (safezone.Collider.bounds.Contains(transform.position))
            {
                return true;
            }
        }

        return false;
    }
}
