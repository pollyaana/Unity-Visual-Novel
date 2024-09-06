using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class ButtonClick : MonoBehaviour
{
    public SpriteState sprState = new SpriteState();
    public void OpenAuthorNovel()
    {
        SceneManager.LoadScene("AuthorChooseCharacter");
    }
    public void OpenPrologue()
    {
        SceneManager.LoadScene("PrologueChooseCharacter");
    }

    public void OpenKnowledgeAssesment()
    {
        SceneManager.LoadScene("KnowledgeAssesmentChooseCharacter");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void OpenStatistics()
    {
        SceneManager.LoadScene("Statistics");
    }
    public void OpenGroups()
    {
        SceneManager.LoadScene("Groups");
    }
    public void OpenAddGroup()
    {
        SceneManager.LoadScene("AddGroup");
    }
    public void OpenMenu()
    {
        try
        {
            if (AudioManager.audio.clip != Resources.Load<AudioClip>($"Music\\background")) AudioManager.audio.Stop();
        }
        catch { }
       
        if (PlayerPrefs.GetString("role") == "pupil")
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("Menu2");
    }
    public void OpenGroup()
    {
        try
        {
            string groupId = this.gameObject.name;
            PlayerPrefs.SetInt("groupId", Convert.ToInt32(groupId));
            PlayerPrefs.Save();
        }
        catch
        {

        }
        finally
        {
            SceneManager.LoadScene("Group");
        }

    }
    public void OpenPupil()
    {
        string pupilId = this.gameObject.name;
        PlayerPrefs.SetInt("pupilId", Convert.ToInt32(pupilId));
        PlayerPrefs.Save();
        SceneManager.LoadScene("Pupil");
    }
    public void OpenAddPupil()
    {
        SceneManager.LoadScene("AddPupil");
    }
}