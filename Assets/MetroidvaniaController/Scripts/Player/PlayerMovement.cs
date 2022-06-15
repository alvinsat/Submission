using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Actor controller;
	public Animator anim;

	public float runSpeed = 40f;

	private float horizontalMove = 0f;
	private float verticalMove = 0f;
	private bool jump = false;
	private bool dash = false;

	PlatformerControl currentPlatform;

	//bool dashAxis = false;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical");

		anim.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Z))
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			dash = true;
		}

		/*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
		{
			if (dashAxis == false)
			{
				dashAxis = true;
				dash = true;
			}
		}
		else
		{
			dashAxis = false;
		}
		*/

	}

	public void OnFall()
	{
		anim.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		anim.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Platformer")) {
			currentPlatform = other.GetComponent<PlatformerControl>();
		}
    }
    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag("Platformer")) {
			currentPlatform = null;
		}
    }
}
