using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Level;

    [Header("Dimensions")]
    public float Collumbs;
    public float Rows;

    private void Awake() {
        GenerateMap();
    }
    private void GenerateMap() {

        //Creates the Level
        for(int x = 0; x < Collumbs; x++) {
            for(int y = 0; y < Rows; y++) {
                GameObject room = Instantiate(Level, Vector3.zero, Quaternion.identity, transform);
                room.name = "Room: " + ((x * Rows) + y + 1);

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
