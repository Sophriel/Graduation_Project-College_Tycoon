using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    private MainCamera mainCamera;
    private GUICamera guiCamera;
    private bool isMainRayHit;
    private float maxDistance = 100.0f;

    [SerializeField]
    PiUIManager piUi;
    private bool menuOpened;
    private PiUI normalMenu;

    public bool BuildMode = false;
    public GameObject PointingObject;
    public Vector3 PointingPosition;
    public GameObject PickedObject;

    public float RotationSpeed = 150.0f;


    private void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        guiCamera = FindObjectOfType<GUICamera>();

        normalMenu = piUi.GetPiUIOf("Right Click Menu");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            CancelBuilding();
        }

        else if (Input.GetButtonDown("LeftClick"))
        {
            ClickObject();
        }

        else if (Input.GetButtonDown("RightClick") && !BuildMode)
        {
            if (!piUi.PiOpened("Right Click Menu"))
            {
                piUi.ChangeMenuState("Right Click Menu", Input.mousePosition);
            }
        }

        else if (Input.GetButtonUp("RightClick") && !BuildMode)
        {
            if (piUi.PiOpened("Right Click Menu"))
            {
                piUi.ChangeMenuState("Right Click Menu", Input.mousePosition);
            }
        }

        if (BuildMode)
        {
            OnBuildMode();
        }
    }

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
        Ray guiRay = guiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(guiRay, maxDistance, 1 << LayerMask.NameToLayer("UI")))
            return true;

        return false;
    }

    #region 오브젝트 클릭

    private void ClickObject()
    {
        //  평상시
        if (!BuildMode)
        {
            //  UI의 클릭을 먼저 검사
            if (CastGuiCamera())
                return;

            CastMainCamera(1 << LayerMask.NameToLayer("Building"));  //  사람도 추가

            //  잡히는게 없으면
            if (PointingObject == null)
                return;

            //  선택한 오브젝트가 없으면
            if (!PickedObject)
            {
                PickObject();
                StartBuilding();  //  @@임시@@
            }

            //  선택한 오브젝트가 기존 선택과 다르면
            else if (PickedObject != PointingObject)
            {
                CancelBuilding();  //  @@임시@@
                PickObject();
                StartBuilding();  //  @@임시@@
            }

            //  같은걸 선택했으면
            else
            {
                CancelBuilding();  //  @@임시@@
                UnPickObject();
            }
        }

        //  빌드모드
        else
        {
            //  건설 가능한 지역 판별
            if (EnableToBuild())
            {
                ConfirmBuilding();
                return;
            }

            CancelBuilding();
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

    #endregion

    #region 건축

    public void StartBuilding()
    {
        if (!PickedObject)
        {
            EscapeBuildMode();
            return;
        }

        BuildMode = true;

        transform.position = PickedObject.transform.position;

        //  사람이나 건물을 밀어내진 않지만, 설치하려면 확인해야함
        PickedObject.GetComponent<Collider>().isTrigger = true;
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
            PickedObject.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime, 0.0f);

        //  오브젝트 색상 변경
        if (EnableToBuild())
            PickedObject.GetComponent<MeshRenderer>().material.color = Color.green;
        else
            PickedObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void EscapeBuildMode()
    {
        BuildMode = false;

        if (PickedObject)
        {
            PickedObject.GetComponent<MeshRenderer>().material.color = Color.white;

            PickedObject.GetComponent<Collider>().isTrigger = false;

            PickedObject = null;
        }

        PointingObject = null;
    }

    /// <summary> 돈 체크 해야함. </summary>
    private bool EnableToBuild()
    {
        if (!PickedObject)
        {
            EscapeBuildMode();
            return false;
        }

        if (CastGuiCamera())
            return false;

        if (PickedObject.GetComponent<Building>().IsColliding)
            return false;

        return true;
    }

    /// <summary> 돈 지불 해야함. </summary> 
    private void ConfirmBuilding()
    {
        PickedObject.GetComponent<Building>().IsInstance = false;
        EscapeBuildMode();
    }

    private void CancelBuilding()
    {
        if (!PickedObject)
            return;

        //  새로 생성된 빌딩
        if (PickedObject.GetComponent<Building>().IsInstance)
            Destroy(PickedObject);

        //  기존에 있던 빌딩
        else
            PickedObject.transform.position = transform.position;

        EscapeBuildMode();
    }

    #endregion
}