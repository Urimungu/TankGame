using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    //References
    [SerializeField] private GameObject _cannon;
    [SerializeField] private GameObject _cannonHolder;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _explosion;

    //Variables
    private TankData _data;
    private Movement _movement;
    private float _timer;
    private bool _started;

    //Initializes variables
    private void Start(){
        if(!_started) StartTank();
    }

    //Initializes the tank
    private void StartTank() {
        _started = true;
        _data = GetComponent<TankData>();
        _movement = GetComponent<Movement>();
        _data.TankName = GameManager.Manager.PlayerOneName;

        //Spawns a Camera
        GameObject cam = Instantiate(_camera, transform.position, Quaternion.identity);
        cam.GetComponent<CameraController>().SpawnCamera(transform, _data);
        _camera = cam;

        _data.ShotHolder = GameManager.Manager.ShotLocation.gameObject;
    }

    private void Update(){
        //Moves the tank forward if able to
        if (_data.TankCanMove){
            float horizontal = Input.GetAxisRaw("Horizontal" + _data.TankNum);
            float vertical = Input.GetAxisRaw("Vertical" + _data.TankNum);
            _movement.RegularMovement(horizontal, vertical, _data, _camera.transform);
        }

        //Checks if butten is pressed and it can in the fire rate.
        if(Input.GetAxisRaw("Fire" + _data.TankNum) > 0.1f && Time.time > _timer) {
            _timer = Time.time + _data.reloadRate;
            GetComponent<ShootManager>().Shoot(_data, _cannon, _cannonHolder, gameObject);
        }
    }

    //Takes damage of getting shot at
    public void TakeDamage(float damage) {
        _data.CurrentHealth -= damage;
        if(_data.CurrentHealth <= 0) {
            _data.CurrentHealth = 0;
            Instantiate(_explosion, transform.position, Quaternion.identity);
            GameManager.Manager.TankDeath(gameObject);
        }
    }

    //Two Player mode
    public void TwoPlayer() {
        StartTank();
        //Fixes the camera display to adjust for 2 players
        Camera temp = _camera.transform.Find("CameraHolder").GetChild(0).GetComponent<Camera>();
        temp.rect = new Rect(0, 0.5f, 1, 0.5f);

        //Moves the Camera Display down if it's the second player
        if(GameManager.Manager.Player2 == gameObject) {
            temp.rect = new Rect(0, 0, 1, 0.5f);
            _data.TankName = GameManager.Manager.PlayerTwoName;
            _data.TankNum = "_P2";
        }
    }
}
