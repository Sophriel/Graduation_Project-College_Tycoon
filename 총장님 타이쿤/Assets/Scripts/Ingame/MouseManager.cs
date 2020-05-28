using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseMode
{
	Idle,
	BuildMode,
	AssignMode
}

public class MouseManager : MonoBehaviour
{
	#region Singleton

	private static volatile MouseManager instance;
	private static object _lock = new System.Object();

	public static MouseManager Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<MouseManager>();

			if (instance != null)
				return instance;

			CreateThis();

			return instance;
		}
	}

	public static MouseManager CreateThis()
	{
		GameObject MouseManagerGameObject = new GameObject("Mouse Manager");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = MouseManagerGameObject.AddComponent<MouseManager>();

		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	#endregion

	#region Subject of Observers

	public delegate void TutorialEvent(bool flag);
	private event TutorialEvent onTutorialEvent; 

	public void AddTutorialEvent(TutorialEvent func)
	{
		onTutorialEvent += func;
	}

	public void DeleteTutorialEvent(TutorialEvent func)
	{
		if (onTutorialEvent != null)
			onTutorialEvent -= func;
	}

	public delegate void BuildEvent();
	private event BuildEvent onBuildEvent;

	public void AddBuildEvent(BuildEvent func)
	{
		onBuildEvent += func;
	}

	public void DeleteBuildEvent(BuildEvent func)
	{
		if (onBuildEvent != null)
			onBuildEvent -= func;
	}

	public delegate void AssignEvent(GameObject building);
	private event AssignEvent onAssignEvent;

	public void AddAssignEvent(AssignEvent func)
	{
		onAssignEvent += func;
	}

	public void DeleteAssignEvent(AssignEvent func)
	{
		if (onAssignEvent != null)
			onAssignEvent -= func;
	}

	#endregion

	private MainCamera mainCamera;
	[SerializeField]
	private Camera guiCamera;
	private bool isMainRayHit;
	private float maxDistance = 200.0f;

	public PiUIManager piUi;

	public MouseMode MM { get; private set; }
	public GameObject PointingObject;
	public Vector3 PointingPosition;
	public GameObject PickedObject;
	public float RotationSpeed = 150.0f;

	public ESCMenu ESCmenu;

	private void Start()
	{
		mainCamera = FindObjectOfType<MainCamera>();

		MM = MouseMode.Idle;
	}

	private void Update()
	{
		InputUpdate();

		switch (MM)
		{
			case MouseMode.Idle:
				break;
			case MouseMode.BuildMode:
				OnBuildMode();
				break;
			case MouseMode.AssignMode:
				break;
			default:
				break;
		}
	}

	private void InputUpdate()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			OnCancel();
		}

		else if (Input.GetButtonDown("LeftClick"))
		{
			ClickObject();
		}

		else if (Input.GetButtonDown("RightClick") && MM == MouseMode.Idle)
		{
			if (!piUi.PiOpened("Right Click Menu"))
			{
				piUi.ChangeMenuState("Right Click Menu", Input.mousePosition);
			}
		}

		else if (Input.GetButtonUp("RightClick") && MM == MouseMode.Idle)
		{
			if (piUi.PiOpened("Right Click Menu"))
			{
				piUi.ChangeMenuState("Right Click Menu", Input.mousePosition);
			}
		}
	}

	#region ESC

	private void OpenESCMenu()
	{
		ESCmenu.OnClick();
	}

	private void OnCancel()
	{
		switch (MM)
		{
			case MouseMode.Idle:
				OpenESCMenu();
				break;
			case MouseMode.BuildMode:
				CancelBuilding();
				break;
			case MouseMode.AssignMode:
				CancelAssigning();
				break;
			default:
				break;
		}
	}

	#endregion

	#region 캐스팅

	/// <summary> 필드로 캐스팅 </summary> 
	private void CastMainCamera(int layerMask)
	{
		Ray mainRay = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

		isMainRayHit = Physics.Raycast(mainRay, out RaycastHit mainHit, maxDistance, layerMask);

		if (isMainRayHit)
		{
			PointingObject = mainHit.transform.gameObject;
			PointingPosition = mainHit.point;
			Debug.DrawRay(mainRay.origin, mainRay.direction * mainHit.distance, Color.red);
		}

		else
		{
			PointingObject = null;
		}
	}

	/// <summary> UI로 캐스팅 </summary> 
	private bool CastGuiCamera()
	{
		Ray guiRay = guiCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(guiRay, maxDistance * 5.0f, 1 << LayerMask.NameToLayer("UI")))
			return true;

		return false;
	}

	#endregion

	#region 오브젝트 클릭

	private void ClickObject()
	{
		switch (MM)
		{
			//  평상시
			case MouseMode.Idle:
				//  UI의 클릭을 먼저 검사
				if (CastGuiCamera())
					return;

				CastMainCamera(1 << LayerMask.NameToLayer("Person") | 1 << LayerMask.NameToLayer("Building"));

				//  잡히는게 없으면
				if (PointingObject == null)
					return;

				//  선택한 오브젝트가 없으면
				if (!PickedObject)
				{
					SendMessageToPO();
					PickObject();
					StartBuilding();  //  @@임시@@
				}

				//  선택한 오브젝트가 기존 선택과 다르면
				else if (PickedObject != PointingObject)
				{
					SendMessageToPO();
					CancelBuilding();  //  @@임시@@
					PickObject();
					StartBuilding();  //  @@임시@@
				}

				//  같은걸 선택했으면
				else
				{
					SendMessageToPO();
					CancelBuilding();  //  @@임시@@
					UnPickObject();
				}
				break;


			//  빌드모드
			case MouseMode.BuildMode:
				//  건설 가능한 지역 판별
				if (EnableToBuild())
				{
					ConfirmBuilding();
					return;
				}

				CancelBuilding();
				break;

			case MouseMode.AssignMode:
				CastMainCamera(1 << LayerMask.NameToLayer("Building"));
				ConfirmAssigning();
				break;
			default:
				break;
		}
	}

	private void PickObject()
	{
		PickedObject = PointingObject;
		PointingObject = null;
	}

	private void UnPickObject()
	{
		PickedObject = null;
		PointingObject = null;
	}

	public void SendMessageToPO()
	{
		if (PointingObject)
			PointingObject.BroadcastMessage("OnMouseClickThis", SendMessageOptions.DontRequireReceiver);

		if (PickedObject)
			PickedObject.BroadcastMessage("OnMouseClickOther", SendMessageOptions.DontRequireReceiver);
	}

	#endregion

	#region 빌드모드

	private Building pickedBuilding;

	public void StartBuilding()
	{
		if (!PickedObject || PickedObject.layer != LayerMask.NameToLayer("Building"))
		{
			EscapeBuildMode();
			return;
		}

		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Click);

		MM = MouseMode.BuildMode;

		transform.position = PickedObject.transform.position;

		//  사람이나 건물을 밀어내진 않지만, 설치하려면 확인해야함
		PickedObject.GetComponent<Collider>().isTrigger = true;
		pickedBuilding = PickedObject.GetComponent<Building>();
	}

	/// <summary> 빌드 모드에서 빌딩에 대한 입력, 조건 처리 </summary>  
	private void OnBuildMode()
	{
		if (!PickedObject)
		{
			EscapeBuildMode();
			return;
		}

		CastMainCamera(1 << LayerMask.NameToLayer("Terrain"));

		//  오브젝트 이동 회전
		PickedObject.transform.position = PointingPosition;
		PickedObject.transform.rotation = Quaternion.Euler(0.0f,
			PickedObject.transform.rotation.eulerAngles.y + Input.GetAxisRaw("Mouse X") * RotationSpeed * Time.unscaledDeltaTime, 0.0f);

		//  오브젝트 색상 변경
		if (EnableToBuild())
			PickedObject.GetComponent<MeshRenderer>().material.color = Color.green;
		else
			PickedObject.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	private void EscapeBuildMode()
	{
		MM = MouseMode.Idle;

		if (PickedObject && PickedObject.layer == LayerMask.NameToLayer("Building"))
		{
			PickedObject.GetComponent<MeshRenderer>().material.color = Color.white;

			PickedObject.GetComponent<Collider>().isTrigger = false;

			PickedObject = null;
			pickedBuilding = null;
		}

		PointingObject = null;
	}

	/// <summary> UI에 닿는지, 건물이 지어질 수 있는지 확인 </summary>
	private bool EnableToBuild()
	{
		if (!PickedObject || PickedObject.layer != LayerMask.NameToLayer("Building"))
		{
			EscapeBuildMode();
			return false;
		}

		if (CastGuiCamera())
			return false;

		if (pickedBuilding.IsColliding || !GameManager.Instance.CanSpendMoney(pickedBuilding.Price))
			return false;

		return true;
	}

	/// <summary> 건물을 설치하고 GM에 알림 </summary> 
	private void ConfirmBuilding()
	{
		//  게임내에 추가
		pickedBuilding.IsInstance = false;
		GameManager.Instance.AddBuildingInGame(PickedObject);
		GameManager.Instance.SpendMoney(pickedBuilding.Price);

		//  건설시 옵저버 Notify
		onTutorialEvent?.Invoke(true);
		onBuildEvent?.Invoke();

		EscapeBuildMode();  //  빌드모드 종료

		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Build);
	}

	private void CancelBuilding()
	{
		if (!PickedObject || PickedObject.layer != LayerMask.NameToLayer("Building"))
			return;

		//  새로 생성된 빌딩
		if (PickedObject.GetComponent<Building>().IsInstance)
			Destroy(PickedObject);

		//  기존에 있던 빌딩
		else
			PickedObject.transform.position = transform.position;

		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Cant);
		EscapeBuildMode();
	}

	#endregion

	#region 할당모드

	public void StartAssigning()
	{
		MM = MouseMode.AssignMode;
	}

	private void EscapeAssignMode()
	{
		MM = MouseMode.Idle;
	}

	private void ConfirmAssigning()
	{
		if (!PointingObject || PointingObject.layer != LayerMask.NameToLayer("Building"))
		{
			CancelAssigning();
			return;
		}

		if (CastGuiCamera())
		{
			CancelAssigning();
			return;
		}

		onTutorialEvent?.Invoke(true);
		onAssignEvent?.Invoke(PointingObject);

		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Build);

		EscapeAssignMode();
	}

	private void CancelAssigning()
	{
		onAssignEvent?.Invoke(null);

		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Cant);

		EscapeAssignMode();
	}

	#endregion
}