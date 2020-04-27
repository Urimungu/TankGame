using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> _menues = new List<GameObject>();

    private void Awake() {
        UpdateMenues(0);
    }


    //Updates which menu is open
    public void UpdateMenues(int menu) {
        //Turns on the menu that matches with the int
        for(int i = 0; i < _menues.Count; i++) 
            _menues[i].SetActive(i == menu);
    }

    //Exits and closes the game
    public void QuitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    //Starts Singleplayer
    public void StartSinglePlayer() {
        GameManager.Manager.SinglePlayer = true;
        SceneManager.LoadScene("SinglePlayer");
    }

    //Start Multiplayer
    public void StartTwoPlayer() {
        SceneManager.LoadScene("TwoPlayer");
    }

}
