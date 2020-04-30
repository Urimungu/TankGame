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
        //Sets the sound
        if(GameManager.Manager.Mute)
            GetComponent<AudioSource>().volume = 0;
        else
            GetComponent<AudioSource>().volume = GameManager.Manager.EffectsVolume;

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
        if(other.gameObject.GetComponent<Blinky>() != null && Shooter != other.gameObject) {
            other.gameObject.GetComponent<Blinky>().GetHit(Damage, Shooter);
            //If a player shot this enemy
            if(Shooter.GetComponent<TankData>() != null)
                Shooter.GetComponent<TankData>().AddPoints(50);
            //If an enemy shot this enemy
            if(Shooter.GetComponent<NPCTankData>() != null)
                Shooter.GetComponent<NPCTankData>().AddPoints(50);
        }
        if(other.gameObject.GetComponent<PlayerController>() != null && Shooter != other.gameObject) {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(Damage);
            //If another player shot this player
            if(Shooter.GetComponent<TankData>() != null)
                Shooter.GetComponent<TankData>().AddPoints(50);
            //If an enemy shot this person
            if(Shooter.GetComponent<NPCTankData>() != null)
                Shooter.GetComponent<NPCTankData>().AddPoints(50);
        }

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
