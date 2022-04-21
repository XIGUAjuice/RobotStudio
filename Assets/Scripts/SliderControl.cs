using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderControl : MonoBehaviour
{   
    /* 预制件 */
    public GameObject joint;        // 关联的关节对象
    public Vector3 jointAngleZero;  // 初始状态的关节欧拉角

    /* 成员变量 */
    public Vector3 jointAngleCur;   // 设定的目标关节欧拉角
    private Quaternion rotationCur; // 设定的目标关节四元数
    private Slider slider;          // 关联的滑动条
    private TMP_InputField input;   // 关联的输入框

    void Start()
    {   
        /* 初始化成员变量 */
        jointAngleCur = jointAngleZero;
        slider = gameObject.GetComponent<Slider>();
        input = slider.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
    }

    public void updateJoint()
    {   
        /* 更新关节角 */
        jointAngleCur.z = jointAngleZero.z + slider.value;  // 根据滑动条的值设定目标关节角

        /* 将关节角转换为四元数，统一用四元数进行设置 */
        rotationCur.eulerAngles = jointAngleCur;            
        joint.transform.localRotation = rotationCur;
    }

    public void onSliderChange(float value)
    {   
        /* 处理事件: 滑动条变动 */
        // 所有更改关节角的方法均通过修改滑动条来实现
        input.text = value.ToString("0");
        updateJoint();
    }

    public void OnInputFieldChanged(string newText)
    {   
        /* 处理事件: 输入框变动 */

        /* 检查是否是数字 */
        if (float.TryParse(newText, out var value))
        {
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue); // 对超出上下限的数字进行修剪
            input.text = value.ToString("0");   // 更改输入框
            slider.value = value;               // 更改滑动条
        }
        else
        {   
            /* 输入格式有误 */
            Debug.LogWarning("Input Format Error!", this);
            slider.value = Mathf.Clamp(0, slider.minValue, slider.maxValue);
            input.text = slider.value.ToString("0");
        }
    }
}
