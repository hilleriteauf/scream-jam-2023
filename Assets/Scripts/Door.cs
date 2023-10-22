using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Lever> levers;

    public float closedY = 2f;
    public float openY = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool allPulled = true;

        foreach (Lever lever in levers)
        {
            if (!lever.isPulled)
            {
                allPulled = false;
                break;
            }
        }

        if (allPulled)
        {
            transform.position = new Vector3(transform.position.x, openY, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, closedY, transform.position.z);
        }
    }
}
