using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTankData : MonoBehaviour
{
    //Tanks
    [Header("Tank Stats")]
    public float TankAcc = 10;
    public float MaxSpeed = 15;
    public float ReverseMax = 10;
    public float StopSpeed = 10;
    public bool TankCanMove = true;

    //Bullet
    [Header("Shooting")]
    public GameObject shot;
    public GameObject ShotHolder;

    public float bulletLifeTime = 4;
    public float bulletSpeed = 40;
    public float bulletDamage = 30;

    public float reloadRate;
}
