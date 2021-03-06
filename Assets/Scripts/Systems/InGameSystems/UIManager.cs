﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
		
	// 인스펙터 노출 변수
	// 일반
	[SerializeField]
	private InitManager		initManager;            // 초기화 매니저
	[SerializeField]
	private CoverSlider		coverSlider;            // 커버 슬라이더
	[SerializeField]
	private PlayEffect		playEffect;             // 시작 이펙트

	// 인스펙터 비노출 변수
	// 수치
	private float			originalTimeScale;      // 원래 타임스케일 값
	private bool			isPaused;				// 일시정지중?

	[HideInInspector]
	public Vector2			midPos;                 // 중앙 지점
		
		
	// 초기화
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		
		midPos = Vector2.zero;

		isPaused = false;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//Screen.SetResolution(Screen.width, Screen.width * (16 / 9), true);
		Application.targetFrameRate = 60;
		
	}

	// 시작 초기화
	private void Start()
	{
		coverSlider.slideFuncs[3] = StartGame;
	}

	// 어플 중지시 퍼즈
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			PassivePause(true);
		}
	}

	// 일시정지 수동으로 끄고 켜기
	public void PassivePause(bool isOff)
	{
		// 퍼즈 해제
		if (!isOff)
		{
			UIEffecter.instance.SetUI(2, false);

			isPaused = false;

			// 타임 스케일 복구
			Time.timeScale = 1;
			GameManager.instance.timeValue = 1f;
		}
		// 퍼즈
		else
		{
			UIEffecter.instance.SetUI(2, true);

			isPaused = true;

			// 타임 스케일 저장
			originalTimeScale = 1;

			// 정지
			Time.timeScale = 0f;
			GameManager.instance.timeValue = 0f;
		}
	}


	// 일시정지 끄고 키기
	public void OnOffPause()
	{
		// 퍼즈 해제
		if (Time.timeScale == 0)
		{
			Continue();
		}
		// 퍼즈
		else
		{
			Pause();
		}
	}

	// 일시정지
	private void Pause()
	{
		UIEffecter.instance.SetUI(2, true);

		isPaused = true;

		// 타임 스케일 저장
		originalTimeScale = Time.timeScale;

		// 정지
		Time.timeScale = 0f;
		GameManager.instance.timeValue = 0f;
	}

	// 계속하기
	private void Continue()
	{
		UIEffecter.instance.SetUI(2, false);

		isPaused = false;

		// 타임 스케일 복구
		Time.timeScale = originalTimeScale;
		GameManager.instance.timeValue = 1f;
	}

	// 시작 UI 제거 및 시작
	public void StartGame()
	{
		// 커버 슬라이더 락
		coverSlider.usingLock = true;

		// 게임 실행
		initManager.enabled = true;
		
		// 루틴 실행
		StartCoroutine(StartRoutine());

		// 최초 실행 여부를 판단하고 값을 초기화
		GuidLineManager.instance.StartSetValue();
	}

	// 시작버튼 클릭 루틴
	private IEnumerator StartRoutine()
	{
		// 아이콘 중앙으로
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], midPos, 0.2f, UIEffecter.FadeFlag.POSITION);
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], new Vector3(0, 0, 360), 0.2f, UIEffecter.FadeFlag.ANGLE);
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], new Vector2(10, 10), 3.5f, UIEffecter.FadeFlag.SCALE);
		playEffect.StartEffect();

		yield return new WaitForSeconds(0.2f);

		// 슬라이드 패널 삭제
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[0], Vector2.zero, 1.2f, UIEffecter.FadeFlag.ALPHA | UIEffecter.FadeFlag.FINDISABLE);
		UIEffecter.instance.ChangeNumberEffect(UIEffecter.instance.texts[0], 0, 0.5f);

		// 버튼 사라짐
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], new Vector2(0, 0), 0.4f, UIEffecter.FadeFlag.ALPHA);

		// 메인패널
		UIEffecter.instance.SetUI(1, true);
	}
}
