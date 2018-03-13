﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
	// 인스펙터 노출 변수
	// 일반
	[SerializeField]
	private Object[]	wallsArray;				    // 벽 조합 배열

	// 수치
	[SerializeField]
	private float	    rotationSpeed = 1f;         // 회전속도
	[SerializeField]
	private float		wallsScale = 1f;			// 벽의 크기

	// 인스펙터 비노출 변수
	// 일반
	private GameManager gameManager;                // 게임매니저
	private Transform	wallsTransform;				// 벽들의 트랜스폼
	private float		timer = 0f;                 // 타이머	


	// 초기화
	private void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		wallsTransform = GameObject.Find("Walls"). GetComponent<Transform>();
	}

	// 프레임
	private void Update()
	{
		timer += Time.deltaTime;

		wallsTransform.rotation = Quaternion.Euler(0, 0, timer * rotationSpeed);
	}

	// 벽 생성
	public void CreateWalls(int ind)
	{
		float wallsSize = wallsTransform.localScale.x;
		// ind = level + score

		// 오버 인덱싱
		if (ind >= wallsArray.Length)
		{
			ind = wallsArray.Length - 1;
		}

		// 생성
		GameObject walls = Instantiate(wallsArray[ind], new Vector3(0, 0, 0), Quaternion.identity, transform) as GameObject;
		int		   warringWallNum = Random.Range(0, Mathf.Min(3, (gameManager.level / 3)));

		// 기존 벽을 파괴
		Destroy(wallsTransform.gameObject);

		// 초기화
		wallsTransform = walls.GetComponent<Transform>();

		// 벽 크기 및 회전량 설정
		float scaleValue = wallsScale * wallsSize;

		wallsTransform.localScale = new Vector3(scaleValue, scaleValue);
		wallsTransform.rotation = Quaternion.Euler(0, 0, 0);

		// 타이머 재설정
		timer = Random.Range(0, 90);
	}


}
