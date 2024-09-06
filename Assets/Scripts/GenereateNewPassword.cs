using Novel;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenereateNewPassword : MonoBehaviour
{
    public void NewPassword()
    {
        string[] pass = AddGroup.GeneratePassword();
        StartCoroutine(InsertPassword(pass[1], PlayerPrefs.GetInt("pupilId")));
        List<string> fio = new List<string>() { EditPupil._inputFieldPupil.text };
        List<string> password = new List<string>() { pass[0] };
        List<string> login = new List<string>() { PlayerPrefs.GetString("login") };
        AddGroup.SaveTextToFile(fio, login, password, PlayerPrefs.GetString("groupName"), fio[0] + "_Логин и пароль ученика " +".txt");

    }
    private IEnumerator InsertPassword(string password, int id)
    {
        string url = $"{Data.supabaseUrl}/rest/v1/Pupil?intpupilid=eq.{id}";
        Debug.Log(url);
        JSONNode node = new JSONObject();
        node["txtpupilpassword"] = password;
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", Data.supabaseKey);

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при отправке запроса: " + request.error);
        }
    }
}
