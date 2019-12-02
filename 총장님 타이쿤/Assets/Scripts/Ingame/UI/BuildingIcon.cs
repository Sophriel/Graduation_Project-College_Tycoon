using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIcon : MonoBehaviour
{
    public GameObject BuildingPrefab;

    public void OnClick()
    {
        MouseManager.Instance.PickedObject = CreateBuilding();
        MouseManager.Instance.StartBuilding();
    }

    public GameObject CreateBuilding()
    {
        GameObject building;
        building = Instantiate(BuildingPrefab);
        building.layer = LayerMask.NameToLayer("Building");
        building.AddComponent<Rigidbody>().useGravity = false;
        building.GetComponent<Rigidbody>().isKinematic = true;
        building.AddComponent<BoxCollider>();
        building.AddComponent<Building>().IsInstance = true;

        return building;
    }
}
