using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreStruct{

    public string Name;
    public int Points;
    public int LivesRemaining;

    public ScoreStruct() {
        Name = "";
        Points = 0;
        LivesRemaining = 0;
    }

    public ScoreStruct(string name, int points, int livesRemaining) {
        Name = name;
        Points = points;
        LivesRemaining = livesRemaining;
    }
}
