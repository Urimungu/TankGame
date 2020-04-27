using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{

    //Fires a bullet
    public void Shoot(TankData TS, GameObject Cannon, GameObject CannonHolder, GameObject shooter){
        GameObject bullet = Instantiate(TS.shot, Cannon.transform.position, CannonHolder.transform.rotation, TS.ShotHolder.transform);
        GetComponent<AudioSource>().volume = GameManager.Manager.EffectsVolume;
        GetComponent<AudioSource>().Play();

        //Changes the bullet states and plays the animations
        bullet.GetComponent<Bullet>().Enter(TS.bulletLifeTime, TS.bulletSpeed, TS.bulletDamage, shooter);
        Cannon.GetComponent<ParticleSystem>().Play();
        StartCoroutine(lightThing(Cannon));
    }

    //Shoots for the Enemy Tank
    public void Shoot(NPCTankData TS, GameObject Cannon, GameObject CannonHolder, GameObject shooter) {
        GameObject bullet = Instantiate(TS.Shot, Cannon.transform.position, CannonHolder.transform.rotation, TS.ShotHolder.transform);
        GetComponent<AudioSource>().volume = GameManager.Manager.EffectsVolume;
        GetComponent<AudioSource>().Play();

        //Changes the bullet states and plays the animations
        bullet.GetComponent<Bullet>().Enter(TS.BulletLifeTime, TS.BulletSpeed, TS.BulletDamage, shooter);
        Cannon.GetComponent<ParticleSystem>().Play();
        StartCoroutine(lightThing(Cannon));
    }

    //Lights up for a split second to show that it fired
    IEnumerator lightThing(GameObject Cannon){
        Cannon.GetComponent<Light>().intensity = 15;
        yield return new WaitForSeconds(0.05f);
        Cannon.GetComponent<Light>().intensity = 0;
    }

}
