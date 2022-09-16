using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }//this is a property, Pascal, not camel

    public event EventHandler OnResourceAmountChanged;

	[SerializeField] private List<ResourceAmount> startingResourceAmountList;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    private void Awake(){
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
			resourceAmountDictionary[resourceType] = 0;                 
		}

		foreach (ResourceAmount resourceAmount in startingResourceAmountList) {
			AddResource(resourceAmount.resourceType, resourceAmount.amount);
		}	
    }

 /*   private void TestLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmountDictionary[resourceType]);
        }
    }*/
    public void AddResource(ResourceTypeSO resourceType, int amount){
        resourceAmountDictionary[resourceType] += amount;

//Null Conditional Operator ?.Invoke only checks after invoke if prior code is not null
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

    }
    public int GetResourceAmount(ResourceTypeSO resourceType) {
        return resourceAmountDictionary[resourceType];
    }
    
	public bool CanAfford(ResourceAmount[] resourceAmountArray){ 
		foreach (ResourceAmount resourceAmount in resourceAmountArray){ 
			if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount){
				//can afford
			} else {//cannot afford
				return false;
			}
		}
		return true; //can afford all
	}

	public void SpendResources(ResourceAmount[] resourceAmountArray){ 
		foreach (ResourceAmount resourceAmount in resourceAmountArray){
			resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
		}
			
	}

}
