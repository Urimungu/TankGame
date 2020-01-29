using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : MonoBehaviour
{
    //Stats
    public float health = 100;
    public bool canMove = true;
    private int curWayPoint;

    //References
    public GameObject explosion;
    public GameObject Cannon;
    public Transform[] WayPoints;
    public Transform currentWaypoint;

    private Rigidbody rb;
    private Transform TF;
    private NavMeshAgent Nav;


    public enum State { Patrol, Chase, Flee}
    private State state;

    void Start() {
        GameManager.GM.EnemyTanks.Add(gameObject);
        state = State.Patrol;
        TF = transform;
        rb = GetComponent<Rigidbody>();
        Nav = GetComponent<NavMeshAgent>();
        currentWaypoint = WayPoints[0];
        Nav.SetDestination(currentWaypoint.position);
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
            Cannon.transform.LookAt(WayPoints[curWayPoint].position, Vector3.up);

        if (Mathf.Abs((transform.position - currentWaypoint.position).magnitude) < 0.5f){
            int randomNumber = Random.Range(0, WayPoints.Length - 1);
            if (curWayPoint == randomNumber) {
                if (curWayPoint == WayPoints.Length)
                    randomNumber--;
                else
                    randomNumber++;
            }

            curWayPoint = randomNumber;

            currentWaypoint = WayPoints[curWayPoint];
            Nav.SetDestination(currentWaypoint.position);
        }
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
