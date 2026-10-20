using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("--- Main Panel ---")]
    public GameObject gameStartPanel;
    public GameObject routeSelectPanel;
    public GameObject gamePanel;
    public GameObject feedbackPopupPanel;
    public GameObject endingPanel;

    [Header("--- Route Choice UI ---")]
    public Button doYoonRouteButton;
    public Button seoYeonRouteButton;

    [Header("--- Main Game UI  ---")]
    public Image bgImage;
    public Image characterImage;
    public TextMeshProUGUI scenarioText;
    public Transform choiceButtonContainer;
    public GameObject choiceButtonPrefab;

    [Header("--- Gage Stat UI ---")]
    public TextMeshProUGUI creditText;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI mentalText;
    public TextMeshProUGUI favorabilityText;

    [Header("--- Wrong Reason Popup UI ---")]
    public GameObject choicePanel;
    public TextMeshProUGUI wrongReasonText;
    public TextMeshProUGUI hintText;
    public Button closePopupButton;

    [Header("--- Ending UI ---")]
    public TextMeshProUGUI endingTitleText;
    public Button restartButton;

    [Header("Button UI")]
    public Button gameStartButton;
    public Button gameQuitButton;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (gameStartButton != null) { gameStartButton.onClick.AddListener(() => GameManager.Instance.ReturnToRouteSelect()); }
        if (doYoonRouteButton != null) { doYoonRouteButton.onClick.AddListener(() => GameManager.Instance.SelectDoYoonRoute()); }
        if (seoYeonRouteButton != null) { seoYeonRouteButton.onClick.AddListener(() => GameManager.Instance.SelectSeoYeonRoute()); }
        if (closePopupButton != null) { closePopupButton.onClick.AddListener(HideWrongFeedbackPopup); }
        if (restartButton != null) { restartButton.onClick.AddListener(() => GameManager.Instance.ReturnToRouteSelect()); }
    }

    public void ShowGameStartPanel()
    {
        gameStartPanel.SetActive(true);
        routeSelectPanel.SetActive(false);
        gamePanel.SetActive(false);
        feedbackPopupPanel.SetActive(false);
        endingPanel.SetActive(false);
    }

    public void ShowRouteSelectPanel()
    {
        gameStartPanel.SetActive(false);
        routeSelectPanel.SetActive(true);
        gamePanel.SetActive(false);
        feedbackPopupPanel.SetActive(false);
        endingPanel.SetActive(false);
    }

    public void ShowGamePanel()
    {
        gameStartPanel.SetActive(false);
        routeSelectPanel.SetActive(false);
        gamePanel.SetActive(true);
        feedbackPopupPanel.SetActive(false);
        endingPanel.SetActive(false);
    }

    public void UpdateTurnUI(TurnData data)
    {
        if (data.bgImage != null)
        {
            bgImage.sprite = data.bgImage;
            bgImage.gameObject.SetActive(true);
        }

        if (data.characterSprite != null)
        {
            characterImage.sprite = data.characterSprite;
            characterImage.gameObject.SetActive(true);
        }
        else { characterImage.gameObject.SetActive(false); }

        scenarioText.text = data.scenarioText;

        ClearChoiceButtons();
        CreateChoiceButtons(data.choices);
        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        if (GameManager.Instance == null) { return; }

        creditText.text = $"신용점수: {GameManager.Instance.creditScore}";
        balanceText.text = $"잔고: {GameManager.Instance.balance:N0}원";
        mentalText.text = $"멘탈: {GameManager.Instance.mental}";
        favorabilityText.text = $"호감도: {GameManager.Instance.favorability}";
    }

    void CreateChoiceButtons(List<ChoiceData> choices)
    {
        foreach (ChoiceData choice in choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            if (btnText != null) { btnText.text = choice.choiceText; }

            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                ChoiceData localChoice = choice;
                btn.onClick.AddListener(() => GameManager.Instance.OnSelectChoice(localChoice));
            }
        }
    }

    void ClearChoiceButtons()
    {
        foreach (Transform child in choiceButtonContainer) { Destroy(child.gameObject); }
    }

    public void ShowWrongFeedbackPopup(string reason, string hint)
    {
        choicePanel.SetActive(false);

        wrongReasonText.text = $"<color=#FF5555><b>[위험 요소]</b></color>\n{reason}";
        hintText.text = $"<color=#FFDD55><b>[힌트]</b></color>\n{hint}";

        feedbackPopupPanel.SetActive(true);
    }

    public void HideWrongFeedbackPopup()
    {
        feedbackPopupPanel.SetActive(false);
        choicePanel.SetActive(true);
    }

    public void ShowEndingUI()
    {
        choicePanel.SetActive(false);
        endingPanel.SetActive(true);
        string routeName = GameManager.Instance.currentRoute != null ? GameManager.Instance.currentRoute.routeName : "";
        endingTitleText.text = $"TRUE ENDING\n\n<{routeName} 클리어!>";
    }
}