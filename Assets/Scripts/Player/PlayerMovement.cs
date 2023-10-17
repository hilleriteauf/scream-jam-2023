using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float walkingSpeed = 12f;

    public float dashDuration = 0.25f;
    public float dashSpeed = 32f;


    private float lastDash = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // Q and D keys
        float z = Input.GetAxis("Vertical"); // Z and S keys

        float speed = walkingSpeed;

        // Dash trigger
        if (Input.GetKeyDown(KeyCode.LeftShift) && (Time.time - lastDash > dashDuration))
        {
            lastDash = Time.time;
        }

        // Apply dash speed
        if (Time.time - lastDash < dashDuration)
        {
            speed = dashSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z; // Move in the direction of the camera

        controller.Move(move * speed * Time.deltaTime); // Move the player
        
    }
}
