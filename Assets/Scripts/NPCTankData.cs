using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCTankData
{
    //Tanks
    [Header("Tank Stats")]
    //Movement
    public float TankSpeed = 6;
    public float MaxSpeed = 10;
    public float ReverseSpeed = 5;
    public float StopSpeed = 10;
    public float TurnSpeed = 250;
    public bool TankCanMove = true;

    //Obstacle Avoidance
    [Header("Avoidance")]
    public float SeeDistance = 6;
    public float AvoidDistance = 2.5f;
    public float OpeningDistance = 2.5f;
    public LayerMask AvoidMask;

    //Stats
    [Header("Health")]
    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public float FleeHealth = 10;

    //Bullet
    [Header("Shooting")]
    public GameObject Shot;
    public GameObject Explosion;
    public GameObject ShotHolder;


    //Bullet Stats
    public float BulletLifeTime = 4;
    public float BulletSpeed = 40;
    public float BulletDamage = 30;
    public float ReloadRate = 0.4f;

    //Personality
    [Header("Personality")]
    public float HearRange = 10;
    public float AlertWaitTime = 3;
    public float FieldOfView = 30;
    public float ViewDistance = 10;
    public float FightRange = 10;
    public LayerMask EnemyLayers;
}
