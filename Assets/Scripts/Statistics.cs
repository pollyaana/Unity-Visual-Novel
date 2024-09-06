using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Zenject;

public class Statistics : MonoBehaviour
{
    private TMP_Dropdown _groupDropdown;
    private TMP_Dropdown _storyDropdown;
    private TMP_Dropdown _knowledgeDropdown;
    private string supabaseUrl = Data.supabaseUrl;
    private string supabaseKey = Data.supabaseKey; 
    private int currentGroupId;
    private UnityEngine.UI.Image barPrefab;
    private Transform barsParent;
    private GameObject axisY;
    private GameObject axisX;
    private TextMeshProUGUI maxCut;
    private TextMeshProUGUI middleCut;
    private TextMeshProUGUI firstCharacter;
    private TextMeshProUGUI secondCharacter;
    private GameObject backgroundPanel;
    private GameObject tablePupils;
    private GameObject prefPupil;
    private Transform _tableTransform;

    private List<int> groupIds = new List<int>();
    private List<string> labelList = new List<string>();
    List<string> charactersOptions = new List<string>();
    private int questionCount = 0;

    [Inject]
    public void Construct(StatsInstaller statsInstaller)
    {
        _groupDropdown = statsInstaller.groupDropdown;
        _storyDropdown = statsInstaller.storyDropdown;
        _knowledgeDropdown = statsInstaller.knowledgeDropdown;
        barPrefab = statsInstaller.barPrefab;
        barsParent = statsInstaller.barsParent;
        axisX = statsInstaller.axisX;
        axisY = statsInstaller.axisY;
        maxCut = statsInstaller.maxCut;
        middleCut = statsInstaller.middleCut;
        firstCharacter = statsInstaller.firstCharacter;
        secondCharacter = statsInstaller.secondCharacter;
        backgroundPanel = statsInstaller.backgroundPanel;
        tablePupils = statsInstaller.tablePupils;
        prefPupil = statsInstaller.prefPupil;
        _tableTransform = statsInstaller.tableTransform;

        _storyDropdown.value = 0;
        _groupDropdown.value = 0;
    }
    void Start()
    {
        StartCoroutine(GetGroupsData());
        StartCoroutine(GetCharactersData());
        StartCoroutine(GetGrapicsData(1));

        _groupDropdown.onValueChanged.AddListener(delegate
        {
            GroupValueChanged(_groupDropdown);
        });

        _storyDropdown.onValueChanged.AddListener(delegate
        {
            StoryValueChanged(_storyDropdown);
        });

        _knowledgeDropdown.onValueChanged.AddListener(delegate
        {
            KnowledgeValueChanged(_knowledgeDropdown);
        });
    }

    private IEnumerator GetGroupsData()
    {
        string url = $"{supabaseUrl}/rest/v1/Group?intteacherid=eq.{PlayerPrefs.GetInt("userid")}";
        UnityWebRequest requestGetCharacters = UnityWebRequest.Get(url);
        requestGetCharacters.SetRequestHeader("apikey", supabaseKey);

        yield return requestGetCharacters.SendWebRequest();

        try {
            _groupDropdown.ClearOptions();
        }
        catch { }

        List<string> options = new List<string>();

        if (requestGetCharacters.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + requestGetCharacters.error);
        }
        else
        {
            string responseText = requestGetCharacters.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode node in jsonNode.AsArray)
            {
                options.Add(node["txtgroupname"]);
                groupIds.Add(node["intgroupid"]);
            }
        }
        _groupDropdown.AddOptions(options);
    }
    private IEnumerator GetCharactersData()
    {
        string url = $"{supabaseUrl}/rest/v1/Character?intstoryid=eq.3";
        UnityWebRequest requestGetCharacters = UnityWebRequest.Get(url);
        requestGetCharacters.SetRequestHeader("apikey", supabaseKey);

        yield return requestGetCharacters.SendWebRequest();

        _knowledgeDropdown.ClearOptions();

        if (requestGetCharacters.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + requestGetCharacters.error);
        }
        else
        {
            string responseText = requestGetCharacters.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode node in jsonNode.AsArray)
            {
                charactersOptions.Add(node["txtcharacterrole"]);
            }

        }
        _knowledgeDropdown.AddOptions(charactersOptions);
    }

    public void StoryValueChanged(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                _knowledgeDropdown.gameObject.SetActive(false);
                barsParent.gameObject.SetActive(true);
                axisX.SetActive(true);
                axisY.SetActive(true);
                backgroundPanel.SetActive(true);
                tablePupils.SetActive(false);
                StartCoroutine(GetGrapicsData(1));
                break;
            case 1:
                _knowledgeDropdown.gameObject.SetActive(false);
                barsParent.gameObject.SetActive(true);
                axisX.SetActive(true);
                axisY.SetActive(true);
                backgroundPanel.SetActive(true);
                tablePupils.SetActive(false);
                StartCoroutine(GetGrapicsData(2));
                break;
            case 2:
                _knowledgeDropdown.gameObject.SetActive(true);
                barsParent.gameObject.SetActive(false);
                axisX.SetActive(false);
                axisY.SetActive(false);
                backgroundPanel.SetActive(false);
                tablePupils.SetActive(true);
                StartCoroutine(GetTableData(_knowledgeDropdown.value));
                break;
            default:
                Debug.Log("Invalid option selected");
                break;
        }
    }

    public void GroupValueChanged(TMP_Dropdown dropdown)
    {
        currentGroupId = groupIds[_groupDropdown.value];
        StartCoroutine(GetGrapicsData(_storyDropdown.value + 1));
        StartCoroutine(GetTableData(0));
    }

    public void KnowledgeValueChanged(TMP_Dropdown dropdown)
    {
        foreach (Transform child in _tableTransform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(GetTableData(_knowledgeDropdown.value));
    }

    private IEnumerator GetGrapicsData(int storyId)
    {
        string urlStoryCharacters = $"{supabaseUrl}/rest/v1/Character?intstoryid=eq.{storyId}";
        UnityWebRequest requestStoryCharacters = UnityWebRequest.Get(urlStoryCharacters);
        requestStoryCharacters.SetRequestHeader("apikey", supabaseKey);

        yield return requestStoryCharacters.SendWebRequest();

        List<string> storyCharacters = new List<string>();

        if (requestStoryCharacters.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + requestStoryCharacters.error);
        }
        else
        {
            string responseText = requestStoryCharacters.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);

            foreach (JSONNode node in jsonNode.AsArray)
            {
                storyCharacters.Add(node["intcharacterid"]);
            }
        }

        StartCoroutine(GetCharactersForLabels(storyCharacters));
        try {
            string urlGroupChoices = $"{supabaseUrl}/rest/v1/ChooseCharacter?intgroupid=eq.{groupIds[_groupDropdown.value]}";
            UnityWebRequest requestGroupChoices = UnityWebRequest.Get(urlGroupChoices);
            requestGroupChoices.SetRequestHeader("apikey", supabaseKey);

            yield return requestGroupChoices.SendWebRequest();
            List<int> groupChoices = new List<int>();
            List<string> characterNames = new List<string>();
            List<JSONNode> nodes = new List<JSONNode>();

            if (requestStoryCharacters.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при получении данных: " + requestGroupChoices.error);
            }
            else
            {
                string responseText = requestGroupChoices.downloadHandler.text;
                var jsonNode = JSON.Parse(responseText);

                foreach (JSONNode node in jsonNode.AsArray)
                {
                    nodes.Add(node);
                }
            }
            foreach (var id in storyCharacters)
            {
                foreach (var node in nodes)
                {
                    if ((string)id == (string)node["intcharacterid"])
                    {
                        groupChoices.Add(node["intchoosescount"]);
                    }
                }
            }
            foreach (Transform child in barsParent)
            {
                Destroy(child.gameObject);
            }

            if (groupChoices != null && groupChoices.Count > 0)
            {
                double maximum = Math.Ceiling(Max(groupChoices) / 5) * 5;
                int height = 0;
                if ((int)maximum != 0)
                {
                    height = (int)barsParent.GetComponent<RectTransform>().rect.height / (int)maximum;
                    maxCut.text = maximum.ToString();
                    //middleCut.text = Math.Round(maximum / 2).ToString();
                    middleCut.text = Math.Round(maximum / 2, 1).ToString();
                }
                else
                {
                    maxCut.text = "5";
                    middleCut.text = "2,5";
                }
                
                List<Color> colors = new List<Color>() { HexToColor("#234A29"), HexToColor("#2C7337") };

                double startCoordinates = -0.6;
                for (int i = 0; i < groupChoices.Count; i++)
                {
                    UnityEngine.UI.Image bar = Instantiate(barPrefab, barsParent);
                    bar.rectTransform.sizeDelta = new Vector2(150, groupChoices[i] * height);
                    bar.transform.localPosition = new Vector3((float)startCoordinates * 350, -barsParent.GetComponent<RectTransform>().rect.height / 2, 0);
                    startCoordinates += 1;
                    bar.GetComponentInChildren<TextMeshProUGUI>().text = groupChoices[i].ToString();
                    bar.color = colors[i];
                }
            }
        }
        finally { }
    }

    private Color HexToColor(string hex)
    {
        Color color = new Color();
        UnityEngine.ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
    public int SaveMax(List<int> groupChoices)
    {
        bool flag = true;
        int max = 0; ;
        while (flag)
        {
            try

            {

                max = groupChoices[0]; // Переменная для хранения максимального значения, начально инициализируем первым элементом массива
                flag = false;
            }
            catch
            {
                flag = true;
            }
        }
        return max;
    }
    double Max(List<int> groupChoices)
    {

        int max = SaveMax(groupChoices);

        for (int i = 1; i < groupChoices.Count; i++)
        {
            if (groupChoices[i] > max)
            {
                max = groupChoices[i];
            }
        }
        return max;
    }

    private IEnumerator GetCharactersForLabels(List<string> storyCharacters)
    {
        foreach (Transform child in _tableTransform)
        {
            Destroy(child.gameObject);
        }
        labelList = new List<string>();
        foreach (var character in storyCharacters)
        {
            string url = $"{supabaseUrl}/rest/v1/Character?intcharacterid=eq.{character}";
            UnityWebRequest requestGetCharacters = UnityWebRequest.Get(url);
            requestGetCharacters.SetRequestHeader("apikey", supabaseKey);

            yield return requestGetCharacters.SendWebRequest();

            if (requestGetCharacters.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при получении данных: " + requestGetCharacters.error);
            }
            else
            {
                string responseText = requestGetCharacters.downloadHandler.text;
                var jsonNode = JSON.Parse(responseText);

                foreach (JSONNode node in jsonNode.AsArray)
                {
                    labelList.Add(node["txtcharacterrole"]);
                }
            }
        }
        firstCharacter.text = labelList[0];
        secondCharacter.text = labelList[1];
        yield return labelList;
    }

    public IEnumerator CountQuestions(int characterId)
    {
        questionCount = 0;
        string url = $"{supabaseUrl}/rest/v1/Question?select=*";
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
                    questionCount += 1;
                }
            }
            PlayerPrefs.SetInt("questionCount", questionCount);
            yield return questionCount;
        }
    }

    private IEnumerator GetTableData(int characterId)
    {
        PlayerPrefs.SetString("chosenCharacterForAnswers", charactersOptions[characterId]);
        if (characterId == 0) characterId = 12;
        if (characterId == 1) characterId = 14;

        yield return CountQuestions(characterId);
        
        PlayerPrefs.SetInt("chosenCharacterId", characterId);
        PlayerPrefs.SetInt("chosenGroupId", groupIds[_groupDropdown.value]);
        string urlPupilsList = $"{supabaseUrl}/rest/v1/AssessmentOfKnowledge?intgroupid=eq.{groupIds[_groupDropdown.value]}";
        UnityWebRequest requestPupilList = UnityWebRequest.Get(urlPupilsList);
        requestPupilList.SetRequestHeader("apikey", supabaseKey);

        yield return requestPupilList.SendWebRequest();

        List<string> pupilsList = new List<string>();
        List<int> pupilsId = new List<int>();
        List<string> pupilsPoints = new List<string>();
        if (requestPupilList.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка при получении данных: " + requestPupilList.error);
        }
        else
        {
            string responseText = requestPupilList.downloadHandler.text;
            var jsonNode = JSON.Parse(responseText);
            foreach (JSONNode node in jsonNode.AsArray)
            {
                if (node["intcharacterid"] == characterId)
                {
                    pupilsId.Add(node["intpupilid"]);
                    pupilsPoints.Add(node["txtpoints"]);

                }
            }
        }
        foreach (var pupilId in pupilsId)
        {
            string urlGetPoints = $"{supabaseUrl}/rest/v1/Pupil?intpupilid=eq.{pupilId}";
            UnityWebRequest requestGetPoints = UnityWebRequest.Get(urlGetPoints);
            requestGetPoints.SetRequestHeader("apikey", supabaseKey);

            yield return requestGetPoints.SendWebRequest();

            if (requestGetPoints.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка при получении данных: " + requestGetPoints.error);
            }
            else
            {
                string responseText = requestGetPoints.downloadHandler.text;
                var jsonNode = JSON.Parse(responseText);

                foreach (JSONNode node in jsonNode.AsArray)
                {
                    string surname = node["txtpupilsurname"];
                    string name = node["txtpupilname"];
                    string middlename = node["txtpupilmiddlename"];
                    pupilsList.Add($"  {surname} {name} {middlename}");
                }
            }
        }
        float totalHeight = 0;
        float blockHeight = 0;
        for (int i = 0; i < pupilsList.Count; i++)
        {
            int buttonNum = i;
            GameObject pupilObject = Instantiate(prefPupil, _tableTransform);
            UnityEngine.UI.Button button = pupilObject.GetComponentInChildren<UnityEngine.UI.Button>();
            TextMeshProUGUI pupilFIO = button.GetComponentInChildren<TextMeshProUGUI>();
            GameObject pointsObject = pupilObject.GetComponentInChildren<Transform>().Find("PointsPanel").gameObject;
            TextMeshProUGUI pupilPointsPanel = pointsObject.GetComponentInChildren<TextMeshProUGUI>();
            pupilFIO.text = pupilsList[i];
            pupilPointsPanel.text = $"{pupilsPoints[i]}/{questionCount}";
            blockHeight = pupilObject.GetComponent<RectTransform>().sizeDelta.y;
            Debug.Log($"blockHeight {blockHeight}");
            totalHeight += blockHeight;
            button.onClick.AddListener(() => ShowPupilAnswers(buttonNum, pupilsId, pupilsList));
        }
        RectTransform contentRectTransform = _tableTransform.GetComponent<RectTransform>();
        Debug.Log(contentRectTransform.sizeDelta.y); Debug.Log(totalHeight); Debug.Log(contentRectTransform.sizeDelta.y - totalHeight);
        Debug.Log(contentRectTransform.anchoredPosition.y);
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }


    public void ShowPupilAnswers(int buttonNum, List<int> pupilsId, List<string> pupilsList)
    {
        PlayerPrefs.SetInt("chosenPupilId", pupilsId[buttonNum]);
        //TextMeshProUGUI chosenPupilFIO = GetComponentInChildren<TextMeshProUGUI>();
        PlayerPrefs.SetString("chosenPupilForAnswers", pupilsList[buttonNum]);
        Debug.Log($"Выбранный ученик {PlayerPrefs.GetString("chosenPupilForAnswers")}");
        SceneManager.LoadScene("ShowAnswers");
    }
}
