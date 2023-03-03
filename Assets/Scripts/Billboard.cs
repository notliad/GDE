using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
    Camera cam;

    void Update()
    {
        if(cam == null)
        {
            cam = FindFirstObjectByType<Camera>();
        }
        if(cam == null)
        {
            return;
        }

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }

}
