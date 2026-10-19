using System;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    public string choiceText;            // 선택지 문구 (예: A. 리볼빙을 신청한다)
    public bool isCorrect;               // 정답 여부

    [Header("오답 시 피드백")]
    [TextArea(2, 4)] public string wrongReason; // 왜 위험한지
    [TextArea(2, 4)] public string hintText;    // 정답 힌트

    [Header("스탯 변화 수치")]
    public int creditChange;             // 신용점수 변화
    public int balanceChange;            // 잔고 변화
    public int mentalChange;             // 멘탈 변화
    public int favorabilityChange;       // 가이드 캐릭터 호감도 변화
}