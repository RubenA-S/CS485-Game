using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SimpleEvents : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent event1;
    public UnityEngine.Events.UnityEvent event2;
    public UnityEngine.Events.UnityEvent event3;
    public UnityEngine.Events.UnityEvent event4;

    public Transform obj0;
    public Transform obj1;
    public Transform obj2;
    public Transform obj3;
    public Transform obj4;
    
    
    
    // Update is called once per frame
    void Update()
    {
    	if(obj1!=null&&obj0!=null)
    	{
        	float dist1 = Vector3.Distance(obj0.position, obj1.position);
        		if (dist1 < 8)
           			event1.Invoke();
        }
        if(obj2!=null&&obj0!=null)
    	{
           	float dist2 = Vector3.Distance(obj0.position, obj2.position);
        		if (dist2 < 8)
           			event2.Invoke();
        }
        if(obj3!=null&&obj0!=null)
    	{
           	float dist3 = Vector3.Distance(obj0.position, obj3.position);
        		if (dist3 < 8)
           			event3.Invoke();
        }
        if(obj4!=null&&obj0!=null)
    	{
           	float dist4 = Vector3.Distance(obj0.position, obj4.position);
        		if (dist4 < 8)
           			event4.Invoke();
        }
           	
    }
}
