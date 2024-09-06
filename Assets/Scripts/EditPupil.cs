using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;
using SimpleJSON;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using System.Collections.Generic;
using TMPro;

namespace Novel
{
    public class EditPupil : MonoBehaviour
    {
        public Text _nameGroupPupil;
        public static TMP_InputField _inputFieldPupil;
        private GameObject _window;
        public GameObject _windowError;
        public Button _btn;

        [Inject]
        public void Construct(EditPupilInstaller editPupilInstaller)
        {
            _nameGroupPupil = editPupilInstaller.group;
            _inputFieldPupil = editPupilInstaller.inputField;
            _windowError = editPupilInstaller.windowError;
            _btn = editPupilInstaller.btn;
        }
        void Start()
        {
            _nameGroupPupil.text = PlayerPrefs.GetString("groupName");
            int pupil = PlayerPrefs.GetInt("pupilId");
            StartCoroutine(GetFIO(pupil));
        }
        public IEnumerator GetFIO(int id)
        {
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
                    if (node["intpupilid"] == id)
                    {
                        PlayerPrefs.SetString("login", node["txtpupillogin"]);
                        _inputFieldPupil.text = node["txtpupilsurname"] + " " + node["txtpupilname"] + " " + node["txtpupilmiddlename"];
                        break;
                    }

                }
            }
        }

        public void ChangeFIO()
        {
            int id = PlayerPrefs.GetInt("pupilId");
            string[] fio = _inputFieldPupil.text.Split(' ');
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
            StartCoroutine(InsertDataCoroutine(fio, id));
        }
        public void Input()
        {
            _windowError.SetActive(false);
            _btn.interactable = true;
        }
        public void NotEmpty()
        {
            if (_inputFieldPupil.text.Length > 0)
            {
                _btn.interactable = true;
            }
            else _btn.interactable = false;
        }
        public int lastPupil;
        private IEnumerator InsertDataCoroutine(string[] fio, int pupil)
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

            url = $"{Data.supabaseUrl}/rest/v1/Pupil?intpupilid=eq.{pupil}";
            JSONNode node = new JSONObject();
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
            node["txtpupilsurname"] = fio[0];
            node["txtpupilname"] = fio[1];
            node["txtpupilmiddlename"] = middleNameRUS;

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

            request = new UnityWebRequest(url, "PATCH");
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
        public void DeleteOrNot()
        {
            _window.SetActive(true);
        }
        public void NotDelete()
        {
            _window.SetActive(false);
        }
        public void DeletePupil()
        {
            int id = PlayerPrefs.GetInt("pupilId");
            string[] fio = _inputFieldPupil.text.Split(' ');
            StartCoroutine(DeleteAnswersCoroutine(id));
            StartCoroutine(DeletePupilCoroutine(id));
            SceneManager.LoadScene("Group");
        }
        private IEnumerator DeleteAnswersCoroutine(int pupil)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/PupilAnswer?intpupilid=eq.{pupil}";
            UnityWebRequest request = new UnityWebRequest(url, "DELETE");
            request.SetRequestHeader("apikey", Data.supabaseKey);

            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при отправке запроса: " + request.error);
            }
        }
        private IEnumerator DeletePupilCoroutine(int pupil)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Pupil?intpupilid=eq.{pupil}";
            UnityWebRequest request = new UnityWebRequest(url, "DELETE");
            request.SetRequestHeader("apikey", Data.supabaseKey);

            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при отправке запроса: " + request.error);
            }
        }
    }
}

