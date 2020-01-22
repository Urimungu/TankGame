using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float TankAcc = 10;
    public float MaxSpeed = 15;
    public float ReverseMax = 10;
    public float StopSpeed = 10;
    public float turnSpeed = 10;

    public bool CanMove = true;
    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CanMove){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
    }

    private void Movement(float hor, float ver)
    {
        //Removes the side speed boost
        Vector2 MoveDirection = new Vector2(hor, ver).normalized;

        //Moves the player forwards and right
        rb.AddForce(transform.forward * TankAcc * MoveDirection.y, ForceMode.Acceleration);
        rb.AddForce(transform.right * TankAcc * MoveDirection.x, ForceMode.Acceleration);

        //Makes the speed different if the player is moving backwards
        float SpeedCap = ver > 0.1f ? MaxSpeed : ReverseMax;

        //Slows the player down if they are going too fast
        if (Mathf.Abs(rb.velocity.magnitude) > SpeedCap) {
            rb.AddForce(-rb.velocity.normalized * MaxSpeed);
        }

        //Slowly comes to a stop
        if (Mathf.Abs(rb.velocity.magnitude) > 0.2f && Mathf.Abs(ver) < 0.1f && Mathf.Abs(hor) < 0.1f) {
            rb.AddForce(rb.velocity.normalized * -StopSpeed);
        }

    }
}
