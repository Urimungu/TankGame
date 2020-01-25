﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float LifeTime = 4;

    //Starts the counter
    private void Start() {
        StartCoroutine(death());
    }

    //Counts to the seconds of life and then destroys itself
    IEnumerator death() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}