using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Text _name;
    [SerializeField] private Text _time;
    [SerializeField] private Text _lives;
    [SerializeField] private Text _points;
    [SerializeField] private Text _seed;
    [SerializeField] private Image _health;

    [Header("Target")]
    [SerializeField] private TankData _data;
    [SerializeField] private bool Player1;

    private void FixedUpdate() {
        //If it doesn't have a tank data
        if(_data == null) {
            _data = Player1 ? GameManager.Manager.Tankdata : GameManager.Manager.Tankdata2;
            return;
        }

        //Updates the health and stuff
        UpdateHUD();
    }

    //Updates the heads up the display for the player
    public void UpdateHUD() {
        _name.text = _data.TankName;
        _lives.text = "Lives: " + _data.Lives;
        _points.text = _data.Points.ToString();
        _seed.text = GameManager.Manager._currentSeed.ToString();
        _health.fillAmount = _data.CurrentHealth / _data.MaxHealth;
        _time.text = "Timer \n" + (GameManager.Manager.MatchTime - GameManager.Manager.Timer).ToString("0");
    }

}
