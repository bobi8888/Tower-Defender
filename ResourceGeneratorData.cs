using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//custom classes do not show up in the editor by default.
public class ResourceGeneratorData{

    public float timerMax;
    public ResourceTypeSO resourceType;
    public float resourceDetectionRadius;
    public int maxResourceAmount;
}
