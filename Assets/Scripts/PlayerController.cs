using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { Idle, Walking, Running, Jumping };

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 10f;
	public float rotationSpeed = 10f;
	public float jumpHeight;
	public bool isGrounded = false;

	public Transform boneHead;
	public Transform boneEyes;

	public MoveState moveState;

	private float animatorSpeed = 0f;
	private Vector3 headRotation = Vector3.zero;

	private Rigidbody rbody;
	private Animator animator;
	private Camera cam;

    void Awake() {
		rbody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		cam = GetComponentInChildren<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

	void OnDestroy() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

    void Update() {
		animatorSpeed = 0f;
		moveState = MoveState.Idle;
		if ( Input.GetButton("Forward") || Input.GetButton("Sideward") ) {
			moveState = MoveState.Running;
			animatorSpeed = 1f;

			float axisForward = Input.GetAxis("Forward");
			float axisSideward = Input.GetAxis("Sideward");

			axisForward = Mathf.Clamp(axisForward, -0.5f, 1f);
			if ( axisForward < 0 ) {
				moveState = MoveState.Walking;
				animatorSpeed = 0.5f;
			}

			if ( Input.GetButton("Walk") ) {
				moveState = MoveState.Walking;
				axisForward /= 2f;
				axisSideward /= 2f;
				animatorSpeed = 0.5f;
			}

			Vector3 moveDirection = Vector3.zero;
			moveDirection.x += transform.forward.x * axisForward + transform.right.x * axisSideward;
            moveDirection.z += transform.forward.z * axisForward + transform.right.z * axisSideward;

			rbody.MovePosition( rbody.position + moveDirection * moveSpeed * Time.deltaTime );
		}
		animator.SetFloat("speed", animatorSpeed, .1f, Time.deltaTime);

		isGrounded = Physics.Raycast(transform.position, Vector3.down, .2f);
		if (Input.GetButtonDown("Jump") && isGrounded) {
			moveState = MoveState.Jumping;
			Vector3 jumpForce = Vector3.up;
			jumpForce *= Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
			rbody.AddForce(jumpForce, ForceMode.VelocityChange);
			animator.Play("Jump");
		}
    }

	void LateUpdate() {
		float axisHorizontal = Input.GetAxis("Horizontal") * rotationSpeed;
		transform.Rotate(0, axisHorizontal * Time.deltaTime, 0);

		float axisVertical = Input.GetAxis("Vertical") * rotationSpeed;
		headRotation.x -= axisVertical * Time.deltaTime;
		headRotation.x = Mathf.Clamp(headRotation.x, -90f, 45f);
		boneHead.transform.localEulerAngles = headRotation;

		cam.gameObject.transform.position = boneEyes.transform.position;
		cam.gameObject.transform.localEulerAngles = new Vector3(headRotation.x, 0, 0);
	}
}
