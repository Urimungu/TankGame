using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float LifeTime = 4;

    //Starts the counter
    private void Start() {
        //Sets the volume
        if(GameManager.Manager.Mute)
            GetComponent<AudioSource>().volume = 0;
        else
            GetComponent<AudioSource>().volume = GameManager.Manager.EffectsVolume;
        StartCoroutine(death());
    }

    //Counts to the seconds of life and then destroys itself
    IEnumerator death() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
