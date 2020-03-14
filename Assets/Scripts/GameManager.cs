using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Manager;

    //References
    [Header("References")]
    public GameObject Player;
    public GameObject MainCamera;
    public TankData Tankdata;

    //Variables
    private bool MouseLocked = true;
    public float PlayerPoints = 0;

    public List<GameObject> EnemyTanks = new List<GameObject>();

    //Enemy Tank Data
    public enum Personality { Blinky, Pinky, Stinky, GodButcher }
    public NPCTankData Blinky;
    public NPCTankData Pinky;
    public NPCTankData Stinky;
    public NPCTankData GodButcher;

    private void Awake()
    {
        //Creates a singleton for the Game Manager
        if (Manager == null) {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }

        //Locks the Mouse position
        Cursor.lockState = CursorLockMode.Locked;
        MainCamera = Camera.main.gameObject;
        Tankdata = GetComponent<TankData>();
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
