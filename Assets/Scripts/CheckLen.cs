using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CheckLen : MonoBehaviour
{
    private TMP_InputField _group;
    private GameObject _window;
    private Button _button;
    [Inject]
    public void Construct(InputInstaller inputInstaller)
    {
        _group = inputInstaller.group;
        _window = inputInstaller.window;
        _button = inputInstaller.btn;

    }
    public void ChangeText()
    {
        var group = _group.text;
        if (group.Length > 20)
        {
            _window.SetActive(true);
           _button.interactable = false;
        }
            
        
        else
        {
            _window.SetActive(false);
            _button.interactable = true;
        }
        
    }
}
