using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    //Stats
    public float health = 100;
    public bool canMove = true;
    public int currentWayPoint = 0;

    //References
    public GameObject explosion;
    public GameObject Cannon;
    public Transform[] WayPoints;
    private Rigidbody rb;
    private Transform TF;


    public enum State { Patrol, Chase, Flee}
    private State state;

    void Start() {
        GameManager.GM.EnemyTanks.Add(gameObject);
        state = State.Patrol;
        TF = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if(canMove)
            StateMachine();

    }

    private void StateMachine() {
        switch (state) {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                break;
            case State.Flee:
                break;
        }
    }

    private void Patrol() {
        if(WayPoints.Length > 0)
            Cannon.transform.LookAt(WayPoints[currentWayPoint].position, Vector3.up);

    }

    private void MoveTowards(Vector3 location) {


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
