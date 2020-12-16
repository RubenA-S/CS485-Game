using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   	private CharacterController controller;
    public float speed;
    public float gravity;
    public float jheight;
    private Vector3 direction;
    private Vector3 walkingV;
    private Vector3 fallingV;

    private void Start()
    {
    	speed = 5.0f;
    	gravity = 9.8f;
    	jheight = 3.0f;
    	direction = Vector3.zero;
    	walkingV = Vector3.zero;
    	fallingV = Vector3.zero;
    	controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       direction.x = Input.GetAxis("Horizontal");
       direction.z = Input.GetAxis("Vertical");
       direction = direction.normalized;
       if(direction != Vector3.zero){
       transform.forward = direction;}
       walkingV = direction * speed;
       controller.Move(walkingV*Time.deltaTime);
       fallingV.y -= gravity*Time.deltaTime;

       if(Input.GetButtonDown("Jump"))
       {
       	fallingV.y = Mathf.Sqrt(gravity * jheight);
       }
       controller.Move(fallingV*Time.deltaTime);
    }
}
