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

    public bool BuildMode = false;
    public GameObject PointingObject;
    public Vector3 PointingPosition;
    public GameObject PickedObject;


    private void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        guiCamera = FindObjectOfType<GUICamera>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            CancelBuilding();
            return;
        }

        if (Input.GetButtonDown("LeftClick"))
        {
            ClickObject();
            return;
        }

        if (BuildMode)
        {
            OnBuildMode();
        }
    }

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

    private void OnBuildMode()
    {
        if (!PickedObject)
        {
            EscapeBuildMode();
            return;
        }

        CastMainCamera(1 << LayerMask.NameToLayer("Terrain"));

        PickedObject.transform.position = PointingPosition;

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

    //  돈 체크 해야함.
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

    //  돈 지불 해야함.
    private void ConfirmBuilding()
    {
        PickedObject.GetComponent<Building>().IsInstance = false;
        EscapeBuildMode();
    }

    private void CancelBuilding()
    {
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