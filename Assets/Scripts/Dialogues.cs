using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;
using System.IO;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.Networking;
using System.Globalization;

public class Dialogues : MonoBehaviour
{
    private Story _currentStory;
    private TextAsset _inkJson;

    private GameObject _dialoguePanel;
    private GameObject _dialoguePanelCentral;
    private TextMeshProUGUI _dialogueText;
    private TextMeshProUGUI _onlyText;
    private TextMeshProUGUI _onlyTextCentral;
    private TextMeshProUGUI _nameText;

    private GameObject _disclaimer;

    [HideInInspector] public GameObject _choiceButtonsPanel;
    private GameObject _choiceButton;
    private List<TextMeshProUGUI> _choicesText = new();

    private GameObject _letter;
    private TextMeshProUGUI _letterText;

    private GameObject _book;
    private TextMeshProUGUI _bookText;

    private GameObject _leftCharacter;
    private GameObject _rightCharacter;
    private GameObject _addedCharacter;
    private GameObject _leftCloud;
    private GameObject _rightCloud;

    private string _storyId;
    private string _currentLeft = "";
    private string _currentRight = "";
    private Image _background;

    private GameObject _ending;

    private GameObject _wordLinkPanel;
    private TextMeshProUGUI _wordLinkPanelText;

    public static string _currentMusic = "";
    Dictionary<string, string> letterAndBookData = new Dictionary<string, string>()
    {
        {"письмо_Толстого", "18.06.1863.\r\nЯсная Поляна\r\n\r\nДорогой друг!\r\n\r\nОт всей души приветствую Вас и желаю доброго здравия.Прошло полтора года с нашей последней встречи, и мне, признаться, не хватает наших бесед.\r\n\r\nНо я хотел поведать Вам об одном занимательном событии.Не обессудьте, Милостивый Государь, но без Вашего личного присутствия не обойтись. Посему приглашаю пожаловать ко мне на следующей неделе в гости.\r\n\r\nЗа сим откланиваюсь и прошу прислать мне ответ, какого числа соблаговолите прибыть.\r\n\r\nКрепко жму руку и надеюсь на скорую встречу! \r\n\r\nЕБЖ \r\nГраф Толстой\r\n\r\n"},
        {"декабристы",  "Это было недавно, в царствование Александра II, в наше время — время цивилизации, прогресса, вопросов, возрождения России и т. д. и т. д.; в то время, когда победоносное русское войско возвращалось из сданного неприятелю Севастополя, когда вся Россия торжествовала уничтожение черноморского флота и белокаменная Москва встречала и поздравляла с этим счастливым событием остатки экипажей этого флота, подносила им добрую русскую чарку водки и, по доброму русскому обычаю, хлеб-соль и кланялась в ноги."},
        {"пастила", "Пастила яблочная\r\n\r\n1 меру  яблок испечь, протереть\r\n20 белков\r\n5 фунтов сахару \r\nCбить до белого, поставить в печь после хлебов\r\n\r"},
        {"пирог", "Пирог Анке\r\n\r\n1 фунт муки, 1/2 фунта масла, 1/4 фунта толченого сахара, 3 желтка, 1 рюмка воды \r\nМасло чтобы было прямо с погреба, похолоднее\r\nК нему начинка: 1/4 фунта масла растереть, 2 яйца тереть с маслом; толченого сахара 1/2 фунта, цедру с 2 лимонов растереть на терке и добавить сок с 3 лимонов\r\nКипятить до тех пор, пока станет густо, как мед\r\n\r"},
        {"письмо_Анны", "Если у вас, доктор, нет в виду ничего лучшего и если перспектива вечера у бедной \r\nбольной не слишком вас пугает, то я буду очень рада видеть вас нынче у себя \r\nмежду семью и десятью часами.\r\n\t\t\t\t\t\t\t\t\tАнна Шерер."}
    };
    
    public bool DialoguePlay {  get; private set; }
    [Inject]
    public void Construct(DialogueInstaller dialogueInstaller)
    {
        if (PlayerPrefs.HasKey("storyId"))
        {
            string storyId = PlayerPrefs.GetString("storyId");
            _storyId = storyId;
        }
        _inkJson = dialogueInstaller.inkJson;
        _dialoguePanel = dialogueInstaller.dialoguePanel;
        _dialoguePanelCentral = dialogueInstaller.dialoguePanelCentral;
        _dialogueText = dialogueInstaller.dialogueText;
        _onlyText = dialogueInstaller.onlyText;
        _onlyTextCentral = dialogueInstaller.onlyTextCentral;
        _nameText = dialogueInstaller.nameText;
        _disclaimer = dialogueInstaller.disclaimer;
        _choiceButtonsPanel = dialogueInstaller.choiceButtonsPanel;
        _choiceButton = dialogueInstaller.choiceButton;
        _letter = dialogueInstaller.letter;
        _letterText = dialogueInstaller.letterText;
        _book = dialogueInstaller.book;
        _bookText = dialogueInstaller.bookText;
        _leftCharacter = dialogueInstaller.leftCharacter;
        _rightCharacter = dialogueInstaller.rightCharacter;
        _addedCharacter = dialogueInstaller.addedCharacter;
        _leftCloud = dialogueInstaller.leftCloud;
        _rightCloud = dialogueInstaller.rightCloud;
        _background = dialogueInstaller.background;
        _ending = dialogueInstaller.ending;
        _wordLinkPanel = dialogueInstaller.wordLinkPanel;
        _wordLinkPanelText = dialogueInstaller.wordLinkPanelText;
    }
    private void Awake()
    {
        _currentStory = new Story(_inkJson.text);
        if (PlayerPrefs.HasKey("characterName"))
        {
            string characterName = PlayerPrefs.GetString("characterName");
            _currentStory.variablesState["yourName"] = characterName;
        }
    }
        void Start()
    {
        StartCoroutine(StartNovelRoutine());
    }
    private IEnumerator StartNovelRoutine()
    {
        StartCoroutine(FadeInObject(_disclaimer));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        StartCoroutine(FadeOutObject(_disclaimer));
        StartDialogue();
    }
    public void StartDialogue()
    {
        DialoguePlay = true;
        StartCoroutine(FadeOutWithCallback(_disclaimer, () =>
        {
            StartCoroutine(FadeInObject(_dialoguePanel));
            ContinueStory();
        }));
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
            targetColor.a += Time.deltaTime * 2;
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
            targetColor.a -= Time.deltaTime * 2;
            objectToFadeOut.color = targetColor;
            yield return null;
        }

        characterObject.SetActive(false);
    }

    private IEnumerator FadeOutWithCallback(GameObject panelObject, Action callback)
    {
        yield return FadeOutObject(panelObject);
        callback(); // Вызываем колбэк после завершения исчезновения
    }

    private bool IsCharacterVisible(GameObject characterObject)
    {
        return characterObject.activeSelf;
    }

    private IEnumerator HideObjectsAndWait()
    {
        yield return HideObjects();
        yield return StartCoroutine(FadeInObject(_ending));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        AudioManager.audio.Stop();
        if (_storyId == "3")
        {
            AudioManager.audio.Stop();
            SceneManager.LoadScene("TestingPart");
        }
        else if (_storyId == "1" || _storyId == "2")
        {

            if (PlayerPrefs.GetString("role") == "pupil")
            {
                yield return StartCoroutine(CountChosenCharacter());
                SceneManager.LoadScene("Menu");
            }
            else
                SceneManager.LoadScene("Menu2");
        }
    }

    private IEnumerator HideObjects()
    {
        StartCoroutine(FadeOutObject(_leftCharacter));
        StartCoroutine(FadeOutObject(_rightCharacter));
        StartCoroutine(FadeOutObject(_leftCloud));
        StartCoroutine(FadeOutObject(_rightCloud));
        yield return StartCoroutine(FadeOutObject(_dialoguePanel));
    }
    public void ContinueStory(bool choiceBefore = false)
    {
        if (_currentStory.canContinue)
        {

            ShowDialogue();
            ShowChoiceButtons(); 
        }
        else if (!choiceBefore)
        {
            ExitDialogue();
        }
    }
    public void ShowDialogue()
    {
        var music = (string)_currentStory.variablesState["music"];
        if (_currentMusic != music)
        {
            _currentMusic = music;
            AudioManager.audio.Stop();
            AudioClip newMusic = Resources.Load<AudioClip>($"Music\\{music}");
            AudioManager.audio.clip = newMusic;
            AudioManager.audio.loop = true;
            if (_currentMusic == "скрип" || _currentMusic == "стук" || _currentMusic == "рояль" || _currentMusic == "шинковка") AudioManager.audio.PlayOneShot(newMusic);
            else
                AudioManager.audio.Play();

        }

        if ((bool)_currentStory.variablesState["isLetter"] == true && (_storyId == "1" || _storyId == "2"))
        {
            ShowLetter();
        }
        else if ((bool)_currentStory.variablesState["isBook"] == true && _storyId == "1")
        {
            ShowBook();
        }
        else if (_storyId == "1")
        {
            StartCoroutine(FadeOutObject(_letter));
            StartCoroutine(FadeOutObject(_book));
            _dialoguePanel.SetActive(true);
        }
        else if (_storyId == "2")
        {
            StartCoroutine(FadeOutObject(_letter));
            _dialoguePanel.SetActive(true);
        }

        _dialogueText.text = _currentStory.Continue();
        _nameText.text = (string)_currentStory.variablesState["characterName"];

        // Для истории Доктора
        if ((string)_currentStory.variablesState["panel"] == "1")
        {
            _dialoguePanel.SetActive(false);
            _dialoguePanelCentral.SetActive(true);
            _onlyTextCentral.text = _dialogueText.text;
            _onlyTextCentral.color = new Color32(0, 0, 0, 255);
            _dialogueText.color = new Color32(0, 0, 0, 0);

        }
        else if (!IsCharacterVisible(_letter) && (!IsCharacterVisible(_book)))
        {
            _dialoguePanel.SetActive(true);
            _dialoguePanelCentral.SetActive(false);
            _onlyTextCentral.color = new Color32(0, 0, 0, 0);
            _dialogueText.color = new Color32(0, 0, 0, 255);
        }

        SetCharacterNameColor();

        Sprite image = Resources.Load<Sprite>($"Images\\Backgrounds\\{(string)_currentStory.variablesState["background"]}"); 
        if (image != null)
        {
            _background.sprite = image; 
        }
        else
        {
            Debug.LogError($"Image {(string)_currentStory.variablesState["background"]} not found!");
        }

        if ((string)_currentStory.variablesState["leftCharacter"] != "")
        {
            if ((!IsCharacterVisible(_leftCharacter) || (_currentLeft != (string)_currentStory.variablesState["leftCharacter"]) && (string)_currentStory.variablesState["leftCharacter"] != "quick"))
            {
                Image lcImage = _leftCharacter.GetComponent<Image>();
                Sprite lcCharSprite = Resources.Load<Sprite>($"Images\\Characters\\{(string)_currentStory.variablesState["leftCharacter"]}");
                lcImage.sprite = lcCharSprite;
                StartCoroutine(FadeInObject(_leftCharacter));
            }
            _currentLeft = (string)_currentStory.variablesState["leftCharacter"];
        }

        if ((string)_currentStory.variablesState["leftCharacter"] == "quick")
            _leftCharacter.SetActive(false);

        if ((string)_currentStory.variablesState["leftCharacter"] == "" && IsCharacterVisible(_leftCharacter))
            StartCoroutine(FadeOutObject(_leftCharacter));

        // Текст
        if ((string)_currentStory.variablesState["characterName"] == "")
        {
            _onlyText.text = _dialogueText.text;
            _onlyText.color = new Color32(0, 0, 0, 255);
            _dialogueText.color = new Color32(0, 0, 0, 0);
        }
        else
        {
            _onlyText.color = new Color32(0, 0, 0, 0);
            _dialogueText.color = new Color32(0, 0, 0, 255);
        }

        if ((string)_currentStory.variablesState["rightCharacter"] != "")
        {   
            if ((!IsCharacterVisible(_rightCharacter)  || _currentRight != (string)_currentStory.variablesState["rightCharacter"]) && (string)_currentStory.variablesState["rightCharacter"] != "quick") 
            {
                _currentRight = (string)_currentStory.variablesState["rightCharacter"];

                if ((string)_currentStory.variablesState["rightCharacter"] == "ЭленЗеркало")
                {
                    Image rcImage = _addedCharacter.GetComponent<Image>();
                    Sprite rcCharSprite = Resources.Load<Sprite>($"Images\\Characters\\{(string)_currentStory.variablesState["rightCharacter"]}");
                    rcImage.sprite = rcCharSprite;
                    _rightCharacter.SetActive(false);
                    StartCoroutine(FadeInObject(_addedCharacter));
                }
                else
                {
                    Image rcImage = _rightCharacter.GetComponent<Image>();
                    Sprite rcCharSprite = Resources.Load<Sprite>($"Images\\Characters\\{(string)_currentStory.variablesState["rightCharacter"]}");
                    rcImage.sprite = rcCharSprite;
                    _addedCharacter.SetActive(false);
                    StartCoroutine(FadeInObject(_rightCharacter));
                }

            }
        }
        
        if ((string)_currentStory.variablesState["rightCharacter"] == "quick")
            _rightCharacter.SetActive(false);

        if ((string)_currentStory.variablesState["rightCharacter"] == "" && IsCharacterVisible(_rightCharacter))
            StartCoroutine(FadeOutObject(_rightCharacter));

        if ((int)_currentStory.variablesState["cloudStatus"] == 1)
            _leftCloud.SetActive(true);
        else
            _leftCloud.SetActive(false);
        if ((int)_currentStory.variablesState["cloudStatus"] == 2)
            _rightCloud.SetActive(true);
        else
            _rightCloud.SetActive(false);

        if ((string) _currentStory.variablesState["wordLink"] != "")
        {
            _wordLinkPanelText.text = (string)_currentStory.variablesState["wordLink"];
            StartCoroutine(ShowWordLink());
        }
    }

    public IEnumerator ShowWordLink()
    {
        StartCoroutine(FadeInObject(_wordLinkPanel));
        yield return new WaitForSeconds(5);
        StartCoroutine(FadeOutObject(_wordLinkPanel));
    }

    private Color GetHexColor(string hexColor)
    {
        Color color;
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out color)) { return color; }
        else
        {
            Debug.LogError("Некорректный формат цвета HEX!");
            return Color.black;
        }
    }

    private void SetCharacterNameColor()
    {
        switch (_nameText.text)
        {
            case "Михаил":
                _nameText.color = GetHexColor("#5E902C");
                break;
            case "Андрей":
                _nameText.color = GetHexColor("#c72639");
                break;
            default:
                _nameText.color = Color.black;
                break;
        }
    }
    private void ShowChoiceButtons()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        _choiceButtonsPanel.SetActive(currentChoices.Count != 0);
        if (currentChoices.Count <= 0)
            return;
        foreach (Transform child in _choiceButtonsPanel.transform)
        {
            Destroy(child.gameObject);
        }
        _choicesText.Clear();
        for (int i = 0; i < currentChoices.Count; i++)
        {
            GameObject choice = Instantiate(_choiceButton);
            choice.GetComponent<ButtonAction>().index = i;
            choice.transform.SetParent(_choiceButtonsPanel.transform);

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = currentChoices[i].text;
            _choicesText.Add(choiceText);
        }
    }
    public void ChoiceButtonAction(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory(true);
    }
    private void ShowLetter()
    {
        _dialoguePanel.SetActive(false);
        string text = letterAndBookData[(string)_currentStory.variablesState["insertText"]];
        _letterText.text = text;
        
        StartCoroutine(FadeInObject(_letter));
    }

    private void ShowBook()
    {
        _dialoguePanel.SetActive(false);
        
        string filePath = $"Assets\\Dialogues\\AuthorStories\\{_currentStory.variablesState["textFile"]}.txt";

        string text = letterAndBookData[(string)_currentStory.variablesState["insertText"]];
        _bookText.text = text;

        StartCoroutine(FadeInObject(_book));
    }

    public void GoToMainPage()
    {
        DialoguePlay = false;
        if (PlayerPrefs.GetString("role") == "pupil")
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("Menu2");
    }
    private void ExitDialogue()
    {
        DialoguePlay = false;
        int storyId = Int32.Parse(PlayerPrefs.GetString("storyId"));
        StartCoroutine(HideObjectsAndWait());
    }

    private IEnumerator CountChosenCharacter()
    {
        string supabaseUrl = Data.supabaseUrl;
        string supabaseKey = Data.supabaseKey;

        string url = $"{supabaseUrl}/rest/v1/ChooseCharacter?select=*";

        UnityWebRequest requestGetCharacters = UnityWebRequest.Get(url);
        requestGetCharacters.SetRequestHeader("apikey", supabaseKey);

        yield return requestGetCharacters.SendWebRequest();

        int currentCharacterCount;
        JSONNode nodeToUpdate = new JSONObject();
        JSONNode foundNode = new JSONObject();

        if (requestGetCharacters.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + requestGetCharacters.error);
        }
        else
        {
            string responseText = requestGetCharacters.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode nodeCharacter in jsonNode.AsArray)
            {
                if (nodeCharacter["intcharacterid"] == PlayerPrefs.GetInt("characterId") && nodeCharacter["intgroupid"] == PlayerPrefs.GetInt("groupid"))
                {
                    currentCharacterCount = nodeCharacter["intchoosescount"];
                    nodeToUpdate["intchoosescount"] = currentCharacterCount + 1;
                    foundNode = nodeCharacter;
                    break;
                }
            }
        }

        string urlToUpdate = $"{supabaseUrl}/rest/v1/ChooseCharacter?intchoosecharacterid=eq.{foundNode["intchoosecharacterid"]}";

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(nodeToUpdate.ToString());

        UnityWebRequest requestToUpdate = new UnityWebRequest(urlToUpdate, "PATCH");
        requestToUpdate.uploadHandler = new UploadHandlerRaw(postData);
        requestToUpdate.downloadHandler = new DownloadHandlerBuffer();
        requestToUpdate.SetRequestHeader("Content-Type", "application/json");
        requestToUpdate.SetRequestHeader("apikey", supabaseKey);

        yield return requestToUpdate.SendWebRequest();

        if (requestToUpdate.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при отправке ответа: " + requestToUpdate.error);
        }
    }
}
