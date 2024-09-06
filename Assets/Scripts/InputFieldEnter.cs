using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using System.Collections.Generic;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using System;
using TMPro;

public class InputFieldEnter : MonoBehaviour
{
    public TMP_InputField _inputField;
    private string link = Data.supabaseUrl;
    private string key = Data.supabaseKey;
    public  static int group;

    [Inject]
    public void Construct(InputInstaller inputInstaller)
    {
        _inputField = inputInstaller.pupils;
    }

    public void SubmitInput()
    {
        StartCoroutine(GetLastIntId());
    }
    IEnumerator GetLastIntId()
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Group?select=intgroupid&order=intgroupid.desc&limit=1";
        string pupils = _inputField.text;
        string[] pupil_str = pupils.Split('\n');
        List<string> surname = new List<string>();
        List<string> name = new List<string>();
        List<string> middleName = new List<string>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", Data.supabaseKey);
        string response = "";
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            response = request.downloadHandler.text;
            JSONNode jsonNode = JSON.Parse(response);
            int lastIntId = jsonNode[0]["intgroupid"].AsInt;
            Debug.Log(lastIntId);
            foreach (string str in pupil_str)
            {
                string[] fio = str.Split(' ');
                surname.Add(fio[0]);
                name.Add(fio[1]);
                middleName.Add(fio[2]);
                StartCoroutine(InsertDataCoroutine(fio[0], fio[1], fio[2], group));
            }

        }
    }
    private IEnumerator InsertDataCoroutine(string surname, string name, string middleName, int group)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Pupil";
        Debug.Log($"url: {url}");

        JSONNode node = new JSONObject();
        node["txtpupilsurname"] = surname;
        node["txtpupilname"] = name;
        node["txtpupilmiddlename"] = middleName;
        node["txtpupillogin"] = "test";
        node["txtpupilpassword"] = "test";
        node["intgroupid"] = group;

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", Data.supabaseKey);

        yield return request.SendWebRequest();

        Debug.Log(request.responseCode);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при отправке запроса: " + request.error);
        }
    }
}