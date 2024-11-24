using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles Camera controls + inputs
public class CameraController : MonoBehaviour
{
    public GameManager gameManager; // need a better solution to this
    public Camera cam;
    public float delta;
    public float transSpeed;
    public float rotSpeed;

    public float maxZoomOut;
    public float maxZoomIn;

    public float maxUp;
    public float maxLow;

    private Vector3 ogPos;
    private Vector3 forward;
    private bool looking = false;

    // used for capturing I suppose
    private CameraTargetDetect cameraTargetDetect;

    void Start()
    {
        ogPos = transform.position;
        forward = cam.transform.forward;
        cameraTargetDetect = GetComponent<CameraTargetDetect>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cameraTargetDetect.GetIsJasonFullyFound())
            {
                Debug.Log("Bingo you win!");
                // Call in a function from GameManager that shuts down the game
                cameraTargetDetect.SetClick(true);
                gameManager.Click();
            }
        }

        if (Input.GetKey(KeyCode.A)) transform.rotation *= Quaternion.AngleAxis(1.0f * delta * Time.deltaTime, transform.up);
        if (Input.GetKey(KeyCode.D)) transform.rotation *= Quaternion.AngleAxis(-1.0f * delta * Time.deltaTime, transform.up);
        if (Input.GetKey(KeyCode.W)) cam.fieldOfView = Mathf.Max(maxZoomIn, cam.fieldOfView - transSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) cam.fieldOfView = Mathf.Min(maxZoomOut, cam.fieldOfView + transSpeed * Time.deltaTime);

        float y = transform.position.y;

        if (Input.GetKey(KeyCode.Q)) y -= transSpeed * Time.deltaTime * (2.0f / (maxUp + maxLow + 0.001f));
        if (Input.GetKey(KeyCode.E)) y += transSpeed * Time.deltaTime * (2.0f / (maxUp + maxLow + 0.001f));

        y = Mathf.Clamp(y, ogPos.y - maxLow, ogPos.y + maxUp);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        if (looking)
        {
            float newRotationX = cam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 1.0f;
            float newRotationY = cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 1.0f;
            cam.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float arrowX = 0.0f;
        float arrowY = 0.0f;

        if (Input.GetKey(KeyCode.LeftArrow)) arrowX = -1.0f;
        if (Input.GetKey(KeyCode.RightArrow)) arrowX = 1.0f;
        if (Input.GetKey(KeyCode.UpArrow)) arrowY = 1.0f;
        if (Input.GetKey(KeyCode.DownArrow)) arrowY = -1.0f;

        if (arrowX != 0.0f || arrowY != 0.0f)
        {
            float newRotationX = cam.transform.localEulerAngles.y + arrowX * rotSpeed * Time.deltaTime;
            float newRotationY = cam.transform.localEulerAngles.x - arrowY * rotSpeed * Time.deltaTime;
            cam.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
    }

    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
