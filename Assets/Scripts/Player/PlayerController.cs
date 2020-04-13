using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    private TankData _data;
    private Movement _movement;
    [SerializeField] private GameObject Cannon;
    [SerializeField] private GameObject CannonHolder;
    [SerializeField] private GameObject Camera;

    private float timer;

    //Initializes variables
    private void Start(){
        _data = GetComponent<TankData>();
        _movement = GetComponent<Movement>();
        _data.ShotHolder = GameManager.Manager.ShotLocation.gameObject;

        //Spawns a Camera
        GameObject cam = Instantiate(Camera, transform.position, Quaternion.identity);
        cam.GetComponent<CameraController>().SpawnCamera(transform, _data);
        Camera = cam;
    }

    private void Update(){
        //Moves the tank forward if able to
        if (_data.TankCanMove){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _movement.RegularMovement(horizontal, vertical, _data, Camera.transform);
        }

        //Checks if butten is pressed and it can in the fire rate.
        if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timer) {
            timer = Time.time + _data.reloadRate;
            GetComponent<ShootManager>().Shoot(_data, Cannon, CannonHolder);
        }
    }

    //Takes damage of getting shot at
    public void TakeDamage(float damage) {
        _data.CurrentHealth -= damage;
        if(_data.CurrentHealth <= 0) {
            _data.CurrentHealth = 0;
            GameManager.Manager.TankDeath(gameObject);
        }
    }
}
