using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float TankAcc = 10;
    public float MaxSpeed = 15;
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
        rb.AddForce(transform.forward * TankAcc * ver, ForceMode.Acceleration);
        rb.transform.Rotate(Vector3.up * turnSpeed * hor);

        //Slows the player down if they are going too fast
        if (rb.velocity.magnitude > MaxSpeed) {
            rb.AddForce(-rb.velocity.normalized * MaxSpeed);
        }

        //Slowly comes to a stop
        if (rb.velocity.magnitude > 0.1f && Mathf.Abs(ver) < 0.1f)
        {
            rb.AddForce(rb.velocity.normalized * -StopSpeed);
        }

    }
}
