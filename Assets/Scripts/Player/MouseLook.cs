using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 3f;

    public Transform playerBody;

    public GameObject torch;
    public GameObject torchPrefab;
    public AudioSource playeraudio;
    public AudioClip[] playeraudioClips;
    public float rotationSpeedHighTreshold = 25f;
    private float rotationSpeedDownTreshold;
    private float xRotation = 0f;
    private bool istriggered = false;
    public float throwForce = 600f;
    private bool prepareThrow = false;
    private float throwCharge = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        rotationSpeedDownTreshold = rotationSpeedHighTreshold - 5f;
        torch.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);

        Vector3 prevRotation = transform.localRotation.eulerAngles;

        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
        playerBody.Rotate(Vector3.up * mouseX);



        if (prepareThrow == true)
        {
            throwCharge += Time.deltaTime;
        }
        if (Input.GetKey("f"))
        {
            prepareThrow = true;
        }
        if (Input.GetKeyUp("f"))
        {
            if (prepareThrow == true)
            {
                Debug.Log("Throw with force " + throwCharge * throwForce);
                torch.GetComponent<Rigidbody>().AddForce(transform.forward * throwCharge * throwForce);
                // torch.transform.localRotation = Quaternion.FromToRotation(torch.transform.position, cam.transform.forward);
                torch.GetComponent<Rigidbody>().useGravity = true;
                prepareThrow = false;
                throwCharge = 0f;
                torch.GetComponent<Interactable>().IsInteractable = true;
                this.playeraudio = null;
                this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)-1).ToString();
                torch.transform.parent = null;
                torch = null;
                // if (int.Parse(this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) != 0)
                // {
                //     torch = Instantiate(torchPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                //     torch.transform.parent = transform;
                //     torch.GetComponent<Rigidbody>().useGravity = false;
                //     torch.GetComponent<Interactable>().IsInteractable = false;
                //     torch.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //     torch.GetComponent<TorchInteract>().PlayerCamera = this.GetComponent<Camera>();
                //     torch.GetComponent<TorchInteract>().Player = this.transform.parent.gameObject;
                //     torch.GetComponent<TorchInteract>().torchPickUpPoint = this.transform.parent.gameObject.transform.Find("TorchPickUpPoint");
                //     torch.transform.position = torch.GetComponent<TorchInteract>().torchPickUpPoint.transform.position;
                //     torch.transform.rotation = torch.GetComponent<TorchInteract>().torchPickUpPoint.transform.rotation;
                // }
            }
        }

        // Playing a sound when the rotation speed is high enough
        if (playeraudio != null)
        {
            if (Mathf.Abs(mouseX) > rotationSpeedHighTreshold || Mathf.Abs(mouseY) > rotationSpeedHighTreshold)
            {
                Debug.Log(Mathf.Abs(mouseX));
                Debug.Log(Mathf.Abs(mouseY));
                if (!istriggered) {
                    istriggered = true;
                    playeraudio.PlayOneShot(playeraudioClips[Random.Range(0, playeraudioClips.Length)]);
                }
            } else if (Mathf.Abs(mouseX) < rotationSpeedDownTreshold && Mathf.Abs(mouseY) < rotationSpeedDownTreshold) {
                istriggered = false;
            }
        }
    }
}
