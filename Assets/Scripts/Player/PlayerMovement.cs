using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float walkingSpeed = 12f;

    public float dashDuration = 0.25f;
    public float dashSpeed = 32f;

    public AudioSource playeraudio;
    public AudioClip DashSound;
    bool IsMoving = false;

    private float lastDash = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playeraudio = GetComponent<AudioSource>();
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
            playeraudio.PlayOneShot(DashSound);
            lastDash = Time.time;
        }

        // Apply dash speed
        if (Time.time - lastDash < dashDuration)
        {
            speed = dashSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z; // Move in the direction of the camera
        
        // Clamping the magnitude of the move vector to 1, so that the player can't move faster diagonally
        Vector3 clamptedMove = Vector3.ClampMagnitude(move, 1f);

        controller.Move(clamptedMove * speed * Time.deltaTime); // Move the player
        
    }

    private void OnDisable()
    {
        playeraudio.Stop();
    }

    private void FixedUpdate(){
        if (controller.velocity.magnitude > 0){
            IsMoving = true;
            Console.WriteLine("Moving");
        }
        else{
            IsMoving = false;
        }
        if (IsMoving){
            if (!playeraudio.isPlaying)
                playeraudio.Play();
        }
        else {
            playeraudio.Stop();
        }
    }
}
