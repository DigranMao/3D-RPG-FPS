using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform playerModel;
    public float minAgnle;
    public float maxAgnle;
    public float mouseSens;
    public bool stickCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
        

    void Update()
    {
        float aimX = Input.GetAxis("Mouse X");
        float aimY = Input.GetAxis("Mouse Y");
        cameraTarget.rotation *= Quaternion.AngleAxis(aimX * mouseSens, Vector3.up);
        cameraTarget.rotation *= Quaternion.AngleAxis(-aimY * mouseSens, Vector3.right);

        var angleX = cameraTarget.localEulerAngles.x;
        if(angleX > 180 && angleX < maxAgnle)
        {
            angleX = maxAgnle;
        }
        else if(angleX < 180 && angleX > minAgnle)
        {
            angleX = minAgnle;
        }

        cameraTarget.localEulerAngles = new Vector3(angleX, cameraTarget.localEulerAngles.y, 0);

        if(stickCamera)
        {
            playerModel.rotation = Quaternion.Euler(0, cameraTarget.rotation.eulerAngles.y, 0);
        }

    }
}
