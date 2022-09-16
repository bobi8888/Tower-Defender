using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingTypeList")]
public class BuildingTypeScriptSO : ScriptableObject {
    public List<BuildingTypeScriptSO> buildingTypeList;
}
