using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public float delta;
    public float transSpeed;

    public float maxZoomOut;
    public float maxZoomIn;

    private Vector3 forward;

    private bool looking = false;

    void Start()
    {
        forward = cam.transform.forward;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation *= Quaternion.AngleAxis(1.0f * delta * Time.deltaTime, transform.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation *= Quaternion.AngleAxis(-1.0f * delta * Time.deltaTime, transform.up);
        }
        if (Input.GetKey(KeyCode.W))
        {
            cam.fieldOfView = Mathf.Max(maxZoomIn, cam.fieldOfView - transSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.fieldOfView = Mathf.Min(maxZoomOut, cam.fieldOfView + transSpeed * Time.deltaTime);
        }

        if (looking)
        {
            float newRotationX = cam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 1.0f;
            float newRotationY = cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 1.0f;
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

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
