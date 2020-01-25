using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    private Rigidbody rb;
    private TankData TS;

    //Initializes variables
    private void Awake(){
        GameManager.GM.Player = gameObject;
        rb = GetComponent<Rigidbody>();
        TS = GameManager.GM.transform.GetComponent<TankData>();
    }

    private void Update()
    {
        //Moves the tank forward if able to
        if (TS.TankCanMove){
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
        rb.AddForce(transform.forward * TS.TankAcc * MoveDirection.y, ForceMode.Acceleration);
        rb.AddForce(transform.right * TS.TankAcc * MoveDirection.x, ForceMode.Acceleration);

        //Makes the speed different if the player is moving backwards
        float SpeedCap = ver > 0.1f ? TS.MaxSpeed : TS.ReverseMax;

        //Slows the player down if they are going too fast
        if (Mathf.Abs(rb.velocity.magnitude) > SpeedCap) {
            rb.AddForce(-rb.velocity.normalized * TS.MaxSpeed);
        }

        //Slowly comes to a stop
        if (Mathf.Abs(rb.velocity.magnitude) > 0.2f && Mathf.Abs(ver) < 0.1f && Mathf.Abs(hor) < 0.1f) {
            rb.AddForce(rb.velocity.normalized * -TS.StopSpeed);
        }

    }
}
