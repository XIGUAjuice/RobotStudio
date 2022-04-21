using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderReset : MonoBehaviour
{   
    public Slider[] sliders;

    public void onClick()
    {   
        /* 复位 */
        foreach(Slider slider in sliders)
        {
            slider.value = 0;
        }
    }
}
