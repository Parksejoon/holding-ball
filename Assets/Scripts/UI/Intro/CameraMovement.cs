﻿using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	// 인스펙터 노출 변수
	// 일반
	[SerializeField]
	private new Camera 	camera;				// 대상 카메라
		
	// 수치
	[SerializeField]
	private Vector2 	limitPos;			// 최대치
		
	// 인스펙터 비노출 변수
	// 수치
	private float 		moveDis;			// 움직임 정도
	private Vector2 	newPos;             // 새로운 좌표
	

	// 프레임
	private void Update()
	{
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				StopCoroutine("CameraMove");

				newPos = -Camera.main.ScreenToWorldPoint(Input.mousePosition) * 0.1f;

				if (newPos.x < limitPos.x && newPos.x > -limitPos.x)
				{
					//camera.transform.position = new Vector3(camera.transform.position.x, newPos.y, UIManager.instance.cameraZpos);
					StartCoroutine(CameraMove(new Vector3(camera.transform.position.x, newPos.y, camera.transform.position.z)));
				}

				if (newPos.y < limitPos.y && newPos.y > -limitPos.y)
				{
					//camera.transform.position = new Vector3(newPos.x, camera.transform.position.y, UIManager.instance.cameraZpos);	
					StartCoroutine(CameraMove(new Vector3(newPos.x, camera.transform.position.y, camera.transform.position.z)));
				}
			}
		}
	}

	// 카메라 이동 코루틴
	private IEnumerator CameraMove(Vector3 targetPos)
	{
		float moveValue = 0;

		while (moveValue < 1)
		{
			camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, moveValue);

			moveValue += 0.03f;

			yield return new WaitForSeconds(0.01f);
		}
	}
}
