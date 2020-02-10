using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTankData : MonoBehaviour
{
    //Tanks
    [Header("Tank Stats")]
    //Movement
    public float TankSpeed = 10;
    public float MaxSpeed = 15;
    public float ReverseSpeed = 10;
    public float StopSpeed = 10;
    public bool TankCanMove = true;
    //Stats
    public float Health = 100;

    //Bullet
    [Header("Shooting")]
    public GameObject Shot;
    public GameObject ShotHolder;

    //Bullet Stats
    public float BulletLifeTime = 4;
    public float BulletSpeed = 40;
    public float BulletDamage = 30;
    public float ReloadRate = 0.4f;
}
