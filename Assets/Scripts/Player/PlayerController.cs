using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    private TankData ThisTrans;
    private Movement _movement;
    public GameObject Cannon;
    public GameObject CannonHolder;

    private float timer;

    //Initializes variables
    private void Awake(){
        GameManager.Manager.Player = gameObject;
        ThisTrans = GameManager.Manager.Tankdata;
        _movement = GetComponent<Movement>();
    }

    private void Update(){
        //Moves the tank forward if able to
        if (ThisTrans.TankCanMove){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _movement.RegularMovement(horizontal, vertical, ThisTrans);
        }

        //Checks if butten is pressed and it can in the fire rate.
        if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timer) {
            timer = Time.time + ThisTrans.reloadRate;
            GetComponent<ShootManager>().Shoot(ThisTrans, Cannon, CannonHolder);
        }
    }
}
