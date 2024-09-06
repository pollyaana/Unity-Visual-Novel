using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Novel
{
    public class Points : MonoBehaviour
    {
        public string answerTable = "StudentAnswer";
        private Image _background;
        private TextMeshProUGUI _pointsText;
        private TextMeshProUGUI _chosenCharacterText;
        public int questionCount;
        private int characterId = 0;
        private string imageName = "";

        [Inject]
        public void Construct(PointsInstaller pointsInstaller)
        {
            _background = pointsInstaller.background;
            _pointsText = pointsInstaller.pointsText;
            _chosenCharacterText = pointsInstaller.chosenCharacterText;
        }


        void Start()
        {
            characterId = PlayerPrefs.GetInt("characterId");
            Debug.Log(characterId);
            if (characterId == 12) imageName = "tavern";
            else if (characterId == 14) imageName = "natasha_house";
            Debug.Log(imageName);
            Sprite image = Resources.Load<Sprite>($"Images\\Backgrounds\\{imageName}");
            Debug.Log(image);
            _background.sprite = image;
            Debug.Log($"Кол-во вопросов и баллов: {PlayerPrefs.GetInt("questionCount")  }");
            Debug.Log($"Получено баллов: {PlayerPrefs.GetString("points")}");
            StartCoroutine(GetPoints());
        }

        public IEnumerator GetPoints()
        {
            Debug.Log(_pointsText.text);
            questionCount = PlayerPrefs.GetInt("questionCount");
            Debug.Log(_pointsText.text);
            yield return questionCount;
            ShowPointsandCharacter(questionCount);
        }

        public void ShowPointsandCharacter(int questionCount)
        {
            Debug.Log(_pointsText.text);
            _pointsText.text = $"{PlayerPrefs.GetString("points")}/{questionCount}";
            Debug.Log(_pointsText.text);
            if (PlayerPrefs.GetString("chosenCharacter") == "Andre") _chosenCharacterText.text = "Андрей Болконский";
            if (PlayerPrefs.GetString("chosenCharacter") == "Natasha") _chosenCharacterText.text = "Наташа Ростова";
        }

        public void GoToTheMainPage()
        {
            Debug.Log("Переходим на главную страницу");
            if (PlayerPrefs.GetString("role") == "pupil")
                SceneManager.LoadScene("Menu");
            else
                SceneManager.LoadScene("Menu2");
        }
    }
}