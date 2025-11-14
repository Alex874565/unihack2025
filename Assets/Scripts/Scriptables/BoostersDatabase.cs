using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostersDatabase", menuName = "ScriptableObjects/BoostersDatabase", order = 6)]
public class BoostersDatabase : ScriptableObject
{
    public List<BoosterData> Boosters => _boosters;
    [SerializeField] private List<BoosterData> _boosters;
}
