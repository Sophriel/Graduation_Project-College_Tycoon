using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
	private bool inputLock;

    public Transform Target;
    private Vector3 camDir;
    private float camSpeed;

    //  카메라 이동 변수
    private Vector3 movement;
    public float MoveSpeed = 10.0f;

    //  카메라 회전 변수
    private float xAngle = 30.0f;
    private float yAngle;
    public float Sensitivity = 150.0f;

    //  카메라 줌 변수
    private float distance = 100.0f;
    private float minDistance = 5.0f;
    private float maxDistance = 200.0f;
    public float ZoomSpeed = 1.0f;

    private void Start()
    {
		inputLock = true;
		iTween.MoveTo(gameObject, iTween.Hash("y", 50.0f, "z", -86.60254f, "time", 3.5f, "oncomplete", "unlockInput"));
	}

	private void unlockInput()
	{
		inputLock = false;
	}

    private void Update()
    {
		if (!inputLock)
		{
			if (Time.timeScale == 0.0f)
				inputForceUpdate();

			else
				inputUpdate();
		}

		//  타겟 시선고정
		transform.LookAt(Target.position);

		if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene("Ingame");
    }

	private void inputUpdate()
	{
		//  카메라 이동
		movement.Set(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		Target.position += Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement * (distance * 0.25f) * MoveSpeed * Time.unscaledDeltaTime;

		//  카메라 회전
		if (!MouseManager.Instance.BuildMode)
		{
			xAngle -= Input.GetAxis("Mouse Y") * Sensitivity * Time.unscaledDeltaTime;
			xAngle = Mathf.Clamp(xAngle, 5.0f, 85.0f);
			yAngle += Input.GetAxis("Mouse X") * Sensitivity * Time.unscaledDeltaTime;
		}

		//  줌인 줌아웃
		distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.unscaledDeltaTime;

		//  최소, 최대 줌 고정
		distance = Mathf.Clamp(distance, minDistance, maxDistance);

		//  카메라 위치설정
		camDir = Quaternion.Euler(xAngle, yAngle, 0.0f) * Vector3.forward;
		transform.position = Target.position + (camDir * -distance);
	}

	private void inputForceUpdate()
	{
		//  카메라 이동
		movement.Set(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
		Target.position += Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement * (distance * 0.25f) * MoveSpeed * Time.unscaledDeltaTime;

		//  카메라 회전
		if (!MouseManager.Instance.BuildMode)
		{
			xAngle -= Input.GetAxisRaw("Mouse Y") * Sensitivity * Time.unscaledDeltaTime;
			xAngle = Mathf.Clamp(xAngle, 5.0f, 85.0f);
			yAngle += Input.GetAxisRaw("Mouse X") * Sensitivity * Time.unscaledDeltaTime;
		}

		//  줌인 줌아웃
		distance -= Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed * Time.unscaledDeltaTime;

		//  최소, 최대 줌 고정
		distance = Mathf.Clamp(distance, minDistance, maxDistance);

		//  카메라 위치설정
		camDir = Quaternion.Euler(xAngle, yAngle, 0.0f) * Vector3.forward;
		transform.position = Target.position + (camDir * -distance);
	}
}
