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

        if (torch != null)
        {
            TorchConfiguration();
        }
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

        if (torch != null && Input.GetKey("f"))
        {
            prepareThrow = true;
        } else
        {
            prepareThrow = false;
        }

        if (prepareThrow == true)
        {
            throwCharge += Time.deltaTime;
        }

        if (Input.GetKeyUp("f"))
        {
            if (torch != prepareThrow == true)
            {
                Debug.Log("Throw with force " + throwCharge * throwForce);
                torch.GetComponent<Rigidbody>().AddForce(transform.forward * throwCharge * throwForce);
                // torch.transform.localRotation = Quaternion.FromToRotation(torch.transform.position, cam.transform.forward);
                torch.GetComponent<Rigidbody>().useGravity = true;
                torch.GetComponent<Interactable>().IsInteractable = true;
                this.playeraudio = null;
                this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text = (int.Parse(this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text)-1).ToString();
                torch.transform.parent = null;
                torch = null;
                if (int.Parse(this.GetComponentInParent<PlayerInteraction>().torchCounterText.GetComponent<TMP_Text>().text) != 0)
                {
                    torch = Instantiate(torchPrefab, GameObject.Find("TorchPickUpPoint").transform.position, Quaternion.identity);
                    this.playeraudio = torch.GetComponent<AudioSource>();
                    TorchConfiguration();
                }
            }

            prepareThrow = false;
            throwCharge = 0f;
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

    private void TorchConfiguration()
    {
        torch.GetComponent<Rigidbody>().useGravity = false;
        torch.GetComponent<Interactable>().IsInteractable = false;
        torch.GetComponent<Rigidbody>().velocity = Vector3.zero;
        torch.GetComponent<TorchInteract>().Player = this.GetComponent<Camera>().gameObject;
        torch.GetComponent<TorchInteract>().torchPickUpPoint = GameObject.Find("TorchPickUpPoint").transform;
        torch.transform.parent = transform;
        torch.transform.localPosition = torch.GetComponent<TorchInteract>().torchPickUpPoint.transform.localPosition;
        torch.transform.localRotation = torch.GetComponent<TorchInteract>().torchPickUpPoint.transform.localRotation;
    }
}
