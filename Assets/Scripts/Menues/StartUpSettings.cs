using UnityEngine;
using UnityEngine.UI;

public class StartUpSettings : MonoBehaviour{

    //Changes the name according to the player
    public void UpdatePlayerOneName(InputField playerOneName) {
        GameManager.Manager.PlayerOneName = playerOneName.text;
    }

    //Updates the name of the second player
    public void UpdatePlayerTwoName(InputField playerTwoName) {
        GameManager.Manager.PlayerTwoName = playerTwoName.text;
    }

    //Loads in the name of the players
    public void LoadPlayerOneName(InputField player) {
        player.text = GameManager.Manager.PlayerOneName;
    }

    //Loads in the name of the players
    public void LoadPlayerTwoName(InputField player) {
        player.text = GameManager.Manager.PlayerTwoName;
    }

}
