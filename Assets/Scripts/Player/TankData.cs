﻿using System.Collections;
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
    public float turnSpeed = 10;
    public float TankHealth = 100;
    public bool TankCanMove = true;

    //Bullet
    [Header("Shooting")]
    public GameObject shot;
    public GameObject ShotHolder;

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
    public float HeightLength = 2;
    public float SideDistance = 1;
    public float theta = 0;
    public bool CameraCanMove = true;
    public LayerMask CameraLayerMask;
}
