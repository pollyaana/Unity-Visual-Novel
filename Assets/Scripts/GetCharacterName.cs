using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GetCharacterName : MonoBehaviour
{
    private TMP_InputField _inputCharacter;
    private Image _background;
    private string _storyId;
    private string _chosenCharacter;

    
    [Inject]
    public void Construct(CharacterNameInstaller characterInstaller)
    {
        _inputCharacter = characterInstaller.inputCharacter;
        _background = characterInstaller.background;
    }
    public void Awake()
    {
        if (PlayerPrefs.HasKey("storyId"))
        {
            _storyId = PlayerPrefs.GetString("storyId");
        }
        if (PlayerPrefs.HasKey("chosenCharacter"))
        {
            _chosenCharacter = PlayerPrefs.GetString("chosenCharacter");
        }

        Sprite image = null;
        if (_storyId == "1" && _chosenCharacter == "Gusar")
            image = Resources.Load<Sprite>($"Images\\Backgrounds\\usadba");
        else if (_storyId == "1" && _chosenCharacter == "Graphine")
            image = Resources.Load<Sprite>($"Images\\Backgrounds\\usadba");
        else if (_storyId == "2" && _chosenCharacter == "Maid")
            image = Resources.Load<Sprite>($"Images\\Backgrounds\\покои2");
        else if (_storyId == "2" && _chosenCharacter == "Doctor")
            image = Resources.Load<Sprite>($"Images\\Backgrounds\\фамилия");

        if (image != null)
        {
            _background.sprite = image; // меняем изображение компонента Image
        }
        else
        {
            Debug.LogError($"Image not found!");
        }
    }
    public void InputCharacterName()
    {
        string characterName;
        characterName = _inputCharacter.text;
        PlayerPrefs.SetString("characterName", characterName);
        if (_storyId == "1" && _chosenCharacter == "Gusar")
        {
            if (characterName != "")
            {
                AudioManager.audio.Stop();
                AudioClip newMusic = Resources.Load<AudioClip>($"Music\\дом");
                AudioManager.audio.loop = true;
                Dialogues._currentMusic = "дом";
                AudioManager.audio.clip = newMusic;
                AudioManager.audio.Play();
                SceneManager.LoadScene("AuthorGusarScene");
            }
        }
        else if (_storyId == "1" && _chosenCharacter == "Graphine")
        {
            if (characterName != "")
            {
                AudioManager.audio.Stop();
                AudioClip newMusic = Resources.Load<AudioClip>($"Music\\дом");
                AudioManager.audio.loop = true;
                Dialogues._currentMusic = "дом";
                AudioManager.audio.clip = newMusic;
                AudioManager.audio.Play();
                SceneManager.LoadScene("AuthorGraphineScene");
            }
        }
        else if (_storyId == "2" && _chosenCharacter == "Maid")
        {
            if (characterName != "")
            {
                AudioManager.audio.Stop();
                Dialogues._currentMusic = "";
                SceneManager.LoadScene("PrologueMaidScene");
            }
        }
        else if (_storyId == "2" && _chosenCharacter == "Doctor")
        {
            if (characterName != "")
            {
                AudioManager.audio.Stop();
                AudioClip newMusic = Resources.Load<AudioClip>($"Music\\часы");
                AudioManager.audio.loop = true;
                Dialogues._currentMusic = "часы";
                AudioManager.audio.clip = newMusic;
                AudioManager.audio.Play();
                SceneManager.LoadScene("PrologueDoctorScene");
            }
        }
    }   
}
