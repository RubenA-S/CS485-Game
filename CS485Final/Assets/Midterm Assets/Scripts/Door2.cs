using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
   	 {
     	if(other.tag == "Player")
        {
            Application.LoadLevel(3);
        }
    }
}