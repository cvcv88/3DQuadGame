using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type { Melee, Range } // ���� Ÿ��
	public Type type;
	public int damage; // ������
	public float rate; // ����
	public BoxCollider meleeArea; // ����
	public TrailRenderer trailEffect; // ȿ�� (�ܻ��� �׷��ִ� ������Ʈ)

	public void Use()
	{
		if (type == Type.Melee)
		{
			// Swing();

			StopCoroutine("Swing"); // Coroutine ���� �Լ�
			StartCoroutine("Swing"); // Coroutine ���� �Լ�
		}
	}
	private IEnumerator Swing() // IEnumerator : ������ �Լ� Ŭ����
	{
		/*
		// 1
		yield return new WaitForSeconds(0.1f); // 0.1�� ���, yield : ����� �����ϴ� Ű����
		// 2
		yield return null; // 1 ������ ���
		// 3
		// yield break; // �ڷ�ƾ Ż�⹮
		*/

		yield return new WaitForSeconds(0.1f);
		meleeArea.enabled = true; // �ݶ��̴� Ȱ��ȭ
		trailEffect.enabled = true; // ȿ�� Ȱ��ȭ

		yield return new WaitForSeconds(0.3f);
		meleeArea.enabled = false; // �ݶ��̴� ��Ȱ��ȭ

		yield return new WaitForSeconds(0.3f);
		trailEffect.enabled = false; // ȿ�� ��Ȱ��ȭ
	}

	// �Ϲ� �Լ� : Use() ���η�ƾ -> Swing() �Լ� ȣ��, ���� ��ƾ -> Use() ���� ��ƾ
	// Coroutine : Use() ���η�ƾ + Swing() �ڷ�ƾ(Co-Op)
}
