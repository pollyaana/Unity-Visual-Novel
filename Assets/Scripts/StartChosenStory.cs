using Ink.Runtime;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class StartChoosenStory : MonoBehaviour
{
    private string supabaseUrl = Data.supabaseUrl;
    private string supabaseKey = Data.supabaseKey; 
    private Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        StartCoroutine(GetCharacterId());
    }
    public IEnumerator GetCharacterId()
    {
        string url = $"{supabaseUrl}/rest/v1/Character?select=*";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode node in jsonNode.AsArray)
            {
                characterDictionary.Add(node["txtcharacterrole"], node["intcharacterid"]);
            }
        }
    }

    public void StartGusarStory()
    {
        SaveCharacter("Гусар");
        PlayerPrefs.SetString("storyId", "1");
        PlayerPrefs.SetString("chosenCharacter", "Gusar");
        SceneManager.LoadScene("CharacterName");
    }
    public void StartGraphineStory()
    {
        SaveCharacter("Графиня");
        PlayerPrefs.SetString("storyId", "1");
        PlayerPrefs.SetString("chosenCharacter", "Graphine");
        SceneManager.LoadScene("CharacterName");
    }
    public void StartMaidStory()
    {
        SaveCharacter("Служанка");
        PlayerPrefs.SetString("storyId", "2");
        PlayerPrefs.SetString("chosenCharacter", "Maid");
        SceneManager.LoadScene("CharacterName");
    }
    public void StartDoctorStory()
    {
        SaveCharacter("Доктор");
        PlayerPrefs.SetString("storyId", "2");
        PlayerPrefs.SetString("chosenCharacter", "Doctor");


        SceneManager.LoadScene("CharacterSurname");
    }
    public void StartAndreaStory()
    {
        SaveCharacter("Андрей Болконский");
        PlayerPrefs.SetString("storyId", "3");
        PlayerPrefs.SetString("chosenCharacter", "Andre");

        AudioManager.audio.Stop();
        AudioClip newMusic = Resources.Load<AudioClip>($"Music\\таверна");
        AudioManager.audio.loop = true;
        Dialogues._currentMusic = "таверна";
        AudioManager.audio.clip = newMusic;
        AudioManager.audio.Play();

        SceneManager.LoadScene("AndreBolconskiy");
    }
    public void StartNatashaStory()
    {
        SaveCharacter("Наташа Ростова");
        PlayerPrefs.SetString("storyId", "3");
        PlayerPrefs.SetString("chosenCharacter", "Natasha");

        AudioManager.audio.Stop();
        AudioClip newMusic = Resources.Load<AudioClip>($"Music\\часы");
        AudioManager.audio.loop = true;
        Dialogues._currentMusic = "часы";
        AudioManager.audio.clip = newMusic;
        AudioManager.audio.Play();

        SceneManager.LoadScene("NatashaRostova");
    }
    public void SaveCharacter(string name)
    {
        bool flag = true;
        while (flag)
        {
            try

            {
                PlayerPrefs.SetInt("characterId", characterDictionary[name]);
                flag = false;
            }
            catch
            {
                flag = true;
            }
        }
    }


}
