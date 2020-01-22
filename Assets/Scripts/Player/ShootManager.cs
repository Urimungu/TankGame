using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public GameObject shot;
    public GameObject ShotHolder;

    private GameObject Cannon;
    private GameObject CannonHolder;

    public float reloadRate;
    private float timer;

    void Start()
    {
        CannonHolder = GameManager.GM.Player.transform.GetChild(0).Find("CannonHolder").gameObject;
        Cannon = CannonHolder.transform.Find("ShotSpawner").gameObject;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timer)
        {
            timer = Time.time + reloadRate;
            Shoot();
        }

    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(shot, Cannon.transform.position, CannonHolder.transform.rotation,
            ShotHolder.transform);
        bullet.GetComponent<Bullet>().Enter(3, 40);
        Cannon.GetComponent<ParticleSystem>().Play();
        StartCoroutine(lightThing());
    }

    IEnumerator lightThing()
    {
        Cannon.GetComponent<Light>().intensity = 15;
        yield return new WaitForSeconds(0.3f);
        Cannon.GetComponent<Light>().intensity = 0;
    }

}
