using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
 
    private GameObject Cannon;
    private GameObject CannonHolder;

    private TankData TS;

    private float timer;

    void Start()
    {
        //Initializes the variables
        TS = GameManager.GM.transform.GetComponent<TankData>();
        CannonHolder = GameManager.GM.Player.transform.GetChild(0).Find("CannonHolder").gameObject;
        Cannon = CannonHolder.transform.Find("ShotSpawner").gameObject;
    }

    private void Update()
    {
        //Checks if butten is pressed and it can in the fire rate.
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timer)
        {
            timer = Time.time + TS.reloadRate;
            Shoot();
        }

    }

    //Fires a bullet
    private void Shoot()
    {
        GameObject bullet = Instantiate(TS.shot, Cannon.transform.position, CannonHolder.transform.rotation,
            TS.ShotHolder.transform);
        //Changes the bullet states and plays teh animations
        bullet.GetComponent<Bullet>().Enter(TS.bulletLifeTime, TS.bulletSpeed, TS.bulletDamage, GameManager.GM.Player);
        Cannon.GetComponent<ParticleSystem>().Play();
        StartCoroutine(lightThing());
    }

    //Lights up for a split second to show that it fired
    IEnumerator lightThing(){
        Cannon.GetComponent<Light>().intensity = 15;
        yield return new WaitForSeconds(0.05f);
        Cannon.GetComponent<Light>().intensity = 0;
    }

}
