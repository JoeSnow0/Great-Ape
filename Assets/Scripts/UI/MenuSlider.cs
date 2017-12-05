using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour {
    Slider slider;
    Text sliderText;

	void Start () {
        slider = GetComponent<Slider>();
        sliderText = GetComponentInChildren<Text>();

        SetText(slider.value.ToString());
    }
	
	void Update () {
		
	}

    public void OnValueChange()
    {
        SetText(slider.value.ToString());
    }
    private void SetText(string text)
    {
        sliderText.text = text;
    }
}