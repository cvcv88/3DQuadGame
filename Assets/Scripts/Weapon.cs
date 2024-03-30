using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type { Melee, Range } // 무기 타입
	public Type type;
	public int damage; // 데미지
	public float rate; // 공속
	public BoxCollider meleeArea; // 범위
	public TrailRenderer trailEffect; // 효과 (잔상을 그려주는 컴포넌트)

	public void Use()
	{
		if (type == Type.Melee)
		{
			// Swing();

			StopCoroutine("Swing"); // Coroutine 종료 함수
			StartCoroutine("Swing"); // Coroutine 시작 함수
		}
	}
	private IEnumerator Swing() // IEnumerator : 열거형 함수 클래스
	{
		/*
		// 1
		yield return new WaitForSeconds(0.1f); // 0.1초 대기, yield : 결과를 전달하는 키워드
		// 2
		yield return null; // 1 프레임 대기
		// 3
		// yield break; // 코루틴 탈출문
		*/

		yield return new WaitForSeconds(0.1f);
		meleeArea.enabled = true; // 콜라이더 활성화
		trailEffect.enabled = true; // 효과 활성화

		yield return new WaitForSeconds(0.3f);
		meleeArea.enabled = false; // 콜라이더 비활성화

		yield return new WaitForSeconds(0.3f);
		trailEffect.enabled = false; // 효과 비활성화
	}

	// 일반 함수 : Use() 메인루틴 -> Swing() 함수 호출, 서브 루틴 -> Use() 메인 루틴
	// Coroutine : Use() 메인루틴 + Swing() 코루틴(Co-Op)
}
