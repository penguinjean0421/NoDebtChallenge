using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("현재 상태")]
    public RouteData currentRoute;
    public int currentTurnIndex = 0;

    [Header("게이지 스탯")]
    public int creditScore;
    public int balance;
    public int mental;
    public int favorability;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // 루트 선택 시 호출
    public void SelectRoute(RouteData route)
    {
        currentRoute = route;
        currentTurnIndex = 0;
        LoadTurn(currentTurnIndex);
    }

    // 턴 로드
    public void LoadTurn(int index)
    {
        if (index >= currentRoute.turnList.Count) { ShowEnding(); return; }

        TurnData data = currentRoute.turnList[index];
        UIManager.Instance.UpdateTurnUI(data);
    }

    // 선택지 선택 시 처리
    public void OnSelectChoice(ChoiceData choice)
    {
        if (choice.isCorrect)
        {
            // 정답 처리: 스탯 반영 및 다음 턴으로
            creditScore += choice.creditChange;
            balance += choice.balanceChange;
            mental += choice.mentalChange;
            favorability += choice.favorabilityChange;

            UIManager.Instance.UpdateStatUI();

            currentTurnIndex++;
            LoadTurn(currentTurnIndex);
        }
        else { UIManager.Instance.ShowWrongFeedbackPopup(choice.wrongReason, choice.hintText); }
    }

    void ShowEnding()
    {
        UIManager.Instance.ShowEndingUI();
    }
}