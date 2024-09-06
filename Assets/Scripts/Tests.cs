using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.InputSystem;

public class Tests : MonoBehaviour
{
    private string supabaseUrl = Data.supabaseUrl;
    private string supabaseKey = Data.supabaseKey; 
    public string keyAnswerTable = "KeyAnswer";
    public string characterTable = "Character";
    public string questionTable = "Question";
    public string pupilAnswerTable = "PupilAnswer";
    public string groupTable = "Group";
    public string assessmentOfKnowledgeTable = "AssessmentOfKnowledge";
    private GameObject _questionPanel;
    private GameObject _answerPanel;
    private TextMeshProUGUI _questionText;
    private TextMeshProUGUI _answerText;
    List<string> questionsTextList = new List<string>();
    List<string> answersTextList = new List<string>();
    List<string> pointsList = new List<string>();
    private int questionIndex = 0; // переменная для хранения индекса текущего вопроса
    public double points = 0;
    public double pastPoints = 0;
    private int pastPointsId = 3;
    public string currentPoints = "0";
    public string currentAnswer;
    private int answerPanelYPosition = 60;
    public List<int> questionKeyAnswersId = new List<int>();
    public List<int> answersIdList = new List<int>();
    public List<string> keysList = new List<string>();
    private int characterId = 0;
    private int groupId = 0;
    private int pupilId = 0;
    private int teacherId = 0;
    private Image _background;
    private string imageName = "";
    //private bool updateAnswers = false;
    private bool isEmpty = true;
    public bool TestPlay { get; private set; }

    [Inject]
    public void Construct(TestInstaller testInstaller)
    {
        _background = testInstaller.background;
        _questionPanel = testInstaller.questionPanel;
        _answerPanel = testInstaller.answerPanel;
        _questionText = testInstaller.questionText;
        _answerText = testInstaller.answerText;
    }
    void Start()
    {
        characterId = PlayerPrefs.GetInt("characterId");
        pupilId = PlayerPrefs.GetInt("userid");
        groupId = PlayerPrefs.GetInt("groupid");
        if (characterId == 12) imageName = "tavern";
        else if (characterId == 14) imageName = "natasha_house";
        Sprite image = Resources.Load<Sprite>($"Images\\Backgrounds\\{imageName}");
        _background.sprite = image;
        StartCoroutine(GetAndContinueQuestions());
    }

    public IEnumerator GetKeyAnswers()
    {
        string url = $"{supabaseUrl}/rest/v1/{keyAnswerTable}?select=*";
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
                if (questionKeyAnswersId.Contains(node["intkeyanswerid"])) 
                    keysList.Add(node["txtkeywords"]);
            }

            yield return keysList;
        }
    }

    public IEnumerator GetPastPoints()
    {
        string url = $"{supabaseUrl}/rest/v1/{assessmentOfKnowledgeTable}?select=*";
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
                if (node["intcharacterid"] == characterId && node["intgroupid"] == groupId && node["intpupilid"] == pupilId)
                {
                    pastPoints = Double.Parse(node["txtpoints"]);
                    pastPointsId = int.Parse(node["intassessmentofknowledgeid"]);
                    isEmpty = false;
                }
            }
            yield return pastPoints;
        }
    }

    public IEnumerator GetTeacherId()
    {
        string url = $"{supabaseUrl}/rest/v1/{groupTable}?select=*";
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
                if (node["intgroupid"] == groupId) teacherId = node["intteacherid"];
            }
            yield return teacherId;
        }
    }

    public IEnumerator GetAnswerId()
    {
        string url = $"{supabaseUrl}/rest/v1/{pupilAnswerTable}?select=*";
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
                if (node["intcharacterid"] == characterId && node["intpupilid"] == pupilId)
                {
                    answersIdList.Add(node["intpupilanswerid"]);
                }
            }
            yield return answersIdList;
        }
    }

    public IEnumerator GetQuestions()
    {
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
                    Debug.Log($"вопрос {node["txtquestiontext"]}");
                    questionsTextList.Add(node["txtquestiontext"]);
                    questionKeyAnswersId.Add(node["intkeyanswerid"]);
                }
            }
            PlayerPrefs.SetInt("questionCount", questionsTextList.Count);
            yield return questionsTextList;
        }
    }

    public void CheckAnswer(string[] keysList, string currentAnswer)
    {
        int count = 0;
        foreach (string keyWord in keysList)
        {
            if (IsSimilarKeyword(keyWord.ToLower(), currentAnswer.ToLower().Split(' ')))
            {
                count++;
            }
        }
        if (count >= 0.15 * keysList.Length && count < 0.3 * keysList.Length)
        {
            currentPoints = "0.5";
            points += 0.5;
        }
        else if (count >= 0.3 * keysList.Length)
        {
            currentPoints = "1";
            points += 1;
        }
        else
        {
            currentPoints = "0";
        }
        pointsList.Add(currentPoints);
    }

    private bool IsSimilarKeyword(string keyword, string[] currentAnswer)
    {
        foreach (string answer in currentAnswer) {
            if (keyword.Remove(keyword.Length - 1) == answer) return true; 
            if ((IsNumber(keyword) && IsNumber(answer))|| currentAnswer.Contains("Расположите")) 
            {
                if (keyword.Contains(answer))
                {
                    return true;
                }
            }
            // Очищаем ключевое слово и ответ от символов, отличных от букв и цифр
            string cleanKeyword = CleanString(keyword);
            string cleanAnswer = CleanString(answer);

            int errorCount = 0;
            if (!IsNumber(keyword) && !IsNumber(answer))
            {
                for (int i = 0; i < Math.Min(cleanKeyword.Length, cleanAnswer.Length); i++)
                {
                    if (cleanKeyword[i] != cleanAnswer[i]) errorCount++;
                    if (errorCount > 1) break;
                    if (i == Math.Min(cleanKeyword.Length, cleanAnswer.Length) - 1 && errorCount<=1) return true;
                }
            }
        }
        // Если дошли до конца и ошибок было не больше 1, возвращаем true
        return false;
    }

    private bool IsNumber(string str)
    {
        int number;
        return int.TryParse(str, out number); // Попытка преобразования строки в число
    }

    private string CleanString(string str)
    {
        return new string(str.Where(c => char.IsLetterOrDigit(c)).ToArray());
    }


    public IEnumerator SendAnswers(string currentAnswer, string currentPoints, int pupilId, int questionId, int stotyId)
    {
        string url = $"{supabaseUrl}/rest/v1/{pupilAnswerTable}";
        //Debug.Log($"url: {url}");
        JSONNode node = new JSONObject();
        node["txtpupilanswertext"] = currentAnswer;
        node["txtpointsforanswer"] = currentPoints;
        node["intpupilid"] = pupilId;
        node["intquestionid"] = questionId;
        node["intcharacterid"] = characterId;
        node["intstoryid"] = stotyId;
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);

        yield return request.SendWebRequest();
        Debug.Log(request.responseCode);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при отправке ответа: " + request.error);
        }
    }

    public IEnumerator EditAnswerById(int answerId, string updatedAnswer, string updatedPoints)
    {
        string url = $"{supabaseUrl}/rest/v1/{pupilAnswerTable}?intpupilanswerid=eq.{answerId}";
        JSONNode node = new JSONObject();
        node["txtpupilanswertext"] = updatedAnswer;
        node["txtpointsforanswer"] = updatedPoints;

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при редактировании ответа: " + request.error);
        }
    }

    public IEnumerator EditPointsInAssessmentOfKnowledge(int pointsId, string updatedPoints)
    {
        string url = $"{supabaseUrl}/rest/v1/{assessmentOfKnowledgeTable}?intassessmentofknowledgeid=eq.{pointsId}";
        Debug.Log($"url: {url}");
        Debug.Log($"Редактируем баллы: {pointsId}");
        Debug.Log($"новые баллы: {updatedPoints}");

        JSONNode node = new JSONObject();
        node["txtpoints"] = updatedPoints;

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);

        yield return request.SendWebRequest();
        Debug.Log(request.responseCode);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при редактировании ответа: " + request.error);
        }
    }

    public IEnumerator SendPoints(string pointsSum, int teacherId, int pupilId, int groupId)
    {
        string url = $"{supabaseUrl}/rest/v1/{assessmentOfKnowledgeTable}";
        //Debug.Log($"url: {url}");
        JSONNode node = new JSONObject();
        node["txtpoints"] = pointsSum.ToString();
        node["intteacherid"] = teacherId;
        node["intpupilid"] = pupilId;
        node["intgroupid"] = groupId;
        node["intcharacterid"] = characterId;
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(node.ToString());

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);

        yield return request.SendWebRequest();
        Debug.Log(request.responseCode);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при отправке ответа: " + request.error);
        }
    }

    private IEnumerator GetAndContinueQuestions()
    {
        yield return StartCoroutine(GetQuestions());
        yield return StartCoroutine(GetKeyAnswers());
        yield return StartCoroutine(GetTeacherId());
        ShowNextQuestion();
    }

    private IEnumerator CheckPoints(double curPoints)
    {
        yield return GetPastPoints();
        Debug.Log($"pupilId {pupilId}");
        if (isEmpty)
        {
            
            for (int i = 0; i < answersTextList.Count; i++)
            {
                Debug.Log($"answer {answersTextList[i]}");
                Debug.Log($"points {pointsList[i]}");
                Debug.Log($"id {questionKeyAnswersId[i]}");
                
                yield return StartCoroutine(SendAnswers(answersTextList[i], pointsList[i], pupilId, questionKeyAnswersId[i], 3));
            }
            yield return StartCoroutine(SendPoints(points.ToString(), teacherId, pupilId, groupId));
        }
        else
        {
            yield return StartCoroutine(GetAnswerId());
            if (curPoints > pastPoints)
            {
                for (int i = 0; i < answersTextList.Count; i++)
                {
                    yield return StartCoroutine(EditAnswerById(answersIdList[i], answersTextList[i], pointsList[i]));
                }
                yield return StartCoroutine(EditPointsInAssessmentOfKnowledge(pastPointsId, points.ToString()));
            }
        }
    }

    public void OnNextButtonClick()
    {
        currentAnswer = _answerText.text;
        _answerPanel.GetComponentInChildren<TMP_InputField>().text = "";
        CheckAnswer(keysList[questionIndex-1].Split(','), currentAnswer.Remove(currentAnswer.Length - 1));
        answersTextList.Add(currentAnswer);
        int questionCount = PlayerPrefs.GetInt("questionCount");
        if (questionIndex != questionCount)
        {
            ShowNextQuestion(); // Вызываем метод продолжения диалога
        }
        else
        {
            PlayerPrefs.SetString("points", points.ToString());
            Button nextButton = GetComponent<Button>();
            nextButton.interactable = false;
            StartCoroutine(CheckPointsAndLoadScene());
        }
    }

    private void ShowNextQuestion()
    {
        if (questionIndex < questionsTextList.Count)
        {
            _questionText.text = questionsTextList[questionIndex].Replace('n', '\n');
            Vector2 answerPosition = _answerPanel.GetComponent<RectTransform>().anchoredPosition;
            if (_questionText.text.Length > 150)
            {
                float textHeight = _questionText.renderedHeight; // Получаем высоту текста
                float blockHeight = _questionPanel.GetComponent<RectTransform>().rect.height; // Получаем высоту блока
                float overflowAmount = textHeight - blockHeight;
                if (textHeight > blockHeight)
                {
                    _answerPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(answerPosition.x, answerPanelYPosition - overflowAmount*11);
                }
            }
            else { _answerPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(answerPosition.x, answerPanelYPosition); }
            questionIndex++;
        }
    }

    private IEnumerator CheckPointsAndLoadScene()
    {
        if (PlayerPrefs.GetString("role") == "pupil")
        {
            yield return StartCoroutine(CheckPoints(points));
        }
        //Debug.Log($"updateAnswers: {updateAnswers}");
        SceneManager.LoadScene("ShowPoints");
    }

}
