using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("--- Route Data ---")]
    public RouteData doYoonRoute;
    public RouteData seoYeonRoute;

    [Header("--- Current Game ---")]
    public RouteData currentRoute;
    public int currentTurnIndex = 0;

    [Header("--- Current Stats ---")]
    public int creditScore;
    public int balance;
    public int mental;
    public int favorability;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        ReturnToGameStart();
    }

    public void SelectDoYoonRoute()
    {
        StartRoute(doYoonRoute);
    }

    public void SelectSeoYeonRoute()
    {
        StartRoute(seoYeonRoute);
    }

    void StartRoute(RouteData route)
    {
        if (route == null) { Debug.LogError("RouteData가 할당되지 않았습니다!"); return; }

        currentRoute = route;
        currentTurnIndex = 0;

        creditScore = route.initialCreditScore;
        balance = route.initialBalance;
        mental = route.initialMental;
        favorability = route.initialFavorability;

        UIManager.Instance.ShowGamePanel();
        LoadTurn(currentTurnIndex);
    }

    public void LoadTurn(int index)
    {
        if (index >= currentRoute.turnList.Count) { ShowEnding(); return; }

        TurnData data = currentRoute.turnList[index];

        if (data.choices != null && data.choices.Count > 0)
        {
            for (int i = 0; i < data.choices.Count; i++)
            {
                int randomIndex = Random.Range(i, data.choices.Count);
                var temp = data.choices[i];
                data.choices[i] = data.choices[randomIndex];
                data.choices[randomIndex] = temp;
            }
        }

        UIManager.Instance.UpdateTurnUI(data);
    }

    // 선택지 선택 시 처리
    public void OnSelectChoice(ChoiceData choice)
    {
        // 정답: 스탯 변동 반영 후 다음 턴 진행
        // 오답: 오답 팝업 출력 (동일 턴 재선택)  
        if (choice.isCorrect)
        {
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

    public void ReturnToGameStart()
    {
        currentRoute = null;
        currentTurnIndex = 0;
        UIManager.Instance.ShowGameStartPanel();
    }

    public void ReturnToRouteSelect()
    {
        currentRoute = null;
        currentTurnIndex = 0;
        UIManager.Instance.ShowRouteSelectPanel();
    }
}