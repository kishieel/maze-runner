using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsController : MonoBehaviour
{
	public AudioSource walk;
	public AudioSource run;
	public AudioSource jump;

	private PlayerController playerController;
	private AudioSource current;

    void Start()
    {
		playerController = GetComponent<PlayerController>();
    }

    void Update()
    {

		// if ( playerController.isGrounded ) {
		//
		// 	if ( Input.GetButton("Forward") ) {
		// 		moveState = MoveState.Running;
		// 	}
		//
		// 	if ( Input.GetButton("Sideward") || ( Input.GetButton("Forward") && Input.GetButton("Walk") ) ) {
		// 		moveState = MoveState.Walking;
		// 	}
		//
		// 	if ( Input.GetButton("Jump") ) {
		// 		moveState = MoveState.Jumping;
		// 	}
		//
		// }

		MoveState moveState = playerController.moveState;

		if ( moveState == MoveState.Running ) {
			if ( run.isPlaying == false ) {
				run.Play();
			}
		} else if ( moveState == MoveState.Walking ) {
			if ( walk.isPlaying == false ) {
				walk.Play();
			}
		} else if ( moveState == MoveState.Jumping ) {
			if ( jump.isPlaying == false ) {
				jump.Play();
			}
		}

		if ( ( moveState != MoveState.Running || playerController.isGrounded == false ) && run.isPlaying == true ) {
			run.Stop();
		}
		if ( ( moveState != MoveState.Walking || playerController.isGrounded == false ) && walk.isPlaying == true ) {
			walk.Stop();
		}

		// if ( playerController.isGrounded ) {
		//
		// 	if ( playerController.)
		//
		// }
		//
		// if ( playerController.isGrounded && ( Input.GetButton("Forward") || Input.GetButton("Sideward") ) && Input.GetButton("Walk") ) {
		// 	if ( walk.isPlaying == false ) {
		// 		walk.Play();
		// 	}
		// } else {
		// 	walk.Stop();
		// }
		//
		// if ( playerController.isGrounded && ( Input.GetButton("Forward") || Input.GetButton("Sideward") ) && !Input.GetButton("Walk") ) {
		// 	if ( run.isPlaying == false ) {
		// 		run.Play();
		// 	}
		// } else {
		// 	Debug.Log("STOJU");

		// }

		// if ( run.isPlaying == false && ( Input.GetButton("Forward") || Input.GetButton("Sideward") ) ) {
		// 	PlaySource(run);
		// } else {
		// 	StopSource(run);
		// }
    }

	void PlaySource(AudioSource source) {
		if ( source != null ) {
			source.volume = Random.Range(0.8f, 1f);
			source.pitch = Random.Range(0.8f, 1.2f);
			source.Play();
		}
	}

	void StopSource(AudioSource source) {
		if ( source != null ) {
			source.Stop();
		}
	}
}
