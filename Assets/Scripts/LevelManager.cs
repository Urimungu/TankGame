using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> RoomTypes = new List<GameObject>();

    [Header("Dimensions")]
    [SerializeField] private float Collumbs;
    [SerializeField] private float Rows;
    [SerializeField] private bool RoomOfTheDay;
    [SerializeField] private int CustomSeed;

    private int GenerateSeed() {
        //Returns Custom Seed
        if(CustomSeed != 0)
            return CustomSeed;

        //Returns Level of the day
        if(RoomOfTheDay) {
            System.DateTime today = System.DateTime.Today;
            int seed = today.Month + today.Day + today.Year;
            return seed;
        }
        
        //Returns a random seed
        return Random.Range(0, 100000);
    }

    public void GenerateMap() {

        //Seed
        int seed = GenerateSeed();
        Random.InitState(seed);

        //Creates the Level
        for(int x = 0; x < Collumbs; x++) {
            for(int y = 0; y < Rows; y++) {
                GameObject room = Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)], Vector3.zero, Quaternion.identity, transform);
                room.name = "Room: " + ((x * Rows) + y + 1);

                //Adds the spawn points to the game manager
                GameManager.Manager.TankSpawnPoints.Add(room.transform.Find("TankSpawn").gameObject);
                GameManager.Manager.PickUpSpawnPoints.Add(room.transform.Find("PickUpSpawn").gameObject);

                //Positions them in a grid like pattern
                float roomWidth = room.transform.localScale.x * 10;
                float roomLength = room.transform.localScale.z * 10;
                room.transform.position = new Vector3(transform.position.x + (y * roomWidth), 0, transform.position.z + (x * roomLength));

                //Removes bottom Wall if not at the bottom and left wall if not at the edge
                if(x != 0) {Destroy(room.transform.Find("SouthWall").gameObject);}
                if(y != 0) { Destroy(room.transform.Find("WestWall").gameObject);}

                //Opens the doors if they are in between other ones
                if(x != Collumbs - 1) { Destroy(room.transform.Find("NorthWall").Find("Door").gameObject); }
                if(y != Rows - 1) { Destroy(room.transform.Find("EastWall").Find("Door").gameObject); }
            }
        }
        
    }
}
