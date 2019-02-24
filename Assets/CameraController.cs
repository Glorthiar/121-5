using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Roation
    [SerializeField] private string MouseXInputName;
    [SerializeField] private string MouseYInputName;
    [SerializeField] private float MouseSensitivity;

    [SerializeField] private Transform PlayerBody;

    private float XAxisClamp;

    void Awake()
    {
        LockCursor();
    }

    void Start()
    {
        XAxisClamp = 0;
    }

    void Update()
    {
        CameraRotation();
    }

    private void ClampXAxisRotationToValue(float Value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = Value;
        transform.eulerAngles = eulerRotation;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation()
    {
        //Rotate Head
        float MouseX = Input.GetAxis(MouseXInputName) * MouseSensitivity;
        float MouseY = Input.GetAxis(MouseYInputName) * MouseSensitivity;

        XAxisClamp += MouseY;

        if (XAxisClamp > 60.0f)
        {
            XAxisClamp = 60.0f;
            MouseY = 0.0f;
            ClampXAxisRotationToValue(300.0f);
        }
        else if (XAxisClamp < -60.0f)
        {
            XAxisClamp = -60.0f;
            MouseY = 0.0f;
            ClampXAxisRotationToValue(60.0f);
        }

        transform.Rotate(Vector3.left * MouseY);

        //Rotate Body
        PlayerBody.Rotate(Vector3.up * MouseX);

    }

}
