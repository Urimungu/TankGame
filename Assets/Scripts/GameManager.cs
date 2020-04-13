using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Manager;

    //References
    [Header("References")]
    public GameObject Player;
    public TankData Tankdata;

    //Variables
    private bool MouseLocked = true;
    public float PlayerPoints = 0;
    public float RespawnTime = 5;

    //Enemy Tanks
    public List<GameObject> EnemyTanks = new List<GameObject>();
    public Transform ShotLocation; 

    //Terrain
    public List<GameObject> TankSpawnPoints = new List<GameObject>();
    public List<GameObject> PickUpSpawnPoints = new List<GameObject>();

    [Header("PickUps")]
    [SerializeField] private int _healthUps;
    [SerializeField] private int _damageUps;
    [SerializeField] private int _speedUps;
    [SerializeField] private int _fasterBullets;
    [SerializeField] private int _fasterFire;
    private List<int> _pickUpLocations = new List<int>();
    private GameObject _pickUpHolder;

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
        Tankdata = GetComponent<TankData>();
        StartGame();
    }

    //Exits from the first person
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

    //It spawns the player into the world.
    private void StartGame() {
        GetComponent<LevelManager>().GenerateMap();

        //Spawns the player
        Player = SpawnTank("Player");
        Tankdata = Player.GetComponent<TankData>();
        
        //Spawns Blinky and adds all the Pickup locations as waypoints
        GameObject tank = SpawnTank("Blinky");
        EnemyTanks.Add(tank);

        //Spawns Stinky and adds all the Pickup locations as waypoints
        tank = SpawnTank("Stinky");
        EnemyTanks.Add(tank);

        //Spawns Pinky and adds all the Pickup locations as waypoints
        tank = SpawnTank("Pinky");
        EnemyTanks.Add(tank);

        //Spawns God Butcher and adds all the Pickup locations as waypoints
        tank = SpawnTank("GodButcher");
        EnemyTanks.Add(tank);


        //Spawns in the PickUps
        for(int i = 0; i < _healthUps; i++) SpawnPickUp("HealthUp", 0);
        for(int i = 0; i < _damageUps; i++) SpawnPickUp("DamageUp", 0);
        for(int i = 0; i < _speedUps; i++) SpawnPickUp("SpeedUp", 0);
        for(int i = 0; i < _speedUps; i++) SpawnPickUp("FasterFire", 0);
        for(int i = 0; i < _speedUps; i++) SpawnPickUp("FasterBullets", 0);
    }

    //Spawns the tanks with the specific name
    private GameObject SpawnTank(string tankName) {
        return Instantiate(Resources.Load<GameObject>("Prefabs/Tanks/" + tankName), 
            TankSpawnPoints[Random.Range(0, TankSpawnPoints.Count)].transform.position, Quaternion.identity);
    }

    //Controls the spawning of Pick Ups
    public void SpawnPickUp(string pickupname, float time, int remove = -1) {
        //Creates a holder for the pick ups if there is none
        if(_pickUpHolder == null) _pickUpHolder = new GameObject("PickUpHolder");

        //Takes off the location that has already been picked up and makes it available again
        if(remove != -1) _pickUpLocations.Remove(remove);

        StartCoroutine(PickUpSpawner(pickupname, time));
    }

    //Spawns in a pick up
    IEnumerator PickUpSpawner(string pickupname, float time) {
        yield return new WaitForSeconds(time);

        //Chooses a unique place to spawn a pick up
        int spawnPoint = Random.Range(0, PickUpSpawnPoints.Count);
        while(_pickUpLocations.Contains(spawnPoint)) spawnPoint = Random.Range(0, PickUpSpawnPoints.Count);

        _pickUpLocations.Add(spawnPoint);

        Vector3 pos = PickUpSpawnPoints[spawnPoint].transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/PickUps/" + pickupname), pos, Quaternion.identity, _pickUpHolder.transform);
        go.name = spawnPoint.ToString();
    }
    //Handles the player dying and enemy tanks dying
    public void TankDeath(GameObject tank) {
        tank.SetActive(false);
        StartCoroutine(TankDeathCont(tank));
    }

    IEnumerator TankDeathCont(GameObject tank) {
        //Waits the respawning time
        yield return new WaitForSeconds(RespawnTime);
        tank.transform.position = TankSpawnPoints[Random.Range(0, TankSpawnPoints.Count)].transform.position;

        //Replenishes the health
        if(tank.GetComponent<TankData>() != null) tank.GetComponent<TankData>().CurrentHealth = tank.GetComponent<TankData>().MaxHealth;
        if(tank.GetComponent<NPCTankData>() != null) tank.GetComponent<NPCTankData>().CurrentHealth = tank.GetComponent<NPCTankData>().MaxHealth;

        tank.SetActive(true);
    }
}
