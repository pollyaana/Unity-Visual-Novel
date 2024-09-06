using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogueInstaller : MonoInstaller
{
    public TextAsset inkJson;

    public GameObject dialoguePanel;
    public GameObject dialoguePanelCentral;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI onlyText;
    public TextMeshProUGUI onlyTextCentral;
    public TextMeshProUGUI nameText;

    public GameObject disclaimer;

    public GameObject choiceButtonsPanel;
    public GameObject choiceButton;

    public GameObject letter;
    public TextMeshProUGUI letterText;

    public GameObject book;
    public TextMeshProUGUI bookText;

    public GameObject leftCharacter;
    public GameObject rightCharacter;
    public GameObject addedCharacter;
    public GameObject leftCloud;
    public GameObject rightCloud;

    public Image background;

    public GameObject ending;

    public GameObject wordLinkPanel;
    public TextMeshProUGUI wordLinkPanelText;
}
