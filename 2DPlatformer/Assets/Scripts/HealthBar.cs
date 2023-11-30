using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateHealth(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
