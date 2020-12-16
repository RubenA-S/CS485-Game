using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktoMenu : MonoBehaviour
{
	public GUISkin mySkin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnGUI()
    	{
    		GUI.skin = mySkin;
    	  if(GUI.Button(new Rect(20,20,180,40),"Menu"))
    	 	 {Application.LoadLevel(0);}	
    	}
}
