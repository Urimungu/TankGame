using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DamageUp : MonoBehaviour
{
    //Variables
    [SerializeField] private float _multiplier = 2;
    [SerializeField] private int _activeTime = 10;
    [SerializeField] private float _spawnTime = 5;
    [SerializeField] private Vector3 _spinRate;

    private bool _isRunning;

    private void Update() {
        //Spins the powerup
        if(!_isRunning) transform.Rotate(_spinRate);
    }

    //Whomever collides with the trigger first
    private void OnTriggerEnter(Collider other) {
        //Player
        if(other.GetComponent<TankData>() != null && !_isRunning) {
            _isRunning = true;
            StartCoroutine(PlayerBonus(other.GetComponent<TankData>()));
            return;
        }

        //Enemy
        if(other.GetComponent<NPCTankData>() != null && !_isRunning) {
            _isRunning = true;
            StartCoroutine(EnemyBonus(other.GetComponent<NPCTankData>()));
            return;
        }
    }

    //Gives the powerup to the player if they pick it up
    private IEnumerator PlayerBonus(TankData data) {
        //Buffs the player
        data.bulletDamage *= _multiplier;

        //Turns itself off while makingthe script keep running
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        //Waits
        yield return new WaitForSeconds(_activeTime);

        //Removes the buff and destroys itself
        data.bulletDamage /= _multiplier;
        GameManager.Manager.SpawnPickUp("DamageUp", _spawnTime, int.Parse(name));
        Destroy(gameObject);
    }

    //Gives the powerup to the enemy if they pick it up
    private IEnumerator EnemyBonus(NPCTankData data) {
        //Buffs the enemy
        data.BulletDamage *= _multiplier;

        //Turns itself off while making the script keep running
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        //Waits
        yield return new WaitForSeconds(_activeTime);

        //Removes the buff and destroys itself
        data.BulletDamage /= _multiplier;
        GameManager.Manager.SpawnPickUp("DamageUp", _spawnTime, int.Parse(name));
        Destroy(gameObject);
    }
}
