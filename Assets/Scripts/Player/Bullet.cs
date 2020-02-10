using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    private GameObject Shooter;

    private float lifeTime = 1;
    private bool isRunning = false;
    private float Damage = 30;

    public void Enter(float life, float speed, float damage, GameObject shooter){
        //Moves the bullet forward
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);

        //Sets the values
        Damage = damage;
        lifeTime = life;
        Shooter = shooter;

        //Begins the Death Counter
        if (!isRunning)
            StartCoroutine(deathTimer());

    }

    //Checks to see if it has hit anything
    void OnCollisionEnter(Collision other){
        //Damages enemy if there is one
        if(other.gameObject.GetComponent<EnemyTank>() != null)
            other.gameObject.GetComponent<EnemyTank>().GetHit(Damage, Shooter);

        //Regardless of what it hits, it destroys itself and explodes
        SpawnExplode();
        Destroy(gameObject);
    }

    //Spawns in the explosion at that collision point
    private void SpawnExplode() {
        GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
        boom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }

    //If it hasn't hit anything at this point then it will destroy itself and not spawn anything
    IEnumerator deathTimer(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
