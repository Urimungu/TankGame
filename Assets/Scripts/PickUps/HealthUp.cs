using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    //Variables
    [SerializeField] private float _gain = 50;
    [SerializeField] private float _spawnTime = 5;
    [SerializeField] private Vector3 _spinRate;

    private void Update() {
        //Spins the powerup
        transform.Rotate(_spinRate);
    }

    //Whomever collides with the trigger first
    private void OnTriggerEnter(Collider other) {
        //Player
        if(other.GetComponent<TankData>() != null) {
            PlayerBonus(other.GetComponent<TankData>());
            return;
        }

        //Enemy
        if(other.GetComponent<NPCTankData>() != null) {
            EnemyBonus(other.GetComponent<NPCTankData>());
            return;
        }
    }

    //Gives the powerup to the player if they pick it up
    private void PlayerBonus(TankData data) {
        //Gives the player health
        data.CurrentHealth += _gain;
        if(data.CurrentHealth > data.MaxHealth) data.CurrentHealth = data.MaxHealth;

        //Spawns the next one in and destroys itself
        GameManager.Manager.SpawnPickUp("HealthUp", _spawnTime, int.Parse(name));
        Destroy(gameObject);
    }

    //Gives the powerup to the enemy if they pick it up
    private void EnemyBonus(NPCTankData data) {
        //Gives the enemy health
        data.CurrentHealth *= _gain;
        if(data.CurrentHealth > data.MaxHealth) data.CurrentHealth = data.MaxHealth;

        //Spawns the next one in and destroys itself
        GameManager.Manager.SpawnPickUp("HealthUp", _spawnTime, int.Parse(name));
        Destroy(gameObject);
    }
}
