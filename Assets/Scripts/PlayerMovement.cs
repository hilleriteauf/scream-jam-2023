using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // A and D keys
        float z = Input.GetAxis("Vertical"); // W and S keys

        Vector3 move = transform.right * x + transform.forward * z; // Move in the direction of the camera

        controller.Move(move * speed * Time.deltaTime); // Move the player
        
    }
}
