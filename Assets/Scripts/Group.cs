using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class Group : MonoBehaviour
{
    public Text _nameGroup;
    GameObject _groupPrefab;
    Transform _contentTransform;
    private List<int> pupilsId;
    [Inject]
    public void Construct(MenuGroupInstaller menuGroupInstaller)
    {
        _nameGroup = menuGroupInstaller.nameGroup;
        _groupPrefab = menuGroupInstaller.groupPrefab;
        _contentTransform = menuGroupInstaller.contentTransform;        
    }
    public int groupId;
    void Start()
    {
        groupId = PlayerPrefs.GetInt("groupId");
        StartCoroutine(GetGroupName(groupId));
        StartCoroutine(ShowPupils(groupId));
    }

    public IEnumerator GetPupilsId(int id)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Group?select=*";
        Debug.Log("before request");

        UnityWebRequest request = UnityWebRequest.Get(url);
        Debug.Log("before header");
        request.SetRequestHeader("apikey", Data.supabaseKey);
        Debug.Log("before return");
        yield return request.SendWebRequest();
        Debug.Log("after return");
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
                if (node["intgroupid"] == 1)
                {
                    Debug.Log(node["txtgroupname"]);
                    break;
                }

            }
        }
    }
  
    public IEnumerator GetGroupName(int id)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Group?select=*";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", Data.supabaseKey);
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
                if (node["intgroupid"] == id)
                {
                    _nameGroup.text  =  node["txtgroupname"];
                    PlayerPrefs.SetString("groupName", node["txtgroupname"]);
                    PlayerPrefs.Save();
                    break;
                }

            }
        }
    }

    public IEnumerator ShowPupils(int id)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Pupil?select=*&order=txtpupilsurname.asc";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", Data.supabaseKey);
        List<string> pupils = new List<string>();
        List<int> ids = new List<int>();
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
                if (node["intgroupid"] == id)
                {

                    pupils.Add(node["txtpupilsurname"] + " " + node["txtpupilname"] + " " + node["txtpupilmiddlename"]);
                    ids.Add(node["intpupilid"]);
                }

            }
        }
        float totalHeight = 0;
        float blockHeight = 0;
        for (int i = 0; i < pupils.Count; i++)
        {
            GameObject pupil = Instantiate(_groupPrefab, _contentTransform);
            pupil.name = Convert.ToString(ids[i]);
            Text groupText = pupil.GetComponentInChildren<Text>();
            groupText.text = pupils[i];
            blockHeight = pupil.GetComponent<RectTransform>().sizeDelta.y;
            totalHeight += blockHeight;
        }

        RectTransform contentRectTransform = _contentTransform.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }
}
