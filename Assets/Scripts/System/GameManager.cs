﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 인스펙터 노출 변수
	// 일반
    [SerializeField]
	private Text		scoreText;                  // 점수
	[SerializeField]
	private int			failScore = 0;              // 페일시 추가 점수
	[SerializeField]
	private float		perfectPower = 50f;         // 퍼펙트시 슛 파워
	[SerializeField]
	private float		goodPower = 200f;           // 굿시 슛 파워
	[SerializeField]
	private float		failPower = 0;              // 페일시 슛 파워

	// 인스펙터 비노출 변수
	// 일반
	private WallManager wallManager;				// 월 매니저
	private Ball		ball;                       // 볼
    private Touch		touch;                      // 터치 구조체
	private Camera		camera;                     // 카메라
	private int			wallCount = 0;				// 벽 카운트

	// 수치
	[HideInInspector]
	public  float	    shotPower = 0;              // 발사 속도
	[HideInInspector]
	public  bool		isTouch;                    // 현제 터치의 상태

	public  int			score = 0;                  // 점수
    private bool		previousIsTouch;            // 이전 터지의 상태
	private bool		canTouch = true;			// 터치 가능?
	

    // 초기화
    void Awake()
    {
		wallManager		= GameObject.Find("WallManager").GetComponent<WallManager>();
        ball			= GameObject.Find("Ball").GetComponent<Ball>();
		camera			= GameObject.Find("Main Camera").GetComponent<Camera>();

        isTouch		    = false;
        previousIsTouch = false;
    }

    // 프레임
    void Update()
    {
		//// 클릭 처리 ( PC )
		//if (Input.GetMouseButtonDown(0))
		//{
		//	isTouch = true;
		//}

		if (!Input.GetMouseButton(0))
		{
			isTouch = false;
		}

		//// 터치 처리 ( 모바일 )

		//if (Input.touchCount > 0)
		//{
		//    for (int i = 0; i < Input.touchCount; i++)
		//    {
		//        touch = Input.GetTouch(i);
		//        if (touch.phase == TouchPhase.Began)
		//        {
		//            isTouch = true;

		//            break;
		//        }
		//    }
		//}

		// 홀딩 처리
		if (isTouch != previousIsTouch && canTouch)
		{
			if (isTouch)
			{
				// 홀딩 처리
				HoldingBall();
			}
			else
			{
				if (ball.isHolding)
				{
					// 언홀딩 처리
					UnHoldingBall();
				}

			}
		}

        previousIsTouch = isTouch;
    }

    // 홀딩 처리
    void HoldingBall()
    {
		ball.HoldingHolder();
    }

    // 언홀딩 처리
    void UnHoldingBall()
    {
        ball.UnholdingHolder();
    }

    // 캐치 퍼펙트판정
    public void PerfectCatch()
    {
		// 발사 속도 설정
		shotPower = perfectPower * Mathf.Min(Mathf.Max(1f, (score / 60f)), 1.5f);
	}

    // 캐치 굿판정
    public void GoodCatch()
    {
		// 발사 속도 설정
        shotPower = goodPower * Mathf.Min(Mathf.Max(1f, (score / 60f)), 1.5f);
    }


    // 캐치 페일판정
    public void FailCatch()
    {
        shotPower = failPower;
		AddScore(failScore);
    }

    // 점수 상승
    public void AddScore(int upScore)
    {
		if (upScore >= 4)
		{
			GameObject.Find("Main Camera").GetComponent<CameraEffect>().FlashBoom();
		}

		score += upScore;
        scoreText.text = score.ToString();
    }

	// 벽 파괴
	public void WallDestroy()
	{
		// 월 매니저로 벽 새로생성 요청
		wallManager.CreateWalls(score);

		if (wallCount++ < 4)
		{
			// 카메라 줌아웃
			StartCoroutine(CameraZoomOut());
		}

		// 공 당기기
		StartCoroutine(BallPull());
	}

	// 홀더 체크
	public void HolderCheck(GameObject target)
	{
		if (ball.bindedHolder == target)
		{
			// 언홀딩
			ball.UnbindingHolder();
		}
	}

	// 게임 오버
	public void GameOver()
	{
		Debug.Log("GG");

		// ☆★☆스크립트 다끊고 제거해야함☆★☆

		//ball.BallDestroy();
		//Destroy(camera.GetComponent<CameraChase>());
	}

	// 카메라 줌아웃
	private IEnumerator CameraZoomOut()
	{
		for (int i = 0; i < 50; i++)
		{
			// 카메라 줌아웃
			camera.orthographicSize += 0.1f;

			yield return new WaitForSeconds(0.01f);
		}
	}

	// 공 당기기
	private IEnumerator BallPull()
	{
		// 공쪽 코루틴 시작
		StartCoroutine(ball.BallPullManager());

		// 터치 제어
		canTouch = false;

		yield return new WaitForSeconds(1f);

		canTouch = true;
	}
}
    
