using System;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    public string choiceText;
    public bool isCorrect;

    [Header("오답 시 피드백")]
    [TextArea(2, 4)] public string wrongReason;
    [TextArea(2, 4)] public string hintText;

    [Header("스탯 변화 수치")]
    public int creditChange;
    public int balanceChange;
    public int mentalChange;
    public int favorabilityChange;
}