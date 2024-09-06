using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System.Text;
using Random = System.Random;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System;

namespace Novel
{
    public class Pupil : MonoBehaviour
    {
        public Text _nameGroup;
        public TMP_InputField _inputField;
        public GameObject _windowError;
        public Button _btn;


        [Inject]
        public void Construct(PupilInstaller pupilInstaller)
        {
            _nameGroup = pupilInstaller.group;
            _inputField = pupilInstaller._inputField;
            _windowError = pupilInstaller.windowError;
            _btn = pupilInstaller.btn;
        }
        [Inject]

        void Start()
        {
            int groupId = PlayerPrefs.GetInt("groupId");
            StartCoroutine(GetGroupName(groupId));
        }
        public int lastPupil;
        public IEnumerator AddNewPupil()
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Pupil?select=intpupilid&order=intpupilid.desc&limit=1";
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("apikey", Data.supabaseKey);
            string response;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                response = request.downloadHandler.text;
                JSONNode jsonNode = JSON.Parse(response);
                lastPupil = jsonNode[0]["intpupilid"].AsInt;
            }

            int groupId = PlayerPrefs.GetInt("groupId");
            string[] fio = _inputField.text.Split(' ');
            string surname = AddGroup.ConvertRussianLastNameToEnglish(fio[0]);
            string name = AddGroup.ConvertRussianLastNameToEnglish(fio[1]);
            string middleName;
            string middleNameRUS = "";
            string login;
            if (fio.Length == 4)
            {
                middleNameRUS = fio[2] + " " + fio[3];
                middleName = AddGroup.ConvertRussianLastNameToEnglish(middleNameRUS);
                string[] spl = middleName.Split();
                login = surname + name[0] + spl[0][0] + spl[1][0] + lastPupil;
            }
            else if (fio.Length == 2)
            {
                login = surname + name[0] + lastPupil;
            }

            else
            {
                middleNameRUS = fio[2];
                middleName = AddGroup.ConvertRussianLastNameToEnglish(middleNameRUS);
                login = surname + name[0] + middleName[0] + lastPupil;
            }
            url = $"{Data.supabaseUrl}/rest/v1/Pupil?select=intpupilid&order=intpupilid.desc&limit=1";
            request = UnityWebRequest.Get(url);
            request.SetRequestHeader("apikey", Data.supabaseKey);
            yield return request.SendWebRequest();
            Debug.Log(url);
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                response = request.downloadHandler.text;
                JSONNode jsonNode = JSON.Parse(response);
                lastPupil = jsonNode[0]["intpupilid"].AsInt + 1;
                string[] pass = AddGroup.GeneratePassword();
                List<string> fioList = new List<string>() { fio[0] };
                List<string> password = new List<string>() { pass[0] };
                List<string> loginList = new List<string>() { login };
                StartCoroutine(InsertDataCoroutine(fio[0], fio[1], middleNameRUS, login, pass[1], groupId));
                AddGroup.SaveTextToFile(fioList, loginList, password, PlayerPrefs.GetString("groupName"), _inputField.text + "_Логин и пароль ученика.txt ");
                SceneManager.LoadScene("Group");
            }
        }
        public void Input()
        {
            _windowError.SetActive(false);
            _btn.interactable = true;
        }
        public void NotEmpty()
        {
            if (_inputField.text.Length > 0)
            {
                _btn.interactable = true;
            }
            else _btn.interactable = false;
        }
        public void Save()
        {
            string[] fio = _inputField.text.Split(' ');
            try
            {
                if (fio.Length < 2 || fio.Length > 4)
                    throw new Exception();
            }
            catch
            {
                _windowError.SetActive(true);
                _btn.interactable = false;
                return;

            }
            StartCoroutine(AddNewPupil());
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
                        _nameGroup.text = node["txtgroupname"];
                        break;
                    }

                }
            }
        }
        private IEnumerator InsertDataCoroutine(string surname, string name, string middleName, string login, string password, int group)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Pupil";

            JSONNode node = new JSONObject();
            node["txtpupilsurname"] = surname;
            node["txtpupilname"] = name;
            node["txtpupilmiddlename"] = middleName;
            node["txtpupillogin"] = login;
            node["txtpupilpassword"] = password;
            node["intgroupid"] = group;

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

            UnityWebRequest request = new UnityWebRequest(url, "POST");
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
}
