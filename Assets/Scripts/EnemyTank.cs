using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    //Stats
    private int _currentWayPoint;

    //References
    public GameObject Explosion;
    public GameObject Cannon;
    public Transform[] WayPoints;

    private Rigidbody _rigidBody;
    private Transform _transform;
    private Transform _player;
    private NPCTankData _data;

    //Obstacle Avoidance
    private Vector3 _newTargetPosition;

    public enum State { Patrol, Chase, Flee}
    private State _state;

    private void Start() {
        //Sets all of the References
        _data = GetComponent<NPCTankData>();
        _state = State.Patrol;
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();

        //Prevents Crash if the game doesn't have a GameManager
        if (GameManager.Manager == null || GameManager.Manager.Player == null)
            return;
        _player = GameManager.Manager.Player.transform;
    }

    private void Update() {
        //Start running if the Tank is able to move
        if(_data.TankCanMove)
            StateMachine();
    }

    //Controls the Enemy depending on what Stat he is in
    private void StateMachine() {
        switch (_state) {
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
        //Moves
        Movement();

        //Looks at Way-point
        if(WayPoints.Length > 0)
            Cannon.transform.LookAt(WayPoints[_currentWayPoint].position, Vector3.up);

        //Selects a new Way-point if they already reached the one they needed to get to
        if (Mathf.Abs((transform.position - WayPoints[_currentWayPoint].position).magnitude) < 0.5f){
            WayPoints[_currentWayPoint] = WayPoints[RandomSelector(WayPoints.Length - 1)];
        }
    }

    //Moves the Tank from point A to point B
    private void Movement() {
        //Moves towards target
        var newDirection = NewDirection() * _data.TankSpeed;
        _rigidBody.velocity = new Vector3(newDirection.x, _rigidBody.velocity.y, newDirection.z);
        _transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);

    }

    private Vector3 NewDirection()
    {
        //If the Tank has reached its temporary Target Position
        if ((_transform.position - _newTargetPosition).magnitude < 0.5f)
            _newTargetPosition = Vector3.zero;

        //If the Tank hasn't reached his Obstacle Avoidance Position yet, then keep going until he does
        if (_newTargetPosition != Vector3.zero)
            return (_newTargetPosition - _transform.position).normalized;

        //If there is something in front of the Tank then Change Course
        RaycastHit hit;
        if (Physics.Raycast(_transform.position, _transform.forward, out hit, _data.AvoidDistance, _data.AvoidMask)) {
            if (hit.collider != null)
                RightOrLeft(hit.collider);
        }

        //Returns Regular Forward if nothing is in the way
        return (WayPoints[_currentWayPoint].position - _transform.position).normalized;
    }

    //False is Left, True is Right
    private void RightOrLeft(Collider hit) {

        RaycastHit currentHit;
        Vector3 center = _transform.position + _transform.GetComponent<CapsuleCollider>().center;

        float angle = 0;
        for (int i = 0; i < 20; i++) {
            Physics.Raycast(center, (_transform.forward + _transform.right * angle).normalized, out currentHit, 30, _data.AvoidMask);
            if(currentHit.collider != hit)
                return;

            angle += 0.25f;
        }
    }

    //Gets run by the bullet if it comes in contact and detects that it has health
    public void GetHit(float damage, GameObject shooter) {
        _data.Health -= damage;
        if(_data.Health <= 0) {
            if(shooter == _player) 
                GameManager.Manager.PlayerPoints++;

            //Creates explosion on Destruction
            GameObject boom = Instantiate(Explosion, transform.position, Quaternion.identity, transform.parent);
            boom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Destroy(gameObject);
        }
    }

    //Selects a Random number that isn't the one that already exists
    private int RandomSelector(int max) {
        int randomNumber = Random.Range(0, max);
        if(_currentWayPoint == randomNumber) {
            if(_currentWayPoint == WayPoints.Length)
                randomNumber--;
            else
                randomNumber++;
        }

        return randomNumber;
    }
}
