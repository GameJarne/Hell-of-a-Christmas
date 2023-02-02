using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class uiRotate : MonoBehaviour
{
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = true;

    public float xRotateSpeed = 1;
    public float yRotateSpeed = 1;
    public float zRotateSpeed = 1;

    void Update()
    {
        float x = (rotateX) ? xRotateSpeed * Time.deltaTime : 0;
        float y = (rotateY) ? yRotateSpeed * Time.deltaTime : 0;
        float z = (rotateZ) ? zRotateSpeed * Time.deltaTime : 0;

        transform.Rotate(x, y, z);
    }
}
