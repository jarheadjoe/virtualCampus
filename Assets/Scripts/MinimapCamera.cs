using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform target;

    float defaultPosY;
    
    void Start()
    {
        defaultPosY = transform.position.y;
    }
    
    void Update()
    {
        // Apply position
        transform.position = new Vector3(target.position.x, defaultPosY, target.position.z);
        // Apply rotation
        transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);
    }
}