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
			Destroy(gameObject, 3f); // 닿은 것이 floor면 3초 뒤에 없애기
		}
		else if(collision.gameObject.tag == "Wall")
		{
			Destroy(gameObject); // 닿은 것이 Wall이면 바로 없애기
		}
	}
}
