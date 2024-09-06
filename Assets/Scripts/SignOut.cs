using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SignOut : MonoBehaviour
{
    private GameObject _dialogueExitPanel;

    [Inject]
    public void Construct(SettingsInstaller settingsInstaller)
    {
        _dialogueExitPanel = settingsInstaller.dialogueExitPanel;
    }

    public void ShowDialogueExitWindow()
    {
        _dialogueExitPanel.SetActive(true);
    }

    public void CloseDialogueExitWindow()
    {
        _dialogueExitPanel.SetActive(false);
    }
    public void SingOut()
    {
        try
        {
            AudioManager.audio.Stop();
        }
        catch { }
        CloseDialogueExitWindow();
        PlayerPrefs.DeleteKey("userid");
        PlayerPrefs.SetInt("sound",0);
        PlayerPrefs.Save();
        Sound.pos = 0;
        PlayerPrefs.DeleteKey("groupid");
        PlayerPrefs.DeleteKey("role");
        SceneManager.LoadScene("Autorization");
    }
}
