using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightDetector : MonoBehaviour
{

    public bool IsInLight { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool newIsInLight = getInLight();
        if (newIsInLight != IsInLight)
        {
            IsInLight = newIsInLight;
            //Debug.Log("IsInLight : " + IsInLight);
        }
    }

    private bool getInLight()
    {
        // Loop over all the lights in the scene
        foreach (Light light in FindObjectsOfType<Light>())
        {
            if (light.enabled)
            {
                // If the light is a child of the player, the player is in the light
                if (light.transform.IsChildOf(transform))
                {
                    return true;
                }

                // Throw a ray from the player to the light to check if the player is in the light
                RaycastHit hit;
                if (Physics.Raycast(light.transform.position, transform.position - light.transform.position, out hit, light.range))
                {
                    // If the ray hit the light, the player is in the light
                    if (hit.collider.gameObject == gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
