using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UiDisplay : MonoBehaviour

{
    // Start is called before the first frame update

   
    [SerializeField] Slider _currentSlider;

    [SerializeField] float maxValue = 100f;
    public FloatVariable SliderValue;
    

    private float SliderUpdate;
   

    void Start()
    {
        _currentSlider.maxValue = maxValue;
        SliderUpdate = SliderValue.Value;
        SetAndDisplaySlider();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(SliderValue.Value != SliderUpdate)
        {
            SliderUpdate = SliderValue.Value;
            SetAndDisplaySlider();
        }
        
    }

   void SetAndDisplaySlider()
    {
        
        
            _currentSlider.value =  SliderUpdate;
          
        
       
    }

}




   
