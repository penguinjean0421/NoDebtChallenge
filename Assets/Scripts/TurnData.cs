using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnData", menuName = "NoDebt/Turn Data")]
public class TurnData : ScriptableObject
{
    public int turnNumber;
    public Sprite bgImage;
    public Sprite characterSprite;

    [TextArea(3, 6)]
    public string scenarioText;

    public List<ChoiceData> choices;
}