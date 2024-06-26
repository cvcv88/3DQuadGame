using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed;
	public GameObject[] weapons;
	public bool[] hasWeapons;
	public GameObject[] grenades;
	public int hasGrenades;
	public Camera followCamera;

	public int ammo;
	public int coin;
	public int health;

	public int maxAmmo;
	public int maxCoin;
	public int maxHealth;
	public int maxHasGrenades;

	float hAxis;
	float vAxis;

	bool wDown; // Walk
	bool jDown; // Jump
	bool iDown; // Interaction
	bool fDown; // Fire, Coroutine 기본공격
	bool rDown; // Reload 재장전

	bool sDown1;
	bool sDown2;
	bool sDown3;

	bool isJump;
	bool isDodge;
	bool isSwap;
	bool isReload;
	bool isFireReady = true;

	Vector3 moveVec;
	Vector3 dodgeVec;

	Rigidbody rigid;
	Animator anim;

	GameObject nearObject;
	// GameObject equipWeapon;
	Weapon equipWeapon;

	int equipWeaponIndex = -1;

	float fireDelay; // Fire 기본공격 딜레이

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		GetInput();
		Move();
		Turn();
		Jump();
		Attack();
		Reload();
		Dodge();
		Swap();
		Interaction();
	}

	private void GetInput()
	{
		hAxis = Input.GetAxisRaw("Horizontal");
		vAxis = Input.GetAxisRaw("Vertical");
		wDown = Input.GetButton("Walk");
		jDown = Input.GetButtonDown("Jump");
		fDown = Input.GetButton("Fire1");
		rDown = Input.GetButtonDown("Reload");
		iDown = Input.GetButtonDown("Interaction");
		sDown1 = Input.GetButtonDown("Swap1");
		sDown2 = Input.GetButtonDown("Swap2");
		sDown3 = Input.GetButtonDown("Swap3");
	}

	private void Move()
	{
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;
		if (isDodge) // 지금 회피를 하고 있으면
			moveVec = dodgeVec;

		if (isSwap || !isFireReady || isReload) // 무기 변경 or 공격 or 재장전 중에는 이동 불가
			moveVec = Vector3.zero;

		transform.position += moveVec * speed * Time.deltaTime * (wDown ? 0.5f : 1f);

		anim.SetBool("isRun", moveVec != Vector3.zero);
		anim.SetBool("isWalk", wDown);
	}

	private void Turn()
	{
		// 1. 키보드에 의한 회전
		transform.LookAt(transform.position + moveVec);

		// 2. 마우스에 의한 회전
		if (fDown)
		{
			Ray ray = followCamera.ScreenPointToRay(Input.mousePosition); // ScreenPointToRay() : 스크린에서 월드로 Ray를 쏘는 함수
			RaycastHit rayHit;
			if (Physics.Raycast(ray, out rayHit, 100)) // out : return 처럼 반환값을 주어진 변수에 저장하는 키워드
													   // 쏘는 로직, ray의 길이 : 100
			{
				Vector3 nextVec = rayHit.point - transform.position; // rayHit.point : ray가 닿았던 지점
				nextVec.y = 0; // y축이 있으면 부피가 있는 콜라이더에 마우스를 가져다 대면 캐릭터 기울어짐, 0으로 설정해주자
				transform.LookAt(transform.position + nextVec);
			}
		}
	}

	private void Jump()
	{
		if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
		{
			rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
			anim.SetBool("isJump", true);
			anim.SetTrigger("doJump");
			isJump = true;
		}
	}

	private void Attack()
	{
		if (equipWeapon == null) // 손에 아무것도 없으면
			return;

		// 공격딜레이에 시간 더해주고 공격 가능 여부 확인
		fireDelay += Time.deltaTime;
		isFireReady = equipWeapon.rate < fireDelay;

		if (fDown && isFireReady && !isDodge && !isSwap)
		{
			equipWeapon.Use();
			anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
			fireDelay = 0;
		}
	}

	private void Reload()
	{
		if (equipWeapon == null) return;
		if (equipWeapon.type == Weapon.Type.Melee) return;
		if (ammo == 0) return;

		if (rDown && !isJump && !isDodge && !isSwap && isFireReady)
		{
			anim.SetTrigger("doReload");
			isReload = true;

			Invoke("ReloadOut", 2f);
		}
	}

	private void ReloadOut()
	{
		int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo; // 갖고있는 탄창이 최대 탄창보다 작으면 갖고있는 탄창만큼 재장전,
																			  // 최대 탄창이면 그대로 최대탄창만큼 재장전
		equipWeapon.curAmmo = equipWeapon.maxAmmo;
		ammo -= reAmmo;
		isReload = false;
	}


	private void Dodge()
	{
		if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
		{
			dodgeVec = moveVec;
			speed *= 2;
			anim.SetTrigger("doDodge");
			isDodge = true;

			Invoke("DodgeOut", 0.4f);
		}
	}

	private void DodgeOut()
	{
		speed *= 0.5f;
		isDodge = false;
	}

	private void Swap()
	{
		if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
			return;
		if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
			return;
		if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
			return;

		int weaponIndex = -1;
		if (sDown1) weaponIndex = 0;
		if (sDown2) weaponIndex = 1;
		if (sDown3) weaponIndex = 2;

		if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
		{
			if (equipWeapon != null)
				equipWeapon.gameObject.SetActive(false);

			equipWeaponIndex = weaponIndex;
			equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
			equipWeapon.gameObject.SetActive(true);

			anim.SetTrigger("doSwap");

			isSwap = true;

			Invoke("SwapOut", 0.4f);
		}
	}
	private void SwapOut()
	{
		isSwap = false;
	}

	private void Interaction()
	{
		if (iDown && nearObject != null && !isJump && !isDodge)
		{
			if (nearObject.tag == "Weapon")
			{
				Item item = nearObject.GetComponent<Item>();
				int weaponIndex = item.value;
				hasWeapons[weaponIndex] = true;

				Destroy(nearObject);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Floor")
		{
			anim.SetBool("isJump", false);
			isJump = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Item")
		{
			Item item = other.GetComponent<Item>();
			switch (item.type)
			{
				case Item.Type.Ammo:
					ammo += item.value;
					if (ammo > maxAmmo)
						ammo = maxAmmo;
					break;
				case Item.Type.Coin:
					coin += item.value;
					if (coin > maxCoin)
						coin = maxCoin;
					break;
				case Item.Type.Heart:
					health += item.value;
					if (health > maxHealth)
						health = maxHealth;
					break;
				case Item.Type.Grenade:
					grenades[hasGrenades].SetActive(true);
					hasGrenades += item.value;
					if (hasGrenades > maxHasGrenades)
					{
						hasGrenades = maxHasGrenades;
					}
					break;
			}
			Destroy(other.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Weapon")
			nearObject = other.gameObject;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Weapon")
			nearObject = null;
	}
}
