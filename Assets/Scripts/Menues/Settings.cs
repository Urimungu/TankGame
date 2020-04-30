using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //Options
    [SerializeField] private Toggle _dailyRoom;
    [SerializeField] private InputField _seedNumber;
    [SerializeField] private Slider _effectVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Toggle _muteSound;

    //Variables
    private int _preSeed = 0;

    //Loads up the pre-set settings
    public void LoadSettings() {
        var levelManager = GameManager.Manager.gameObject.GetComponent<LevelManager>();

        //Changes the seed number
        if(levelManager.CustomSeed != 0) {
            _seedNumber.text = levelManager.CustomSeed.ToString();
            _preSeed = levelManager.CustomSeed;
        } else {
            _seedNumber.text = "";
        }
        _dailyRoom.isOn = levelManager.RoomOfTheDay;

        //Volume
        _effectVolume.value = GameManager.Manager.EffectsVolume;
        _musicVolume.value = GameManager.Manager.MusicVolume;
        _muteSound.isOn = GameManager.Manager.Mute;
    }

    //Saves the settings on the game manager
    public void SaveSettings() {
        var levelManager = GameManager.Manager.gameObject.GetComponent<LevelManager>();

        //Seed Number
        levelManager.CustomSeed = int.Parse(_seedNumber.text);
        levelManager.RoomOfTheDay = _dailyRoom.isOn;

        //Sound
        GameManager.Manager.EffectsVolume = _effectVolume.value;
        GameManager.Manager.MusicVolume = _musicVolume.value;
        GameManager.Manager.Mute = _muteSound.isOn;
    }

    //Updates music Automatically
    public void MusicIsChanging() {
        GameManager.Manager.MusicVolume = _musicVolume.value;
    }

    //If activated deactives the seed number
    public void RoomOfTheDayChange() {
        _seedNumber.interactable = !_dailyRoom.isOn;

        if(_seedNumber.interactable) {
            _seedNumber.text = _preSeed.ToString();
        } else {
            _seedNumber.text = "";
        }
    }

    //Changes the temporary seed value
    public void SeedChange() {
        _preSeed = int.Parse(_seedNumber.text);
    }

    //Disables the volumes if the game is on mute
    public void OnMute() {
        _effectVolume.interactable = !_muteSound.isOn;
        _musicVolume.interactable = !_muteSound.isOn;
        GameManager.Manager.Mute = _muteSound.isOn;
    }
}
