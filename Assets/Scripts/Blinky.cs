using UnityEngine;
using System.Collections.Generic;

public class Blinky : MonoBehaviour {
    //Stats
    private int _currentWayPoint = 0 ;

    private Transform _cannon;
    private GameObject _shotLocation;
    private Rigidbody _rigidBody;
    private Transform _transform;
    private Transform _player;
    private NPCTankData _data;
    private Movement _movement;

    //Obstacle Avoidance
    private Vector3 _newTargetPosition;

    //Variables
    public enum State { Patrol, Chase, Flee, Alert, Combat }
    private State _state;
    private float _timer;
    private float _timerSecondary;

    private void Start() {
        GameManager.Manager.EnemyTanks.Add(gameObject);
        _data = GetComponent<NPCTankData>();
        _state = State.Patrol;
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();
        _movement = GetComponent<Movement>();
        _cannon = transform.Find("Visuals").Find("CannonHolder");
        _shotLocation = _cannon.Find("ShotSpawner").gameObject;
        _data.ShotHolder = GameManager.Manager.ShotLocation.gameObject;

        //Prevents Crash if the game doesn't have a GameManager
        if(GameManager.Manager == null || GameManager.Manager.Player == null)
            return;
        _player = GameManager.Manager.Player.transform;
    }

    private void Update() {
        //Start running if the Tank is able to move
        if(_data.TankCanMove)
            StateMachine();
        //DrawGizmos();
    }

    //Controls the Enemy depending on what Stat he is in
    private void StateMachine() {
        switch(_state) {
            case State.Patrol:
            Patrol();
            break;
            case State.Chase:
            Chase();
            break;
            case State.Flee:
            Flee();
            break;
            case State.Alert:
            Alert();
            break;
            case State.Combat:
            Combat();
            break;
        }
    }

    private void Combat() {
        Vector3 aimpos = GameManager.Manager.Player.transform.position;
        aimpos.y += (GameManager.Manager.Player.transform.position - _transform.position).magnitude * Mathf.Pow(0.2f, 2);
        _cannon.LookAt(aimpos, Vector3.up);
        //Shoot
        if(Time.time > _timerSecondary) {
            _timerSecondary = Time.time + _data.ReloadRate;
            GetComponent<ShootManager>().Shoot(_data, _shotLocation, _cannon.gameObject, gameObject);
        }

        //Follows the player if he leaves the attacking range
        if((GameManager.Manager.Player.transform.position - _transform.position).magnitude > _data.FightRange) {
            _timer = Time.time + _data.AlertWaitTime  * 2;
            _state = State.Chase;
        }
    }

    private void Flee() {
        //Run Away from the player
        Vector3 newDir = GameManager.Manager.Player.transform.position;
        Vector3 tempPos = _transform.position - (newDir * 10);
        _movement.NPCMovement(_data, NewDirection(tempPos), tempPos, _rigidBody, ref _newTargetPosition, ref _cannon);
    }

    private void Chase() {
        //Moves towards the player
        _movement.NPCMovement(_data, NewDirection(GameManager.Manager.Player.transform.position), GameManager.Manager.Player.transform.position, _rigidBody, ref _newTargetPosition, ref _cannon);

        //Gets to close and starts combat
        if(_data.FightRange > (GameManager.Manager.Player.transform.position - _transform.position).magnitude) {
            _state = State.Combat;
            _rigidBody.velocity = Vector3.zero;
        }

        //If the Tank takes too much damage then he runs away
        if(_data.CurrentHealth < _data.FleeHealth) {
            _state = State.Flee;
        }

        //If the Tank doesn't find him, then return to Patrol
        if(Time.time > _timer) {
            _state = State.Patrol;
        }
    }

    //If the Tank heard the Player
    private void Alert() {

        if(SeesPlayer()) {
            _timer = Time.time + _data.AlertWaitTime * 2;
            _state = State.Chase;
            return;
        }

        //Rotates towards the player
        Vector3 newRotation = (GameManager.Manager.Player.transform.position - _transform.position);
        var tempRot = Quaternion.LookRotation(newRotation.normalized, Vector3.up);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, tempRot, Time.deltaTime * _data.TurnSpeed);

        //If it hasn't detected the player yet, then return
        if(Time.time > _timer) {
            if((GameManager.Manager.Player.transform.position - _transform.position).magnitude > _data.HearRange) {
                _state = State.Patrol;
                return;
            }
            _timer = Time.time + _data.AlertWaitTime;
        }
    }

    private void Patrol() {

        Vector3 newRotation = (GameManager.Manager.Player.transform.position - _transform.position);

        //Enemy Tank has seen the player
        if(SeesPlayer()) {
            _rigidBody.velocity = Vector3.zero;
            _state = State.Combat;
            return;
        }

        //If the Enemy Tank Hears the Player
        if((newRotation).magnitude < _data.HearRange && GameManager.Manager.Tankdata._rigidbody.velocity.magnitude > 1f) {
            _rigidBody.velocity = Vector3.zero;
            _timer = Time.time + _data.AlertWaitTime;
            _timerSecondary = Time.time + 0.5f;
            _state = State.Alert;
            return;
        }

        //Moves
        _movement.NPCMovement(_data, NewDirection(GameManager.Manager.PickUpSpawnPoints[_currentWayPoint].transform.position), 
            GameManager.Manager.PickUpSpawnPoints[_currentWayPoint].transform.position,_rigidBody, ref _newTargetPosition, ref _cannon);

        //Selects a new Way-point if they already reached the one they needed to get to
        if (Mathf.Abs((transform.position - GameManager.Manager.PickUpSpawnPoints[_currentWayPoint].transform.position).magnitude) < 0.5f){
            GameManager.Manager.PickUpSpawnPoints[_currentWayPoint] = GameManager.Manager.PickUpSpawnPoints[RandomSelector(GameManager.Manager.PickUpSpawnPoints.Count - 1)];
            _newTargetPosition = Vector3.zero;
        }
    }

    //Player is within the field of view
    private bool SeesPlayer() {
        //Points Raycast at Player
        Ray ray = new Ray(_transform.position, (_player.position - _transform.position).normalized);
        bool hitEnemy = Physics.Raycast(ray, _data.ViewDistance, _data.EnemyLayers);
        float angle = Vector3.Angle(_player.position - _transform.position, _transform.forward);

        //If the Rayhits the Player and the player is within the field of view
        if(hitEnemy && angle < _data.FieldOfView)
            return true;
        
        return false;
    }

    private void OnDrawGizmos() {

        if(_transform == null)
            return;

        //Draws the field of view of the tank
        UnityEditor.Handles.color = Color.red; 
        Vector3 _origin = _transform.position; 
        Vector3 _pos2 = _origin + ((Quaternion.AngleAxis(-_data.FieldOfView, Vector3.up)) * _transform.forward) * _data.ViewDistance; 
        Vector3 _pos1 = _origin + ((Quaternion.AngleAxis(_data.FieldOfView, Vector3.up)) * _transform.forward) * _data.ViewDistance;
        UnityEditor.Handles.DrawLine(_origin, _pos1);
        UnityEditor.Handles.DrawLine(_origin, _pos2);

        //This is the color of the arc
        Color color = Color.red;
        color.a = 0.4f;
        UnityEditor.Handles.color = color;

        var _cross = Vector3.Cross(_pos1 - _origin, _pos2 - _origin);

        //Draw the arc using everything that was just calculated
        UnityEditor.Handles.DrawSolidArc(_origin, _cross, (Quaternion.AngleAxis(_data.FieldOfView, Vector3.up)) * _transform.forward, _data.FieldOfView * 2, _data.ViewDistance);

        color = Color.grey;
        color.a = 0.2f;
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidDisc(transform.position, _cross, _data.HearRange);

        //Draws the path of the AI
        if(_newTargetPosition != Vector3.zero) {
            Debug.DrawLine(_transform.position, _newTargetPosition, Color.blue);
            Debug.DrawLine(_newTargetPosition, GameManager.Manager.PickUpSpawnPoints[_currentWayPoint].transform.position, Color.blue);
        } else {
            Debug.DrawLine(_transform.position, GameManager.Manager.PickUpSpawnPoints[_currentWayPoint].transform.position, Color.blue);
        }
    }

    //Moves towards the desired position while avoiding obstacles
    private Vector3 NewDirection(Vector3 target)
    {
        //If the Tank has reached its temporary Target Position
        if ((_transform.position - new Vector3(_newTargetPosition.x, _transform.position.y, _newTargetPosition.z)).magnitude < 0.5f)
            _newTargetPosition = Vector3.zero;

        //If the Tank hasn't reached his Obstacle Avoidance Position yet, then keep going until he does
        if (_newTargetPosition != Vector3.zero)
            return (_newTargetPosition - _transform.position).normalized;


        //If there is something in front of the Tank then Change Course
        Vector3 center = _transform.position + _transform.GetComponent<CapsuleCollider>().center;
        if (Physics.Raycast(center, (target - _transform.position).normalized, out var hit, _data.SeeDistance, _data.AvoidMask)) {
            if (hit.collider != null) {
                //If there is a wall in front of the Enemy 
                Vector3 checkRight = RightOrLeft(hit.collider);
                Vector3 checkLeft = RightOrLeft(hit.collider, false);

                //If the path, seems impossible the Tank gives up and tries something else
                if (checkRight == Vector3.zero && checkLeft == Vector3.zero) {
                    _currentWayPoint = RandomSelector(GameManager.Manager.PickUpSpawnPoints.Count - 1);
                    _newTargetPosition = Vector3.zero;
                    return Vector3.zero;
                }

                //If both are viable options, it chooses the shortest route
                float rightLength = (_transform.position - checkRight).magnitude + (checkRight - target).magnitude;
                float leftLength =  (_transform.position - checkLeft).magnitude + (checkLeft - target).magnitude; ;

                //Sets the new Target location to the new point
                if (rightLength < leftLength) {
                    Vector3 newForward = (checkRight - _transform.position).normalized;
                    _newTargetPosition = checkRight + (_transform.right * _data.AvoidDistance) + (newForward * _data.OpeningDistance);
                }else{
                    Vector3 newForward = (checkLeft - _transform.position).normalized;
                    _newTargetPosition = checkLeft - (_transform.right * _data.AvoidDistance) + (newForward * _data.OpeningDistance);
                }

                //Tries to keep everything level
                _newTargetPosition.y = _transform.position.y;
                return (_newTargetPosition - _transform.position).normalized;
            }
        }

        //Returns Regular Forward if nothing is in the way
        return (target - _transform.position).normalized;
    }
    //Chooses to see if it's better to go right or left
    private Vector3 RightOrLeft(Collider hit, bool right = true) {

        //Initializes variables
        Vector3 center = _transform.position + _transform.GetComponent<CapsuleCollider>().center;
        Vector3 lastPos = Vector3.zero;
        float angle = 0;

        //Runs through each 
        for (int i = 0; i < 20; i++) {
            Physics.Raycast(center, (_transform.forward + _transform.right * angle).normalized, out var currentHit, 30, _data.AvoidMask);
            if (currentHit.collider != hit)
                return lastPos;

            lastPos = currentHit.point;
            lastPos.y = _transform.position.y;
            angle += 0.25f * (right ? 1 : -1);
        }

        return Vector3.zero;
    }

    //Gets run by the bullet if it comes in contact and detects that it has health
    public void GetHit(float damage, GameObject shooter) {
        _data.CurrentHealth -= damage;
        if(_data.CurrentHealth <= 0) {
            if(shooter == _player) 
                GameManager.Manager.PlayerPoints++;

            //Creates explosion on Destruction
            GameObject boom = Instantiate(_data.Explosion, transform.position, Quaternion.identity, transform.parent);
            boom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Destroy(gameObject);
        }
    }

    //Selects a Random number that isn't the one that already exists
    private int RandomSelector(int max) {
        //Initialize
        int randomNumber = Random.Range(0, max);

        //Continues until it picks a unique number
        while(_currentWayPoint == randomNumber)
            randomNumber = Random.Range(0, max);

        return randomNumber;
    }
}
