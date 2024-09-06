using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AuthorizationInstaller : MonoInstaller
{
    public Button submitButton;
    public TMP_InputField inputLogin;
    public TMP_InputField inputPassword;
    public GameObject errorMessage;
}
