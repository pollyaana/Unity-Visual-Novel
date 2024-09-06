using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System.Text;
using Random = System.Random;
using System.IO;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using TMPro;

namespace Novel
{
    public class AddGroup : MonoBehaviour
    {
        public TMP_InputField _group;
        public TMP_InputField _inputField;
        public GameObject _windowError;
        public Button _btn;

        [Inject]
        public void Construct(InputInstaller inputInstaller)
        {
            _group = inputInstaller.group;
            _inputField = inputInstaller.pupils;
            _windowError = inputInstaller.windowError;
            _btn = inputInstaller.btn;
        }
        
        public void AddRecord()
        {
            string group = _group.text;
            int teacherId = PlayerPrefs.GetInt("userid");
            try
            {
                string pupils = _inputField.text;
                string[] pupil_str = pupils.Split('\n');
                foreach (string str in pupil_str)
                {
                    string[] fio = str.Split(' ');
                    if (fio.Length < 2 || fio.Length > 4)
                        throw new Exception();
                }
            }
            catch
            {
                _windowError.SetActive(true);
                _btn.interactable = false;
                return;

            }
            StartCoroutine(InsertDataCoroutine(group, teacherId));
        }
        public void Input()
        {
            _windowError.SetActive(false);
            _btn.interactable = true;
        }
        public void NotEmpty()
        {
            if (_group.text.Length > 0)
            {
                _btn.interactable = true;
            }
            else _btn.interactable = false;
        }

        public IEnumerator InsertCharacter(int teacher)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Character?select=*";
            List<int> charactersIds = new List<int>();
            var request = UnityWebRequest.Get(url);
            request.SetRequestHeader("apikey", Data.supabaseKey);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при получении данных: " + request.error);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                JSONNode jsonNode = JSON.Parse(responseText);

                foreach (JSONNode vertix in jsonNode.AsArray)
                {
                    if (vertix["intstoryid"] == 1 || vertix["intstoryid"] == 2)
                    {
                        charactersIds.Add(vertix["intcharacterid"]);
                    }

                }
            }
                url = $"{Data.supabaseUrl}/rest/v1/Group?select=intgroupid&order=intgroupid.desc&limit=1";
                request = UnityWebRequest.Get(url);
                request.SetRequestHeader("apikey", Data.supabaseKey);
                yield return request.SendWebRequest();
                string response;
                int lastIntId = 0;
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                response = request.downloadHandler.text;
                JSONNode jsonNode = JSON.Parse(response);
                lastIntId = jsonNode[0]["intgroupid"].AsInt;
            }
            url = $"{Data.supabaseUrl}/rest/v1/ChooseCharacter";
            foreach (var el in charactersIds)
            {
                {
                    JSONNode node = new JSONObject();
                    node["intgroupid"] = lastIntId;
                    node["intchoosescount"] = 0;
                    node["intteacherid"] = teacher;
                    node["intcharacterid"] = el;
                    byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

                    request = new UnityWebRequest(url, "POST");
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
            SceneManager.LoadScene("Groups");
        }

        public static string[] GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!?@#$%^&*()-+={}/\\";

            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            int length = 8;
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }
            string password = sb.ToString();
            string[] pass = new string[2];
            pass[0] = password;
            pass[1] = HashPassword(password);
            return pass;
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public int lastPupil;
        public IEnumerator GeneratePupilData(string group)
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
            url = $"{Data.supabaseUrl}/rest/v1/Group?select=intgroupid&order=intgroupid.desc&limit=1";
            string pupils = _inputField.text;
            string[] pupil_str = pupils.Split('\n');
            string surname;
            string name;
            string middleName;
            List<string> logins = new();
            List<string> passwords = new();
            request = UnityWebRequest.Get(url);
            request.SetRequestHeader("apikey", Data.supabaseKey);
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
                PlayerPrefs.SetInt("group", lastIntId);
                Random random = new Random();
                List<string> fios = new();
                foreach (string str in pupil_str)
                {
                        fios.Add(str);
                        string[] fio = str.Split(' ');
                        string login = "";
                        surname = ConvertRussianLastNameToEnglish(fio[0]);
                        name = ConvertRussianLastNameToEnglish(fio[1]);
                        string surnameRUS = fio[0];
                        string nameRUS = fio[1];
                        string middleNameRUS = "";
                        if (fio.Length == 4)
                        {
                            middleNameRUS = fio[2] + " " + fio[3];
                            middleName = ConvertRussianLastNameToEnglish(middleNameRUS);
                            string[] spl = middleName.Split();
                            login = surname + name[0] + spl[0][0] + spl[1][0] + lastPupil;
                        }
                     else if(fio.Length == 2)
                        {
                            login = surname + name[0]  + lastPupil;
                        }

                    else
                    {
                        middleNameRUS = fio[2];
                        middleName = ConvertRussianLastNameToEnglish(middleNameRUS);
                        login = surname + name[0] + middleName[0] + lastPupil;
                    }
                        lastPupil += 1;
                        logins.Add(login);
                        string[] pass = GeneratePassword();
                        string password = pass[0];
                        string cript = pass[1];
                        passwords.Add(password);
                        StartCoroutine(InsertDataCoroutine(surnameRUS,nameRUS,middleNameRUS, login, cript, lastIntId));


                }
                SaveTextToFile(fios, logins, passwords, group, group + "_Логины и пароли учеников.txt");
            }

        }


        public static void SaveTextToFile(List<string> fios, List<string> logins, List<string> passwords, string group, string fileName)
        {

            string downloadsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/Downloads/";
            string filePath = downloadsPath + fileName;
            List<string> studentData = new List<string>();
            studentData.Add(group);
            for (int i = 0; i < logins.Count; i++)
            {
                studentData.Add("ФИО: " + fios[i] + "  Логин: " + logins[i] + " Пароль: " + passwords[i]);
            }

            File.WriteAllLines(filePath, studentData);

        }
        public static string ConvertRussianLastNameToEnglish(string russianLastName)
        {
            string lowerCaseLastName = russianLastName.ToLower();
            lowerCaseLastName = lowerCaseLastName
                                .Replace("а", "a")
                                .Replace("б", "b")
                                .Replace("в", "v")
                                .Replace("г", "g")
                                .Replace("д", "d")
                                .Replace("е", "e")
                                .Replace("ж", "zh")
                                .Replace("з", "z")
                                .Replace("и", "i")
                                .Replace("й", "y")
                                .Replace("к", "k")
                                .Replace("л", "l")
                                .Replace("м", "m")
                                .Replace("н", "n")
                                .Replace("о", "o")
                                .Replace("п", "p")
                                .Replace("р", "r")
                                .Replace("с", "s")
                                .Replace("т", "t")
                                .Replace("у", "u")
                                .Replace("ф", "f")
                                .Replace("х", "h")
                                .Replace("ц", "c")
                                .Replace("ч", "ch")
                                .Replace("ш", "sh")
                                .Replace("щ", "sch")
                                .Replace("ъ", "")
                                .Replace("ы", "y")
                                .Replace("ь", "")
                                .Replace("э", "a")
                                .Replace("ю", "yu")
                                .Replace("я", "ya");

            return lowerCaseLastName;
        }
        private IEnumerator InsertDataCoroutine(string surname, string name, string middleName, string login, string password, int group)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Pupil";
            Debug.Log(surname);
            Debug.Log(name);
            Debug.Log(middleName);
            JSONNode node = new JSONObject();
            node["txtpupilsurname"] = surname;
            node["txtpupilname"] = name;
            node["txtpupilmiddlename"] = middleName;
            node["txtpupillogin"] =  login;
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

        private IEnumerator InsertDataCoroutine(string group, int id)
        {
            string url = $"{Data.supabaseUrl}/rest/v1/Group";

            JSONNode node = new JSONObject();
            node["txtgroupname"] = group;
            node["intteacherid"] = id;

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
            StartCoroutine(InsertCharacter(id));
            StartCoroutine(GeneratePupilData(group));
        }
    }
}