using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour{

    public void RegularMovement(float hor, float ver, TankData trans, Transform cam) {
        //Removes the side speed boost
        Vector2 MoveDirection = new Vector2(hor, ver).normalized;

        //Moves the player forwards and right
        trans._rigidbody.AddForce(cam.forward * trans.TankAcc * MoveDirection.y, ForceMode.Acceleration);
        trans._rigidbody.AddForce(cam.right * trans.TankAcc * MoveDirection.x, ForceMode.Acceleration);

        //Makes the speed different if the player is moving backwards
        float SpeedCap = ver > 0.1f ? trans.MaxSpeed : trans.ReverseMax;

        //Slows the player down if they are going too fast
        if(Mathf.Abs(trans._rigidbody.velocity.magnitude) > SpeedCap) {
            trans._rigidbody.AddForce(-trans._rigidbody.velocity.normalized * trans.MaxSpeed);
        }

        //Slowly comes to a stop
        if(Mathf.Abs(trans._rigidbody.velocity.magnitude) > 0.2f && Mathf.Abs(ver) < 0.1f && Mathf.Abs(hor) < 0.1f) {
            trans._rigidbody.AddForce(trans._rigidbody.velocity.normalized * -trans.StopSpeed);
        }

    }

    //Moves the Tank from point A to point B
    public void NPCMovement(NPCTankData _data, Vector3 NewDirection, Vector3 target, Rigidbody _rigidBody, ref Vector3 _newTargetPosition, ref Transform Cannon) {
        //Moves towards target
        var newDirection = NewDirection * _data.TankSpeed;
        _rigidBody.velocity = new Vector3(newDirection.x, _rigidBody.velocity.y, newDirection.z);
        Vector3 newRotation = new Vector3(_newTargetPosition.x, 0, _newTargetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z);
        var tempRot = Quaternion.LookRotation(newRotation.normalized, Vector3.up);

        //Rotates properly
        if(_newTargetPosition == Vector3.zero) {
            Vector3 newRotate = new Vector3(target.x, 0, target.z) - new Vector3(transform.position.x, 0, transform.position.z);
            tempRot = Quaternion.LookRotation(newRotate, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, tempRot, Time.deltaTime * _data.TurnSpeed);

            //Looks at Way-point
            Cannon.LookAt(target, Vector3.up);
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, tempRot, Time.deltaTime * _data.TurnSpeed);
        Cannon.LookAt(_newTargetPosition, Vector3.up);

    }
}
