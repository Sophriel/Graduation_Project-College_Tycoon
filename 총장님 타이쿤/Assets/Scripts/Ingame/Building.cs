using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  모든 설치, 재건축 가능한 오브젝트에 포함하는 스크립트
public class Building : MonoBehaviour
{
	#region 기본 세팅

	public Collider BuildingCollider;
	public bool IsColliding;  //  다른 Building, TerrainObject 와 충돌?

	public bool IsInstance = false;  //  새로 산 빌딩?

	//  빌딩 기본 세팅
	private void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer("Building");

		if (gameObject.GetComponent<Rigidbody>() == null)
		{
			gameObject.AddComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Rigidbody>().isKinematic = true;

		}

		if (gameObject.GetComponent<BoxCollider>() == null)
		{
			gameObject.AddComponent<BoxCollider>();
			BuildingCollider = GetComponent<Collider>();
		}
	}

	#endregion

	#region 빌드모드

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Building")
			|| other.gameObject.layer == LayerMask.NameToLayer("TerrainObject")
			|| other.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			IsColliding = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Building")
			|| other.gameObject.layer == LayerMask.NameToLayer("TerrainObject")
			|| other.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			IsColliding = false;
		}
	}

	#endregion

	#region 멤버

	public Department Owner;

	public int Price = 1000;

	#endregion
}
