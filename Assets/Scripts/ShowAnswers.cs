using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.Networking;
using SimpleJSON;


public class ShowAnswers : MonoBehaviour
{
    private string supabaseUrl = Data.supabaseUrl;
    private string supabaseKey = Data.supabaseKey; 
    public string answerTable = "PupilAnswer";
    public string questionTable = "Question";
    public string characterTable = "Character";
    public string pupilTable = "Pupil";
    private TextMeshProUGUI _FIOText;
    private TextMeshProUGUI _characterText;
    public int questionCount;
    List<string> questionsTextList = new List<string>();
    List<int> questionsIdList = new List<int>();
    List<string> answersTextList = new List<string>();
    List<string> pointsTextList = new List<string>();
    private int characterId = 1;
    private int chosenPupilId = 0;
    private string characterName;
    private string pupilFIO;
    public GameObject _questionPrefab;
    public GameObject _answerPrefab;
    public Transform _contentTransform;
    string[] chosenPupil;

    [Inject]
    public void Construct(ShowAnswersInstaller showAnswersInstaller)
    {
        _FIOText = showAnswersInstaller.FIOText;
        _characterText = showAnswersInstaller.characterText;
        _questionPrefab= showAnswersInstaller.questionPrefab;
        _answerPrefab = showAnswersInstaller.answerPrefab;
        _contentTransform= showAnswersInstaller.contentTransform;
    }


    void Start()
    {
        chosenPupil = PlayerPrefs.GetString("chosenPupilForAnswers").TrimStart().Split(' ');
        characterId = PlayerPrefs.GetInt("chosenCharacterId");
        chosenPupilId = PlayerPrefs.GetInt("chosenPupilId");
        pupilFIO = $"{chosenPupil[0]} {chosenPupil[1][0]}. {chosenPupil[2][0]}.";
        characterName = PlayerPrefs.GetString("chosenCharacterForAnswers");
        PlayerPrefs.SetString("chosenCharacterName", "Andre");
        StartCoroutine(GetQuestionsAndInsertData());
    }


    public IEnumerator GetQuestionsAndInsertData()
    {
        yield return StartCoroutine(GetQuestions());
        yield return StartCoroutine(GetAnswers());
        Debug.Log($"всего вопросов - {questionsTextList.Count}");
        Debug.Log($"всего ответов - {answersTextList.Count}");
        _FIOText.text += $" {pupilFIO}";
        _characterText.text += $" {characterName}";
        float totalHeight = 0;
        //float textHeight = 0;
        //float blockHeight = 0;
        try
        {
            for (int i = 0; i < answersTextList.Count; i++)
            {
                GameObject questionObject = Instantiate(_questionPrefab, _contentTransform);
                TextMeshProUGUI questionText = questionObject.GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log(questionText.text);
                questionText.text = $"{i+1}. {questionsTextList[i].Replace('n','\n')} ({pointsTextList[i]})";
                RectTransform questionRectTransform = questionObject.GetComponent<RectTransform>();
                float questionTextHeight = questionText.preferredHeight;
                questionRectTransform.sizeDelta = new Vector2(questionRectTransform.sizeDelta.x, questionTextHeight);
                float questionBlockHeight = questionObject.GetComponent<RectTransform>().sizeDelta.y;
                totalHeight += questionTextHeight;

                GameObject answerObject = Instantiate(_answerPrefab, _contentTransform);
                TextMeshProUGUI answerText = answerObject.GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log(answerText.text);
                answerText.text = answersTextList[i];
                RectTransform answerRectTransform = answerObject.GetComponent<RectTransform>();
                float answerTextHeight = answerText.preferredHeight;
                answerRectTransform.sizeDelta = new Vector2(answerRectTransform.sizeDelta.x, answerTextHeight);
                float answerBlockHeight = questionObject.GetComponent<RectTransform>().sizeDelta.y;
                totalHeight += answerTextHeight;
            }
            RectTransform contentRectTransform = _contentTransform.GetComponent<RectTransform>();
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
        }
        catch { }
        
    }

    public void SetText(GameObject newObject, string textToFill)
    {
        TextMeshPro answerTextMeshPro = newObject.GetComponentInChildren<TextMeshPro>();
        if (answerTextMeshPro != null)
        {
            answerTextMeshPro.text = textToFill;
        }
    }

    public IEnumerator GetQuestions()
    {
        Debug.Log($"Проверка1 - {characterId}");
        string url = $"{supabaseUrl}/rest/v1/{questionTable}?select=*";
        //Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);

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
                if (node["intcharacterid"] == characterId)
                {
                    questionsTextList.Add(node["txtquestiontext"]);
                    questionsIdList.Add(node["intquestionid"]);
                }
            }
            PlayerPrefs.SetInt("questionCount", questionsTextList.Count);
            yield return questionsTextList;
        }
    }

    public IEnumerator GetAnswers()
    {
        string url = $"{supabaseUrl}/rest/v1/{answerTable}?select=*";
        Debug.Log(url);
        Debug.Log($"Проверка2 - {chosenPupilId}");
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);

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
                if (node["intpupilid"] == chosenPupilId)
                {
                    foreach(int el in questionsIdList)
                    { 
                        if (node["intquestionid"] == el)
                        {
                            Debug.Log($"Ответ на вопрос номер {el}: {node["txtpupilanswertext"]}");
                            answersTextList.Add(node["txtpupilanswertext"]);
                            pointsTextList.Add(node["txtpointsforanswer"]);
                        }
                    }   
                }
            }
            yield return answersTextList;
        }
    }
}
