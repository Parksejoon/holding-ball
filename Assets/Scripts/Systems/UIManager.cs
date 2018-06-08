﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
		
	// 인스펙터 노출 변수
	// 일반
	[SerializeField]
	private Button 			restartButton;          // 재시작 버튼
	[SerializeField]
	private StartManager	startManager;           // 시작 매니저
	[SerializeField]
	private CoverSlider		coverSlider;            // 커버 슬라이더
	[SerializeField]
	private PlayEffect		playEffect;				// 시작 이펙트
		
	// 인스펙터 비노출 변수
	// 수치
	private float			originalTimeScale;		// 원래 타임스케일 값
		
		
	// 초기화
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	// 시작 초기화
	private void Start()
	{
		coverSlider.slideFuncs[3] = SetUIs;
	}

	// 퍼즈 체크
	public void CheckPause()
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
		
		// 타임 스케일 복구
		Time.timeScale = originalTimeScale;
		GameManager.instance.timeValue = 1f;
	}

	// 시작 UI 제거 및 시작
	public void SetUIs()
	{
		// 커버 슬라이더 락
		coverSlider.usingLock = true;

		// 게임 실행
		restartButton.interactable = true;
		startManager.enabled = true;
		
		// 루틴 실행
		StartCoroutine(StartRoutine());
	}

	// 시작버튼 클릭 루틴
	private IEnumerator StartRoutine()
	{
		// 사라짐
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[0], Vector2.zero, 1.2f, UIEffecter.FadeFlag.ALPHA | UIEffecter.FadeFlag.FINDISABLE);
		//UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[0], Vector2.zero, 1.2f, UIEffecter.FadeFlag.ALPHA);

		// 중앙으로
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], new Vector2(Screen.width / 2f, Screen.height / 2f), 0.2f, UIEffecter.FadeFlag.POSITION);
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], new Vector3(0, 0, 360), 0.2f, UIEffecter.FadeFlag.ANGLE);
		UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], Vector2.zero, 0.8f, UIEffecter.FadeFlag.ALPHA | UIEffecter.FadeFlag.FINDISABLE);
		//UIEffecter.instance.FadeEffect(UIEffecter.instance.panels[4], Vector2.zero, 1f, UIEffecter.FadeFlag.ALPHA);
		playEffect.StartEffect();

		yield return null;

		// 메인패널
		UIEffecter.instance.SetUI(1, true);
	}
}
