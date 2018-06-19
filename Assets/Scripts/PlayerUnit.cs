using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {

	[Header("Movement")]
	[SerializeField]
	float speed = 4f;
	[SerializeField]
	float speedMultiplier = 2f; // increase speed multiplier while sprinting
	float tempSpeed; 
	[SerializeField]
    float jumpSpeed = 8.0F;
	[SerializeField]
    float gravity = 20.0F;
	[SerializeField]
	[Header("Head Vision")]
	GameObject head;
	float rotationX;
	float rotationY;
	float moveZ;
	float moveX;
    Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	
	[SerializeField]
	float sensitivity = 1.0f;
	[SerializeField]
	float smoothing = 2.0f;
	Vector2 mouseLook;
	Vector2 smoothV;
	GameObject character;

	void Awake()
    {
		controller = GetComponent<CharacterController>();
		character = this.gameObject;
	}

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

    void Update() {
		if( hasAuthority == false )
		{
			return;
		}
        Move();
		MoveHead();
    }

	// Update is called once per frame
	void MoveHead () {
		var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -80f, 80f); 

		head.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		//head.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}


	// Player Movement
	void Move()
    {
		tempSpeed = speed; //this rests our speed back to natural speed

		if (controller.isGrounded) {
			if(Input.GetKey(KeyCode.LeftShift)) 
			{
				tempSpeed *= speedMultiplier; //this increases our speed by the multiplier
			}
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= tempSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
	}

	// Push Power
	public float pushPower = 2.0F;
    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

}
