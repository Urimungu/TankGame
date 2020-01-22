using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 1;
    private bool isRunning = false;
    private float speed = 30;

    public void Enter(float life, float speed)
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);

        lifeTime = life;

        if (!isRunning)
            StartCoroutine(deathTimer());

    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
