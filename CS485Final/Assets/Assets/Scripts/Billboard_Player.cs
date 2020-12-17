using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard_Player : MonoBehaviour
{
    public Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.LookAt(transform.position + cam.forward);
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }
}
