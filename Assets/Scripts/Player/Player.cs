using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;

    private float vSpeed = 0f;

    private void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        
        var speedVector = transform.forward * Input.GetAxis("Vertical") * speed;

        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;

        characterController.Move(speedVector * Time.deltaTime);


    }
}
