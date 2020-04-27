using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //References
    private Transform _transform;
    private Transform _target;
    private GameObject _holder;
    private GameObject _mainCam;

    private TankData _data;
    public Transform CannonHolder;

    //Sets the values once it is spawned in
    public void SpawnCamera(Transform target, TankData data)
    {
        //Finds the objects it needs
        _target = target;
        _data = data;

        _holder = transform.Find("CameraHolder").gameObject;
        CannonHolder = _target.GetChild(0).Find("CannonHolder");
        _mainCam = _holder.transform.GetChild(0).gameObject;
        SetUpCamera();
        _transform = GetComponent<Transform>();
    }

    //Creates the camera values if they aren't already set up
    private void SetUpCamera(){
        //Sets the position for the camera
        _holder.transform.position = _target.transform.forward * -_data.CameraDistance;
        _holder.transform.position = new Vector3(_holder.transform.position.x, _data.CameraHeight, _holder.transform.position.z);

    }

    private void Update(){
        //Gets basic input from the Editor
        if (_data != null && _data.CameraCanMove){
            float MouseX = Input.GetAxisRaw("MouseX" + _data.TankNum);
            float MouseY = Input.GetAxisRaw("MouseY" + _data.TankNum);
            Movement(MouseX, MouseY);
        }
    }

    private void Movement(float hor, float ver)
    {
        //Follows the player and adds a side distance
        _transform.position = _target.position + (_target.transform.right * _data.SideDistance);

        //Rotate around the tank in the X direction (Left/Right)
        _target.Rotate(_target.transform.up * hor * _data.RotationSpeed);
        _transform.rotation = _target.rotation;

        //Rotates the Camera in the Y direction (Up/Down)
        Vector3 newPos = new Vector3();
        _data.theta = Mathf.Clamp(_data.theta + (_data.VerticalSpeed / 10) * -ver, -2.5f, -0.5f);
        newPos.z = _data.CameraDistance * Mathf.Sin(_data.theta);
        newPos.y = _data.CameraDistance * Mathf.Cos(_data.theta) + _data.CameraHeight;

        //Applies the new transforms and makes sure they work
        _holder.transform.localPosition = newPos;
        _mainCam.transform.localPosition = Vector3.zero;
        _mainCam.transform.LookAt(new Vector3(_transform.position.x, _transform.position.y + _data.CameraHeight / 2, _transform.position.z) - (_transform.right * _data.SideDistance / 3), Vector3.up);

        //Aesthetics 
        CannonHolder.transform.LookAt(_mainCam.transform.position + (_mainCam.transform.forward * 100), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out hit, 100, _data.CameraLayerMask)) {
            if(hit.collider != null) {
                CannonHolder.transform.LookAt(hit.point, Vector3.up);
            }
        }
    }
}
