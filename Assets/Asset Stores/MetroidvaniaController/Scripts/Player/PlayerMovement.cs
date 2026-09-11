using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;

	[SerializeField] InputActionReference moveAction;
	[SerializeField] InputActionReference jumpAction;
	[SerializeField] InputActionReference dashAction;

	void OnEnable()
	{
		moveAction.action.performed += OnMove;
		moveAction.action.canceled += OnMove;
		jumpAction.action.performed += OnJump;
		dashAction.action.performed += OnDash;

		moveAction.action.Enable();
		jumpAction.action.Enable();
		dashAction.action.Enable();
	}

	void OnDisable()
	{
		moveAction.action.performed -= OnMove;
		moveAction.action.canceled -= OnMove;
		jumpAction.action.performed -= OnJump;
		dashAction.action.performed -= OnDash;

		moveAction.action.Disable();
		jumpAction.action.Disable();
		dashAction.action.Disable();
	}

	void OnMove(InputAction.CallbackContext context)
	{
		horizontalMove = context.ReadValue<float>() * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
	}

	void OnJump(InputAction.CallbackContext context)
	{
		jump = true;
	}

	void OnDash(InputAction.CallbackContext context)
	{
		dash = true;
	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}
