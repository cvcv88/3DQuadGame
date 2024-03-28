using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
	bool jDown;

	bool isJump;
	bool isDodge;

    Vector3 moveVec;

	Rigidbody rigid;
    Animator anim;

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
		Dodge();
	}

    private void GetInput()
    {
		hAxis = Input.GetAxisRaw("Horizontal");
		vAxis = Input.GetAxisRaw("Vertical");
		wDown = Input.GetButton("Walk");
		jDown = Input.GetButton("Jump");
	}

    private void Move()
    {
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;

		transform.position += moveVec * speed * Time.deltaTime * (wDown ? 0.5f : 1f);

		anim.SetBool("isRun", moveVec != Vector3.zero);
		anim.SetBool("isWalk", wDown);
	}

	private void Turn()
	{
		transform.LookAt(transform.position + moveVec);
	}

	private void Jump()
	{
		if (jDown && !isJump)
		{
			rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
			anim.SetBool("isJump", true);
			anim.SetTrigger("doJump");
			isJump = true;
		}
	}

	private void Dodge()
	{
		if (jDown && !isDodge)
		{
			speed *= 2;
			anim.SetBool("isJump", true);
			anim.SetTrigger("doJump");
			isJump = true;
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
}
