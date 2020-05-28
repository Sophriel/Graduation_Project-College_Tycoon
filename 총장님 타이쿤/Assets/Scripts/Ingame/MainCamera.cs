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
		lockInput();
		iTween.MoveTo(gameObject, iTween.Hash("y", 50.0f, "z", -86.60254f, "time", 3.5f,
			"easetype", iTween.EaseType.easeOutQuart, "oncomplete", "unlockInput", "ignoretimescale", true));
	}

	private void lockInput()
	{
		inputLock = true;
	}
	private void unlockInput()
	{
		inputLock = false;
	}

	#region 포커싱

	public void FocusToTarget(Vector3 pos)
	{
		iTween.ValueTo(gameObject, iTween.Hash("from", distance, "to", 10f, "time", 2.0f,
			"easetype", iTween.EaseType.easeOutQuart, "onupdate", "DistUpdate", "ignoretimescale", true));
		iTween.MoveTo(Target.gameObject, iTween.Hash("position", pos, "time", 2.0f,
			"easetype", iTween.EaseType.easeOutQuart, "ignoretimescale", true));
	}

	private void DistUpdate(float dist)
	{
		distance = dist;
	}

	public void EndFocusing()
	{
		iTween.ValueTo(gameObject, iTween.Hash("from", distance, "to", 100f, "time", 2.0f,
			"easetype", iTween.EaseType.easeOutQuart, "onupdate", "DistUpdate", "ignoretimescale", true));
		iTween.MoveTo(Target.gameObject, iTween.Hash("position", Vector3.zero, "time", 2.0f,
			"easetype", iTween.EaseType.easeOutQuart, "ignoretimescale", true));
	}

	#endregion

	#region 이동

	private void Update()
	{
		if (!inputLock)
		{
			if (Time.timeScale != 1.0f)
				InputForceUpdate();

			else
				InputUpdate();
		}

		//  타겟 시선고정
		transform.LookAt(Target.position);

		if (Input.GetKey(KeyCode.R))
			SceneManager.LoadScene("Ingame");
	}

	private void InputUpdate()
	{
		//  카메라 이동
		movement.Set(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		Target.position += Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement * (distance * 0.25f) * MoveSpeed * Time.deltaTime;

		//  카메라 회전
		if (MouseManager.Instance.MM == MouseMode.Idle)
		{
			xAngle -= Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
			xAngle = Mathf.Clamp(xAngle, 5.0f, 85.0f);
			yAngle += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
		}

		//  줌인 줌아웃
		distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;

		//  최소, 최대 줌 고정
		distance = Mathf.Clamp(distance, minDistance, maxDistance);

		//  카메라 위치설정
		camDir = Quaternion.Euler(xAngle, yAngle, 0.0f) * Vector3.forward;
		iTween.MoveUpdate(gameObject, iTween.Hash("position", Target.position + (camDir * -distance), "time", 0.5f));
	}

	private void InputForceUpdate()
	{
		//  카메라 이동
		movement.Set(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
		Target.position += Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement * (distance * 0.25f) * MoveSpeed * Time.unscaledDeltaTime;

		//  카메라 회전
		if (MouseManager.Instance.MM == MouseMode.Idle)
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

	#endregion
}
