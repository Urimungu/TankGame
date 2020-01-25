using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{

    public float health = 100;
    public GameObject explosion;

    void Start() {
        GameManager.GM.EnemyTanks.Add(gameObject);
    }

    //Gets run by the bullet if it comes in contact and detects that it has health
    public void GetHit(float damage, GameObject Shooter) {
        health -= damage;
        if(health <= 0) {
            if(Shooter == GameManager.GM.Player) 
                GameManager.GM.PlayerPoints++;

            GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
            boom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Destroy(gameObject);
        }
    }

}
