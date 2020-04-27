using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    [SerializeField] private List<ScoreStruct> _scores = new List<ScoreStruct>();

    //Runs the functions
    private void Start() {
        LoadScores();
        CombineScores();
        SortScores();
        DisplayScores();
        SaveScores();
    }

    //Gets the scores from the main menu
    private void LoadScores() {
        for(int i = 0; i < transform.childCount - 1; i++) {
            //Creates the new score struct
            var score = new ScoreStruct {
                Name = PlayerPrefs.GetString("SCORETANKNAME" + i, ""),
                Points = PlayerPrefs.GetInt("SCOREPOINTS" + i, 0),
                LivesRemaining = PlayerPrefs.GetInt("SCORELIVESREMAINING" + i, 0)
            };

            //Adds it to the list
            _scores.Add(score);
        }
    }

    //Combines the new scores and the old scores
    private void CombineScores() {
        //Ads in the scores from the game manager
        for(int i = 0; i < GameManager.Manager.Scores.Count; i++) {
            //Creates a new instance of the reference
            var score = new ScoreStruct { 
                Name = GameManager.Manager.Scores[i].Name, 
                Points = GameManager.Manager.Scores[i].Points,
                LivesRemaining = GameManager.Manager.Scores[i].LivesRemaining
            };
            _scores.Add(score);
        }
    }

    //Sorts the scores from Hightest points to lowest points
    private void SortScores() {
        for(int i = 0; i < _scores.Count; i++) {
            //Initializes to the first of the list
            var biggest = _scores[i];

            //Breaks if it reached the second to last one
            if(i == _scores.Count - 1) break;

            //Counts from the next one to see if it's bigger than the last
            for(int j = i + 1; j < _scores.Count; j++) {
                if(biggest.Points < _scores[j].Points && biggest.LivesRemaining < _scores[j].LivesRemaining) {
                    //Switches the two Scores if the scores are bigger
                    _scores[i] = _scores[j];
                    _scores[j] = biggest;
                    biggest = _scores[i];
                } else if(biggest.Points < _scores[j].Points) {
                    //Switches the two Scores if the score is the same but one has more lives
                    _scores[i] = _scores[j];
                    _scores[j] = biggest;
                    biggest = _scores[i];
                } else if(biggest.Name == "" && _scores[j].Name != "") {
                    //Switches the two Scores if the biggest one is blank
                    _scores[i] = _scores[j];
                    _scores[j] = biggest;
                    biggest = _scores[i];
                }
            }
        }
    }

    //Physically displays it on the leaderboard
    private void DisplayScores() {
        //Gets the children located under this game object
        for(int i = 1; i < transform.childCount; i++) {
            //Updates the Names, Points, and Lives of each display
            transform.GetChild(i).Find("Name").GetComponent<Text>().text = _scores[i - 1].Name;
            transform.GetChild(i).Find("Points").GetComponent<Text>().text = _scores[i - 1].Points.ToString();
            transform.GetChild(i).Find("Lives").GetComponent<Text>().text = _scores[i - 1].LivesRemaining.ToString();
        }
    }

    //Saves the scores
    private void SaveScores() {

        //Saves the scores to the player prefs
        for(int i = 0; i < _scores.Count; i++) {
            PlayerPrefs.SetString("SCORETANKNAME" + i, _scores[i].Name);
            PlayerPrefs.SetInt("SCOREPOINTS" + i, _scores[i].Points);
            PlayerPrefs.SetInt("SCORELIVESREMAINING" + i, _scores[i].LivesRemaining);
        }

        //Removes the Scores from the game manager
        GameManager.Manager.Scores.Clear();
    }

    //Returns to main Menu
    public void ReturnToMenu() {
        GameManager.Manager.ChangeMusic("ButterCup");
        SceneManager.LoadScene("MainMenu");
    }

    //Exits the game
    public void Quit() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
