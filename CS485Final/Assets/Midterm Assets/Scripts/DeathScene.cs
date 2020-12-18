using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScene : MonoBehaviour
{
	private int origin_x;
	private int origin_y;
	public int bWidth;
	public int bHeight;
	public GUISkin mySkin;
  
  
    // Start is called before the first frame update
    void Start()
    {
        bWidth = 180;
        bHeight = 40;
        origin_x = Screen.width / 2 - bWidth / 2;
        origin_y = Screen.height / 2 - bHeight * 2;
    }

   void OnGUI(){
   	GUI.skin = mySkin;
   	if(GUI.Button(new Rect(origin_x,origin_y,bWidth,bHeight),"Menu"))
   	{
   		Application.LoadLevel(0);
   	}
   	if(GUI.Button(new Rect(origin_x,origin_y + bHeight + 20,bWidth,bHeight),"Quit"))
   	{
   		#if UNITY_EDITOR
   			UnityEditor.EditorApplication.isPlaying = false;
   		#else
   			Application.Quit();
   		#endif
   	}
   }
}
