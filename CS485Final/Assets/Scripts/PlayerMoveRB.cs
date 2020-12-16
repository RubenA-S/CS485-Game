using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRB : MonoBehaviour
{
	public float speed;
	private Vector3 direction;
	private Rigidbody rbody;
	public float jHeight;
	public LayerMask ground;
	public Transform feet;
	private int dJump = 0;
	private float rspeed;
	private float rotX;
	private float rotY;
    // Start is called before the first frame update
    void Start()
    {
        speed = 8.0f;
        jHeight = 5.0f;
        rspeed = 1f;
        rotX = 0;
        rotY = 10f;
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction = direction.normalized;
        if(direction.x != 0)
        {
        	//transform.forward = direction;
        	//rbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
        	rbody.MovePosition(rbody.position + transform.right * direction.x * speed * Time.deltaTime);
        }
        if(direction.z != 0)
        {
        	//transform.forward = direction;
        	//rbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
        	rbody.MovePosition(rbody.position + transform.forward * direction.z * speed * Time.deltaTime);
        }
        rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rspeed;
        rotY += Input.GetAxis("Mouse Y") * rspeed;
        transform.localEulerAngles = new Vector3(-rotY,rotX,0);
        
        bool isGrounded()
        {
        	if(Physics.CheckSphere(feet.position, .1f,ground))
        	{
        		dJump = 0;
        		return true;
        	}
        	else
        	{return false;}
        }
        if(Input.GetButtonDown("Jump") && (isGrounded() || dJump < 2))
        {
        	dJump += 1;
        	rbody.AddForce(Vector3.up * jHeight, ForceMode.VelocityChange);
        }
    }
}
