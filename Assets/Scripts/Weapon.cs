using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type { Melee, Range } // ���� Ÿ��(����, ���Ÿ�)
	public Type type;
	public int damage; // ������
	public float rate; // ����
	public int maxAmmo; // �ִ� ź�� źâ
	public int curAmmo; // ���� ź�� źâ

	public BoxCollider meleeArea; // ����
	public TrailRenderer trailEffect; // ȿ�� (�ܻ��� �׷��ִ� ������Ʈ)
	public Transform bulletPos; // �Ѿ� ������ ������ ��ġ
	public GameObject bullet; // �Ѿ� ������ ������ ����
	public Transform bulletCasePos; // �Ѿ� ���̽� ������ ������ ��ġ
	public GameObject bulletCase; // �Ѿ� ���̽� ������ ������ ����

	public void Use()
	{
		if (type == Type.Melee)
		{
			// Swing();

			StopCoroutine("Swing"); // Coroutine ���� �Լ�
			StartCoroutine("Swing"); // Coroutine ���� �Լ�
		}
		else if (type == Type.Range && curAmmo > 0)
		{
			curAmmo--;
			StartCoroutine("Shot");
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

	private IEnumerator Shot()
	{
		// 1. �Ѿ� ����, �Ѿ� �߻�
		GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
		Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
		bulletRigid.velocity = bulletPos.forward * 50f; // bulletPos�� �� �������� �ӵ� ����
		yield return null;

		// 2. ź�� ����
		GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
		Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
		Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
		caseRigid.AddForce(caseVec, ForceMode.Impulse); // ź�� ����
		caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // ź�� ȸ��
	}

	// �Ϲ� �Լ� : Use() ���η�ƾ -> Swing() �Լ� ȣ��, ���� ��ƾ -> Use() ���� ��ƾ
	// Coroutine : Use() ���η�ƾ + Swing() �ڷ�ƾ(Co-Op)
}
