﻿using System.Collections.Generic;
using UnityEngine;

public class ShotLineCollider : MonoBehaviour
{
	// 인스펙터 노출 변수
	// 수치
	[SerializeField]
	private Material		powerHolderMat;       // 강화 홀더의 머티리얼
	[SerializeField]
	private ParticleSystem	powerHolderParticle;  // 강화 홀더의 이펙트 파티클
	[SerializeField]
	private float		    perfectDis;			  // 퍼펙트 판정범위
	[SerializeField]
	private float		    goodDis;	  		  // 굿 판정범위

	// 인스펙터 비노출 변수
	// 일반
	[HideInInspector]
	public List<Transform>  perfect;              // 퍼펙트 판정 오브젝트 리스트
	[HideInInspector]
	public List<Transform>  good;                 // 일반 판정 오브젝트 리스트
	[HideInInspector]
	public List<Transform>  holderList;           // 홀더 리스트

	
	// 판정 측정
	public void Judgment()
	{
		// 초기화
		float range = transform.lossyScale.x / 2f;		// 반지름
		float x		= transform.position.x;             // 중심 X좌표
		float y		= transform.position.y;             // 중심 Y좌표

		// 타겟 목록 초기화
		perfect		   = new List<Transform>();
		good		   = new List<Transform>();

		// 홀더 리스트 복사
		holderList = HolderManager.instance.holderList;
		
		// 홀더들을 불러와 판정
		for (int i = 0; i < holderList.Count; i++)
		{
			float	distance = 0;                                       // 홀더와 공 사이의 거리
			float	holdDistance = 0;									// 홀더와 라인 사이의 거리

			if (holderList[i] != null)
			{
				Vector3 holderListPosition = holderList[i].position;        // 홀더 각자의 좌표
				
				// 거리를 측정해서 판정진행
				distance = Mathf.Sqrt(((holderListPosition.x - x) * (holderListPosition.x - x)) + ((holderListPosition.y - y) * (holderListPosition.y - y)));
				holdDistance = Mathf.Abs(distance - range);

				// 퍼펙트 (5점)
				if (holdDistance < perfectDis)
				{
					//perfect.Add(holderList[i]);
					
					ChangeHolder(i, 5);

				}
				// 일반 (4점)
				else if (holdDistance < goodDis)
				{
					//good.Add(holderList[i]);
					
					ChangeHolder(i, 4);
				}
				// 인사이드 (1점)
				else if (distance < range)
				{
					ChangeHolder(i, 1);
				}
			}
		}
	}

	// 홀더 변환
	private void ChangeHolder(int i, int score)
	{
		holderList[i].gameObject.tag = "PowerHolder";

		Holder targetHolder = holderList[i].gameObject.GetComponent<Holder>();

		targetHolder.SetVelo(Vector2.zero);
		targetHolder.holderPower     = score;
		targetHolder.destroyParticle = powerHolderParticle;
		targetHolder.StartDestroyer();

		holderList[i].GetChild(0).gameObject.GetComponent<Renderer>().material = powerHolderMat;
	}
}
