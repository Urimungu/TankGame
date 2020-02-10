using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    private Rigidbody rb;
    private TankData ThisTrans;

    //Initializes variables
    private void Awake(){
        GameManager.Manager.Player = gameObject;
        rb = GetComponent<Rigidbody>();
        ThisTrans = GameManager.Manager.transform.GetComponent<TankData>();
    }

    private void Update()
    {
        //Moves the tank forward if able to
        if (ThisTrans.TankCanMove){
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
        rb.AddForce(transform.forward * ThisTrans.TankAcc * MoveDirection.y, ForceMode.Acceleration);
        rb.AddForce(transform.right * ThisTrans.TankAcc * MoveDirection.x, ForceMode.Acceleration);

        //Makes the speed different if the player is moving backwards
        float SpeedCap = ver > 0.1f ? ThisTrans.MaxSpeed : ThisTrans.ReverseMax;

        //Slows the player down if they are going too fast
        if (Mathf.Abs(rb.velocity.magnitude) > SpeedCap) {
            rb.AddForce(-rb.velocity.normalized * ThisTrans.MaxSpeed);
        }

        //Slowly comes to a stop
        if (Mathf.Abs(rb.velocity.magnitude) > 0.2f && Mathf.Abs(ver) < 0.1f && Mathf.Abs(hor) < 0.1f) {
            rb.AddForce(rb.velocity.normalized * -ThisTrans.StopSpeed);
        }

    }
}
