using Novel;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class Authorization : MonoBehaviour
{
    private string supabaseUrl = "https://vtdyecqfthdiegrqssbw.supabase.co";
    private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZ0ZHllY3FmdGhkaWVncnFzc2J3Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcxMzE2ODI5OCwiZXhwIjoyMDI4NzQ0Mjk4fQ.Uovfe2NpVq1eg3xzQmUR111ImRJMZWIzABFyQwqeKxY";

    private Button _submitButton;
    private TMP_InputField _inputLogin;
    private TMP_InputField _inputPassword;
    private GameObject _errorMessage;

    private bool isPupil;
    private bool isTeacher;

    [Inject]
    public void Construct(AuthorizationInstaller authorizationInstaller)
    {
        _submitButton = authorizationInstaller.submitButton;
        _inputLogin = authorizationInstaller.inputLogin;
        _inputPassword = authorizationInstaller.inputPassword;
        _errorMessage = authorizationInstaller.errorMessage;
    }

    private IEnumerator FadeInObject(GameObject characterObject)
    {
        Image objectToFadeIn = characterObject.GetComponent<Image>();
        if (objectToFadeIn == null)
        {
            Debug.LogError("The component not found on GameObject: " + characterObject.name);
            yield break;
        }

        Color targetColor = objectToFadeIn.color;
        targetColor.a = 0f;

        objectToFadeIn.color = targetColor;
        characterObject.SetActive(true);

        while (objectToFadeIn.color.a < 1f)
        {
            targetColor.a += Time.deltaTime;
            objectToFadeIn.color = targetColor;
            yield return null;
        }
    }

    private IEnumerator FadeOutObject(GameObject characterObject)
    {
        Image objectToFadeOut = characterObject.GetComponent<Image>();
        if (objectToFadeOut == null)
        {
            Debug.LogError("The component not found on GameObject: " + characterObject.name);
            yield break;
        }

        Color targetColor = objectToFadeOut.color;
        targetColor.a = 1f;

        objectToFadeOut.color = targetColor;

        while (objectToFadeOut.color.a > 0f)
        {
            targetColor.a -= Time.deltaTime;
            objectToFadeOut.color = targetColor;
            yield return null;
        }

        characterObject.SetActive(false);
    }

    private IEnumerator ShowErrorMessage()
    {
        StartCoroutine(FadeInObject(_errorMessage));
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeOutObject(_errorMessage));
    }

    public void AuthenticateUser()
    {
        string login = _inputLogin.text;
        string password = _inputPassword.text;
        Debug.Log(password);

        StartCoroutine(IsPupilOrTeacher(login, password)); 
    }
    static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2) return true;
        if (b1 == null || b2 == null) return false;
        if (b1.Length != b2.Length) return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i]) return false;
        }
        return true;
    }
    static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        byte[] buffer4;
        byte[] src = Convert.FromBase64String(hashedPassword);
        byte[] dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);
        byte[] buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
        {
            buffer4 = bytes.GetBytes(0x20);
        }
        return ByteArraysEqual(buffer3, buffer4);
    }

    public IEnumerator IsPupilOrTeacher(string login, string password)
    {
        JSONNode userNode;
        string urlPupil = $"{supabaseUrl}/rest/v1/Pupil?select=intpupilid, txtpupillogin,txtpupilpassword, intgroupid";

        UnityWebRequest requestPupil = UnityWebRequest.Get(urlPupil);
        requestPupil.SetRequestHeader("apikey", supabaseKey);

        yield return requestPupil.SendWebRequest();

        if (requestPupil.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(requestPupil.error);
        }
        else
        {
            string responseText = requestPupil.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);
            foreach (JSONNode node in jsonNode.AsArray)
            {
                if (login == (string)node["txtpupillogin"] && VerifyHashedPassword(node["txtpupilpassword"],password))
                {
                    isPupil = true;
                    userNode = node;
                    PlayerPrefs.SetInt("userid", userNode["intpupilid"]);
                    PlayerPrefs.SetInt("groupid", userNode["intgroupid"]);
                    PlayerPrefs.SetString("role", "pupil");
                    break;
                }
                else
                    isPupil = false;
            }
            yield return null;
        }

        string urlTeacher = $"{supabaseUrl}/rest/v1/Teacher?select=intteacherid,txtteacherlogin,txtteacherpassword";

        UnityWebRequest requestTeacher = UnityWebRequest.Get(urlTeacher);
        requestTeacher.SetRequestHeader("apikey", supabaseKey);

        yield return requestTeacher.SendWebRequest();

        if (requestTeacher.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(requestTeacher.error);
        }
        else
        {
            string responseText = requestTeacher.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode node in jsonNode.AsArray)
            {
                if (login == (string)node["txtteacherlogin"] && VerifyHashedPassword(node["txtteacherpassword"],password))
                {
                    isTeacher = true;
                    userNode = node;
                    Debug.Log(userNode["intteacherid"]);
                    PlayerPrefs.SetInt("userid", userNode["intteacherid"]);
                    PlayerPrefs.SetInt("groupid", 0);
                    PlayerPrefs.SetString("role", "teacher");
                    break;
                }
                else
                    isTeacher = false;
            }

            yield return null;
        }

        if (isPupil)
        {
            isTeacher = false;
            SceneManager.LoadScene("Menu");
        }

        else if (isTeacher)
        {
            isPupil = false;
            SceneManager.LoadScene("Menu2");
        }

        else if (!isPupil && !isTeacher)
            StartCoroutine(ShowErrorMessage());

    }
}
