using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurnData", menuName = "NoDebt/Turn Data")]
public class TurnData : ScriptableObject
{
    public int turnNumber;               // 턴 번호 (1~17)
    public Sprite bgImage;               // 배경 이미지
    public Sprite characterSprite;       // 스탠딩 일러스트 (정하윤 / 한지후)

    [TextArea(3, 6)]
    public string scenarioText;          // 장면 상황 설명 및 대사

    public List<ChoiceData> choices;     // 선택지 목록 (2~3개)
}