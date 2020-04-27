using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    //Tanks
    [Header("Tank Stats")]
    public float TankAcc = 10;
    public float MaxSpeed = 15;
    public float ReverseMax = 10;
    public float StopSpeed = 10;
    public bool TankCanMove = true;
    public string TankNum = "_P1";
    public string TankName = "Player";

    //Health
    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public int Lives;

    //Points
    public int Points = 0;

    //Bullet
    [Header("Shooting")]
    public GameObject shot;
    public GameObject ShotHolder;
    public Rigidbody _rigidbody;

    //Bullet Stats
    public float bulletLifeTime = 4;
    public float bulletSpeed = 40;
    public float bulletDamage = 30;
    public float reloadRate;

    //Camera
    //Variables/Stats
    [Header("Camera Options")]
    public float RotationSpeed = 10;
    public float VerticalSpeed = 10;
    public float CameraDistance = 3;
    public float CameraHeight = 4;
    public float SideDistance = 1;
    public float theta = 0;
    public bool CameraCanMove = true;
    public LayerMask CameraLayerMask;

    //Functions
    public void AddPoints(int points) {Points += points;}
}
