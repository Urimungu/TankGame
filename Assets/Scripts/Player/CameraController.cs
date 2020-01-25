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

    private TankData TD;

    public Transform CannonHolder;


    private void Start()
    {
        //Finds the objects it needs
        Target = GameManager.GM.Player.transform;
        TD = GameManager.GM.transform.GetComponent<TankData>();
        Holder = transform.Find("CameraHolder").gameObject;
        CannonHolder = Target.GetChild(0).Find("CannonHolder");
        SetUpCamera();
        TF = GetComponent<Transform>();
        MainCam = GameManager.GM.MainCamera;
    }

    private void SetUpCamera()
    {
        //Sets the position for the camera
        Holder.transform.position = Target.transform.forward * -TD.CameraDistance;
        Holder.transform.position = new Vector3(Holder.transform.position.x, TD.CameraHeight, Holder.transform.position.z);

    }

    private void Update()
    {
        //Gets basic input from the Editor
        if (TD.CameraCanMove)
        {
            float MouseX = Input.GetAxisRaw("Mouse X");
            float MouseY = Input.GetAxisRaw("Mouse Y");
            Movement(MouseX, MouseY);
        }

    }

    private void Movement(float hor, float ver)
    {
        //Follows the player and adds a side distance
        TF.position = Target.position + (Target.transform.right * TD.SideDistance);

        //Rotate around the tank in the X direction (Left/Right)
        Target.Rotate(Target.transform.up * hor * TD.RotationSpeed);
        TF.rotation = Target.rotation;

        //Rotates the Camera in the Y direction (Up/Down)
        Vector3 newPos = new Vector3();
        TD.theta = Mathf.Clamp(TD.theta + (TD.VerticalSpeed / 10) * -ver, -2.5f, -0.5f);
        newPos.z = TD.CameraDistance * Mathf.Sin(TD.theta);
        newPos.y = TD.CameraDistance * Mathf.Cos(TD.theta) + TD.CameraHeight;

        //Applies the new transforms and makes sure they work
        Holder.transform.localPosition = newPos;
        MainCam.transform.localPosition = Vector3.zero;
        MainCam.transform.LookAt(new Vector3(TF.position.x, TF.position.y + TD.CameraHeight / 2, TF.position.z) - (TF.right * TD.SideDistance / 3), Vector3.up);

        //Aesthetics 
        CannonHolder.transform.LookAt(MainCam.transform.position + (MainCam.transform.forward * 100), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out hit, 100, TD.CameraLayerMask)) {
            if(hit.collider != null) {
                CannonHolder.transform.LookAt(hit.point, Vector3.up);
            }
        }
    }
}
