using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRouteData", menuName = "NoDebt/Route Data")]
public class RouteData : ScriptableObject
{
    public string routeName;

    [Header("Initial Data")]
    public int initialCreditScore;
    public int initialBalance;
    public int initialMental;
    public int initialFavorability;

    [Header("Turn List")]
    public List<TurnData> turnList;
}