using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Manager;

    [Header("References")]
    public GameObject Player;
    public TankData Tankdata;
    public GameObject Player2;
    public TankData Tankdata2;

    [Header("Variables")]
    public float RespawnTime = 5;
    public float Timer;
    public float MatchTime;
    private bool _inGame;

    [Header("Enemy Tanks")]
    public List<GameObject> EnemyTanks = new List<GameObject>();
    [SerializeField] private Transform _shotLocation;
    public Transform ShotLocation {
        get {
            //Returns the gameobject if there is one
            if(_shotLocation != null) return _shotLocation;

            //Creates a gameobject if there isn't any
            GameObject temp = new GameObject("ShotLocation");
            _shotLocation = temp.transform;
            return _shotLocation;
        }
        set { 
            _shotLocation = value;
        }
    }

    [Header("Terrain")]
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

    [Header("Player Options")]
    [SerializeField] private bool _singlePlayer;
    [SerializeField] private int _startingLives;
    public string PlayerOneName = "Player One";
    public string PlayerTwoName = "Player Two";

    //Public SinglePlayer setter
    public bool SinglePlayer { 
        get => _singlePlayer; 
        set { _singlePlayer = value; } 
    }

    [Header("Scenes")]
    [SerializeField] private List<string> _playScenes = new List<string>();
    private Scene _prevScene;
    public int _currentSeed { get; set; }

    [Header("Scoring")]
    public List<ScoreStruct> Scores = new List<ScoreStruct>();

    [Header("Sound")]
    public float EffectsVolume;
    public float MusicVolume;
    public bool Mute;

    [Header("Temporary")]
    [SerializeField] private bool _deleteHistory;

    private void Awake(){
        //Creates a singleton for the Game Manager
        if (Manager == null) {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }

        if(_deleteHistory) PlayerPrefs.DeleteAll();
    }

    //Exits from the first person
    private void FixedUpdate() {
        //Runs everytime the scene is changed
        if(_prevScene != SceneManager.GetActiveScene()) {
            _prevScene = SceneManager.GetActiveScene();
            _inGame = false;

            //Starts the game if it's a playable game
            if(_playScenes.Contains(_prevScene.name)) {
                Timer = 0;
                _inGame = true;
                ChangeMusic("SurfingWithTheAlien");
                StartGame();
            }
        }

        //Gets the timer
        if(_inGame) MatchManager();
        if(Mute)
            GetComponent<AudioSource>().volume = 0;
        else
            GetComponent<AudioSource>().volume = MusicVolume;
    }

    //Runs the Match
    private void MatchManager() {
        //Counts
        Timer += Time.deltaTime;

        //It's single player and the player has died
        if(_singlePlayer && Tankdata.Lives == 0 && Tankdata.CurrentHealth <= 0) {
            CompileScores();
            return;
        }

        //Both the players have died if it's two player
        if(!_singlePlayer && Tankdata.Lives == 0 && Tankdata.CurrentHealth <= 0 && Tankdata2.Lives == 0 && Tankdata2.CurrentHealth <= 0) {
            CompileScores();
            return;
        }

        //If Time Runs Out
        if(Timer > MatchTime) CompileScores();
    }

    //Compiles and handles scores
    private void CompileScores() {
        //Adds player one
        Scores.Add(new ScoreStruct(Tankdata.TankName, Tankdata.Points, Tankdata.Lives));

        //If player 2 exists
        if(Tankdata2 != null)
            Scores.Add(new ScoreStruct(Tankdata2.TankName, Tankdata2.Points, Tankdata2.Lives));

        //AI tanks
        foreach(GameObject enemyTank in EnemyTanks) {
            ScoreStruct tank = new ScoreStruct {
                Name = enemyTank.GetComponent<NPCTankData>().TankName,
                Points = enemyTank.GetComponent<NPCTankData>().Points,
                LivesRemaining = enemyTank.GetComponent<NPCTankData>().Lives
            };
            Scores.Add(tank);
        }
        //Removes all references
        RemoveReferences();

        ChangeMusic("IveBeenWaiting");
        _inGame = false;
        Timer = 0;
        SceneManager.LoadScene("GameOver");
    }

    //Changes the song that is playing
    public void ChangeMusic(string song) {
        var clip = Resources.Load<AudioClip>("Sounds/" + song);
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    //Removes all linked references to start fresh
    private void RemoveReferences() {
        foreach(Transform child in transform) Destroy(child.gameObject);
        EnemyTanks.Clear();
        Player = null;
        Tankdata = null;
        Player2 = null;
        Tankdata2 = null;
        TankSpawnPoints.Clear();
        PickUpSpawnPoints.Clear();
    }

    //It spawns the player into the world.
    private void StartGame() {
        GetComponent<LevelManager>().GenerateMap();

        //Spawns the player
        Player = SpawnTank("Player");
        Tankdata = Player.GetComponent<TankData>();
        Tankdata.Lives = _startingLives;

        //Changes Makes sure the tanks know it's 2 player mode
        if(!_singlePlayer) {
            //Spawns player 2
            Player2 = SpawnTank("Player2");
            Tankdata2 = Player2.GetComponent<TankData>();
            Tankdata2.Lives = _startingLives;

            //Fixes Camera display
            Player.GetComponent<PlayerController>().TwoPlayer();
            Player2.GetComponent<PlayerController>().TwoPlayer();
        }

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
        for(int i = 0; i < _fasterFire; i++) SpawnPickUp("FasterFire", 0);
        for(int i = 0; i < _fasterBullets; i++) SpawnPickUp("FasterBullets", 0);
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

        if(tank.GetComponent<TankData>() != null && tank.GetComponent<TankData>().Lives > 0) {
            tank.GetComponent<TankData>().Lives--;
            StartCoroutine(TankDeathCont(tank));
            return;
        }

        //Subtracts a life
        if(tank.GetComponent<NPCTankData>() != null && tank.GetComponent<NPCTankData>().Lives > 0) {
            tank.GetComponent<NPCTankData>().Lives--;
            tank.GetComponent<NPCTankData>().CurrentHealth = tank.GetComponent<NPCTankData>().MaxHealth;
            tank.GetComponent<Blinky>().UpdateHealth();
            StartCoroutine(TankDeathCont(tank));
        }
    }

    //Handles the timing of the death
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
