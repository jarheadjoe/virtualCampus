using UnityEngine;
using System.Collections;
// control main camera with key and mouse
public class KeyMove : MonoBehaviour
{
    //key control, can change moving speed
    public float MaxSpeed     = 50; 
    public float Acceleration = 10;
    public float Deceleration = 500;
    //mouse control, can mouse settings
    public float sensitivityX = 1F;        
    public float sensitivityY = 1F;
    public float MovingSpeed = 20f;
    //private, dont change these
    float delta_x, delta_y, delta_z;
    float horizonSpeed, vertSpeed = 0;
    private float minimumY = -120F;          
    private float maximumY = 120F;
    float rotationY = 0F; 
    float distance = 5;
    float ZoomSpeed = 20f;                 
    Quaternion rotation;
    void Start()
    {

    }
    void Update()
    {
        //key moving
        
        if ((Input.GetKey(KeyCode.A)) && (horizonSpeed < MaxSpeed))
        {
            horizonSpeed = horizonSpeed - Acceleration * Time.deltaTime;
        }
                
        else if ((Input.GetKey(KeyCode.D)) && (horizonSpeed > -MaxSpeed))
            horizonSpeed = horizonSpeed + Acceleration * Time.deltaTime;
        
        else
        {
            if (horizonSpeed > Deceleration * Time.deltaTime)
                horizonSpeed = horizonSpeed - Deceleration * Time.deltaTime;
            else if (horizonSpeed < -Deceleration *  Time.deltaTime)
                horizonSpeed = horizonSpeed + Deceleration * Time.deltaTime;
            else
                horizonSpeed = 0;
        }
        delta_x = horizonSpeed * Time.deltaTime;

        if ((Input.GetKey(KeyCode.W)) && (vertSpeed < MaxSpeed))
        {
            vertSpeed = vertSpeed + Acceleration * Time.deltaTime;
        }
        else if ((Input.GetKey(KeyCode.S)) && (vertSpeed > -MaxSpeed))
            vertSpeed = vertSpeed - Acceleration * Time.deltaTime;
        else
        {
            if (vertSpeed > Deceleration * Time.deltaTime)
                vertSpeed = vertSpeed - Deceleration * Time.deltaTime;
            else if (vertSpeed < -Deceleration * Time.deltaTime)
                vertSpeed = vertSpeed + Deceleration * Time.deltaTime;
            else
                vertSpeed = 0;
        }
        delta_z = vertSpeed * Time.deltaTime;

        transform.Translate(delta_x, 0, delta_z);
        // mouse right button
        if (Input.GetMouseButton(1))
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);


        }
        // mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            delta_z = -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            transform.Translate(0, 0, -delta_z);
            distance += delta_z;
        }
        
        if (Input.GetMouseButton(2))
        {
            delta_x = Input.GetAxis("Mouse X") * MovingSpeed;
            delta_y = Input.GetAxis("Mouse Y") * MovingSpeed;
            rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.position = rotation * new Vector3(-delta_x, -delta_y, 0) + transform.position;
        }

    }
    // camera collision
    private void OnCollisionEnter(Collision collision)
    {
        vertSpeed = 0;
        horizonSpeed = 0;
    }
}