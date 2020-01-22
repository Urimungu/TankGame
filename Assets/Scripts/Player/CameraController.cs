using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //References
    private Transform TF;
    private Transform Target;
    private GameObject Holder;
    private GameObject MainCam;

    public Transform CannonHolder;

    public LayerMask layermask;

    //Variables/Stats
    [Header("Stats")]
    public float RotationSpeed = 10;
    public float VerticalSpeed = 10;
    public float CameraDistance = 3;
    public float CameraHeight = 4;
    public float HeightLength = 2;
    public float SideDistance = 1;
    public float theta = 0;
    public bool CanMove = true;


    private void Start()
    {
        //Finds the objects it needs
        Target = GameManager.GM.Player.transform;
        Holder = transform.Find("CameraHolder").gameObject;
        CannonHolder = Target.GetChild(0).Find("CannonHolder");
        SetUpCamera();
        TF = GetComponent<Transform>();
        MainCam = GameManager.GM.MainCamera;
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
        TF.position = Target.position + (Target.transform.right * SideDistance);

        //Rotate around the tank in the X direction (Left/Right)
        Target.Rotate(Target.transform.up * hor * RotationSpeed);
        TF.rotation = Target.rotation;

        //Rotates the Camera in the Y direction (Up/Down)
        Vector3 newPos = new Vector3();
        theta = Mathf.Clamp(theta + (VerticalSpeed / 10) * -ver, -2.5f, -0.5f);
        newPos.z = CameraDistance * Mathf.Sin(theta);
        newPos.y = CameraDistance * Mathf.Cos(theta) + CameraHeight;

        //Applies the new transforms and makes sure they work
        Holder.transform.localPosition = newPos;
        MainCam.transform.localPosition = Vector3.zero;
        MainCam.transform.LookAt(new Vector3(TF.position.x, TF.position.y + CameraHeight / 2, TF.position.z) - (TF.right * SideDistance / 3), Vector3.up);

        //Aesthetics 
        CannonHolder.transform.LookAt(MainCam.transform.position + (MainCam.transform.forward * 100), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out hit, 100, layermask)) {
            if(hit.collider != null) {
                CannonHolder.transform.LookAt(hit.point, Vector3.up);
            }
        }
    }
}
