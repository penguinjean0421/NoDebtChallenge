using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("--- 메인 게임 UI 요소 ---")]
    public Image bgImage;                     // 배경 이미지
    public Image characterImage;              // 가이드 캐릭터 스탠딩 일러스트
    public TextMeshProUGUI scenarioText;      // 지문 및 상황 대사 텍스트
    public Transform choiceButtonContainer;  // 선택지 버튼들이 생성될 Vertical Layout Group Transform
    public GameObject choiceButtonPrefab;    // 선택지 버튼 프리팹

    [Header("--- 게이지 스탯 UI ---")]
    public TextMeshProUGUI creditText;        // 💳 신용점수
    public TextMeshProUGUI balanceText;       // 💰 통장 잔고
    public TextMeshProUGUI mentalText;        // 🧠 멘탈
    public TextMeshProUGUI favorabilityText;  // 💗 가이드 캐릭터 호감도

    [Header("--- 오답 피드백 팝업 UI ---")]
    public GameObject feedbackPopupPanel;     // 오답 전면 팝업 패널
    public TextMeshProUGUI wrongReasonText;  // 왜 위험한지 설명 텍스트
    public TextMeshProUGUI hintText;         // 정답 힌트 텍스트
    public Button closePopupButton;          // [다시 선택하기] 버튼

    [Header("--- 엔딩 UI ---")]
    public GameObject endingPanel;           // 엔딩 화면 패널
    public TextMeshProUGUI endingTitleText;  // 엔딩 제목 텍스트

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // 팝업 닫기 버튼에 이벤트 연결 (동일 턴 재선택용)
        if (closePopupButton != null) { closePopupButton.onClick.AddListener(HideWrongFeedbackPopup); }

        // 초기 패널 비활성화
        if (feedbackPopupPanel != null) { feedbackPopupPanel.SetActive(false); }
        if (endingPanel != null) { endingPanel.SetActive(false); }
    }

    // 1. 해당 턴의 데이터로 UI 갱신
    public void UpdateTurnUI(TurnData data)
    {
        // 배경 설정
        if (data.bgImage != null)
        {
            bgImage.sprite = data.bgImage;
            bgImage.gameObject.SetActive(true);
        }

        // 캐릭터 스탠딩 일러스트 설정
        if (data.characterSprite != null)
        {
            characterImage.sprite = data.characterSprite;
            characterImage.gameObject.SetActive(true);
        }
        else { characterImage.gameObject.SetActive(false); }

        // 본문 텍스트 출력
        scenarioText.text = data.scenarioText;

        // 이전 턴 선택지 버튼 제거 후 새 선택지 버튼 생성
        ClearChoiceButtons();
        CreateChoiceButtons(data.choices);

        // 스탯 표시 업데이트
        UpdateStatUI();
    }

    // 2. 게이지 스탯 수치 UI 반영
    public void UpdateStatUI()
    {
        if (GameManager.Instance == null) return;

        creditText.text = $"💳 신용점수: {GameManager.Instance.creditScore}";
        balanceText.text = $"💰 잔고: {GameManager.Instance.balance:N0}원";
        mentalText.text = $"🧠 멘탈: {GameManager.Instance.mental}";
        favorabilityText.text = $"💗 호감도: {GameManager.Instance.favorability}";
    }

    // 3. 동적 선택지 버튼 생성 (2~3개)
    void CreateChoiceButtons(List<ChoiceData> choices)
    {
        foreach (ChoiceData choice in choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);

            // 버튼 라벨 텍스트 바인딩
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) { btnText.text = choice.choiceText; }

            // 버튼 클릭 이벤트 바인딩
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                ChoiceData localChoice = choice; // 람다 캡처용 지역 변수
                btn.onClick.AddListener(() => GameManager.Instance.OnSelectChoice(localChoice));
            }
        }
    }

    // 기존 선택지 버튼 싹 비우기
    void ClearChoiceButtons()
    {
        foreach (Transform child in choiceButtonContainer) { Destroy(child.gameObject); }
    }

    // 4. 오답 선택 시 피드백 팝업 출력
    public void ShowWrongFeedbackPopup(string reason, string hint)
    {
        wrongReasonText.text = $"<color=#FF5555><b>[위험 요소]</b></color>\n{reason}";
        hintText.text = $"<color=#FFDD55><b>[💡 힌트]</b></color>\n{hint}";
        feedbackPopupPanel.SetActive(true);
    }

    // 오답 팝업 닫기 -> 동일 턴에서 다시 선택 가능
    public void HideWrongFeedbackPopup()
    {
        feedbackPopupPanel.SetActive(false);
    }

    // 5. 17턴 종료 후 트루 엔딩 화면 연출
    public void ShowEndingUI()
    {
        endingPanel.SetActive(true);

        string routeTitle = GameManager.Instance.currentRoute != null
            ? GameManager.Instance.currentRoute.routeName
            : "";

        endingTitleText.text = $"🏆 TRUE ENDING\n\n<{routeTitle} 완전 클리어!>";
    }
}