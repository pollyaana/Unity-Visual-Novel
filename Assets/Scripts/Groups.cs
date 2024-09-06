using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class Groups : MonoBehaviour
{
    private int teacherId;
    private GameObject _groupPrefab;
    private Transform _contentTransform;

    [Inject]
    public void Construct(GroupsInstaller groupsInstaller)
    {
        _groupPrefab = groupsInstaller.groupPrefab;
        _contentTransform = groupsInstaller.contentTransform;
    }


    void Start()
    {
        teacherId = PlayerPrefs.GetInt("userid");
        PlayerPrefs.SetInt("teacherId", teacherId);
        teacherId = PlayerPrefs.GetInt("teacherId");
        StartCoroutine(ShowGroups());
    }


    public IEnumerator ShowGroups()
    {
        yield return StartCoroutine(GetGroups(teacherId));
    }
    public IEnumerator GetGroups(int id)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Group?select=*&order=txtgroupname.asc";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", Data.supabaseKey);
        List<string> groups = new List<string>();
        List<int> ids  = new List<int>();
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
                if (node["intteacherid"] == id)
                {
                    groups.Add(node["txtgroupname"]);
                    ids.Add(node["intgroupid"]);
                }

            }
        }
        float totalHeight = 0;
        float blockHeight = 0;
        for (int i = 0; i < groups.Count; i++)
        {
            GameObject group = Instantiate(_groupPrefab, _contentTransform);
            group.name = Convert.ToString(ids[i]);
            Text groupText = group.GetComponentInChildren<Text>();
            groupText.text = groups[i];
            blockHeight = group.GetComponent<RectTransform>().sizeDelta.y;
            totalHeight += blockHeight;
        }
        RectTransform contentRectTransform = _contentTransform.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight - blockHeight / 2 + blockHeight / 2);
    }
}
