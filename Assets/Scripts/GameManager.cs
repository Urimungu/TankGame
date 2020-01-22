using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager GM;

    //References
    [Header("References")]
    public GameObject Player;
    public GameObject MainCamera;

    //Variables
    private bool MouseLocked;
    private void Awake()
    {
        //Creates a singleton for the Game Manager
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }

        //References the player/Tank
        Player = GameObject.FindGameObjectWithTag("Player");
        MouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        MainCamera = Camera.main.gameObject;
    }

    private void Update() {
        //Locks the mouse at the center of the screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MouseLocked = !MouseLocked;
            if (MouseLocked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
