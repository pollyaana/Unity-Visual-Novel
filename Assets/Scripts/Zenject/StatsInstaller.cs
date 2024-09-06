using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatsInstaller : MonoInstaller
{
    public TMP_Dropdown groupDropdown;
    public TMP_Dropdown storyDropdown;
    public TMP_Dropdown knowledgeDropdown;
    public Image barPrefab; 
    public Transform barsParent;
    public GameObject axisY;
    public GameObject axisX;
    public TextMeshProUGUI maxCut;
    public TextMeshProUGUI middleCut;
    public TextMeshProUGUI firstCharacter;
    public TextMeshProUGUI secondCharacter;
    public GameObject backgroundPanel;
    public GameObject tablePupils;
    public GameObject prefPupil;
    public Transform tableTransform;
}
