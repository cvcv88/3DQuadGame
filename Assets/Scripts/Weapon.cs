using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type { Melee, Range } // 무기 타입(근접, 원거리)
	public Type type;
	public int damage; // 데미지
	public float rate; // 공속
	public int maxAmmo; // 최대 탄약 탄창
	public int curAmmo; // 현재 탄약 탄창

	public BoxCollider meleeArea; // 범위
	public TrailRenderer trailEffect; // 효과 (잔상을 그려주는 컴포넌트)
	public Transform bulletPos; // 총알 프리팹 생성할 위치
	public GameObject bullet; // 총알 프리팹 저장할 변수
	public Transform bulletCasePos; // 총알 케이스 프리팹 생성할 위치
	public GameObject bulletCase; // 총알 케이스 프리팹 저장할 변수

	public void Use()
	{
		if (type == Type.Melee)
		{
			// Swing();

			StopCoroutine("Swing"); // Coroutine 종료 함수
			StartCoroutine("Swing"); // Coroutine 시작 함수
		}
		else if (type == Type.Range && curAmmo > 0)
		{
			curAmmo--;
			StartCoroutine("Shot");
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

	private IEnumerator Shot()
	{
		// 1. 총알 생성, 총알 발사
		GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
		Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
		bulletRigid.velocity = bulletPos.forward * 50f; // bulletPos의 앞 방향으로 속도 지정
		yield return null;

		// 2. 탄피 배출
		GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
		Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
		Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
		caseRigid.AddForce(caseVec, ForceMode.Impulse); // 탄피 배출
		caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // 탄피 회전
	}

	// 일반 함수 : Use() 메인루틴 -> Swing() 함수 호출, 서브 루틴 -> Use() 메인 루틴
	// Coroutine : Use() 메인루틴 + Swing() 코루틴(Co-Op)
}
