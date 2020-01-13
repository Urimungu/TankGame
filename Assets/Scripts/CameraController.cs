using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //References
    private Transform Target;
    private GameObject Holder;
    public Transform CannonHolder;

    //Variables/Stats
    [Header("Stats")]
    public float RotationSpeed = 10;
    public float CameraDistance = 3;
    public float CameraHeight = 4;
    public float SideDistance = 1;
    public bool CanMove = true;


    private void Start()
    {
        //Finds the objects it needs
        Target = GameManager.GM.Player.transform;
        Holder = transform.Find("CameraHolder").gameObject;
        CannonHolder = transform.GetChild(0).Find("CannonHolder");
        SetUpCamera();
    }

    private void SetUpCamera()
    {
        //Sets the position for the camera
        Holder.transform.position = Target.transform.forward * -CameraDistance;
        Holder.transform.position = new Vector3(Holder.transform.position.x, CameraHeight, Holder.transform.position.z);

    }

    private void Update()
    {
        if (CanMove)
        {
            float MouseX = Input.GetAxisRaw("Mouse X");
            float MouseY = Input.GetAxisRaw("Mouse Y");
            Movement(MouseX, MouseY);
        }

    }

    private void Movement(float hor, float ver)
    {
        //Follows the player and adds a side distance
        transform.position = Target.position + (Target.transform.right * SideDistance);

        //Rotate around the tank in the X direction (Left/Right)
        Target.Rotate(Target.transform.up * hor * RotationSpeed);
        transform.rotation = Target.rotation;

        //Rotates the Camera in the Y direction (Up/Down)
    }
}
