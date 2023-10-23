using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchWallCollision : MonoBehaviour
{

    public float torchRetractSpeed = 1f;

    private TorchInteract torchInteract;

    // Start is called before the first frame update
    void Start()
    {
        torchInteract = GetComponent<TorchInteract>();
    }

    private bool collided = false;

    private void OnTriggerStay(Collider other)
    {
        if (torchInteract == null || torchInteract.Player == null)
        {
            return;
        }

        if (other.gameObject.CompareTag("Untagged"))
        {
            collided = true;
            float currentZ = transform.localPosition.z;
            currentZ -= torchRetractSpeed * Time.fixedDeltaTime;
            if (currentZ < 0)
            {
                currentZ = 0;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentZ);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Untagged"))
        {
            collided = false;
        }
    }

    private void FixedUpdate()
    {
        if (!collided && torchInteract != null && torchInteract.Player != null)
        {
            Vector3 TorchPickupPointPosition = torchInteract.torchPickUpPoint.localPosition;
            float currentZ = transform.localPosition.z;
            if (currentZ < TorchPickupPointPosition.z)
            {
                currentZ += torchRetractSpeed * Time.fixedDeltaTime;
                if (currentZ > TorchPickupPointPosition.z)
                {
                    currentZ = TorchPickupPointPosition.z;
                }
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentZ);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
