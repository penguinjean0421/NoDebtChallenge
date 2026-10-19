using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRouteData", menuName = "NoDebt/Route Data")]
public class RouteData : ScriptableObject
{
    public string routeName;             // 김도윤 루트 / 박서연 루트
    public List<TurnData> turnList;      // 17개의 TurnData 리스트
}