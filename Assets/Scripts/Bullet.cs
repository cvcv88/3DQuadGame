using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Floor")
		{
			Destroy(gameObject, 3f); // ���� ���� floor�� 3�� �ڿ� ���ֱ�
		}
		else if(collision.gameObject.tag == "Wall")
		{
			Destroy(gameObject); // ���� ���� Wall�̸� �ٷ� ���ֱ�
		}
	}
}
