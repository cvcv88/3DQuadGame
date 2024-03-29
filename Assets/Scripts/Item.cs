using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon }; // 열거형
    public Type type; // 아이템 종류
    public int value; // 값을 저장할 변수

	private void Update()
	{
		transform.Rotate(Vector3.up * 20 * Time.deltaTime); // 동전 회전효과
	}
}
