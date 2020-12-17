using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = (float)1;
    public float curHealth = (float)1;

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        fill.color = gradient.Evaluate(1f);
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = player.GetComponent<Player>().maxHealth;
        curHealth = player.GetComponent<Player>().health;
        //Debug.Log(curHealth);

        slider.value = curHealth;
        slider.maxValue = maxHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
