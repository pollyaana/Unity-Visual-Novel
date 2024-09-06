using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using Zenject;

public class DeleteGroup : MonoBehaviour
{
    private GameObject _window;
    [Inject]
    public void Construct(MenuGroupInstaller menuGroupInstaller)
    {
        _window = menuGroupInstaller.window;
    }

    public void DeleteOrNot()
    {
        _window.SetActive(true);
    }
    public void NotDelete()
    {
        _window.SetActive(false);
    }
    public void Delete()
    {
        int groupId = PlayerPrefs.GetInt("groupId");
        StartCoroutine(GetPupilsId(groupId));
    }
    public IEnumerator GetPupilsId(int id)
    {
        List<int> pupilsId = new List<int>();
        string url = $"{Data.supabaseUrl}/rest/v1/Pupil?select=*";
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
                    pupilsId.Add(node["intpupilid"]);
                }

            }
            foreach (var pupil in pupilsId)
            {
                url = $"{Data.supabaseUrl}/rest/v1/PupilAnswer?intpupilid=eq.{pupil}";

                request = new UnityWebRequest(url, "DELETE");
                request.SetRequestHeader("apikey", Data.supabaseKey);

                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Ошибка при отправке запроса: " + request.error);
                }

                url = $"{Data.supabaseUrl}/rest/v1/Pupil?intgroupid=eq.{id}";

                request = new UnityWebRequest(url, "DELETE");
                request.SetRequestHeader("apikey", Data.supabaseKey);

                yield return request.SendWebRequest();
            }
            url = $"{Data.supabaseUrl}/rest/v1/ChooseCharacter?intgroupid=eq.{id}";

            request = new UnityWebRequest(url, "DELETE");
            request.SetRequestHeader("apikey", Data.supabaseKey);

            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при отправке запроса: " + request.error);
            }
            url = $"{Data.supabaseUrl}/rest/v1/Group?intgroupid=eq.{id}";

            request = new UnityWebRequest(url, "DELETE");
            request.SetRequestHeader("apikey", Data.supabaseKey);

            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при отправке запроса: " + request.error);
            }
            SceneManager.LoadScene("Groups");
        }
    }
}
