using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData",menuName = "Game/Equipment/EvolutionDatabase")]
public class EvolutionDataBase : ScriptableObject
{
    public List<EvolutionData> evolutionDataList;
}
