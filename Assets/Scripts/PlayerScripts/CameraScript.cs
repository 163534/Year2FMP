using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RotateCamera();   
    }
    void RotateCamera()
    {
        float rotationPower = 4;
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * rotationPower, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse Y") * rotationPower, -Vector3.right);

        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;
        //Clamp the Up/Down rotation
        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 30)
        {
            angles.x = 30;
        }

        transform.localEulerAngles = angles;
    }
}
