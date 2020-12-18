using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public float maxExp = (float)1;
    public float Exp = (float)1;

    public Slider slider;
    public Image fill;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        maxExp = player.GetComponent<Player>().maxExp;
        Exp = player.GetComponent<Player>().Exp;
        //Debug.Log(curHealth);

        slider.value = Exp;
        slider.maxValue = maxExp;
    }
}
