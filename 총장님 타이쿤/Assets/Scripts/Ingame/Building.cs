using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Collider BuildingCollider;
    public bool IsColliding;

    public bool IsInstance = false;

    void Start()
    {
        BuildingCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            IsColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            IsColliding = false;
        }
    }

}
