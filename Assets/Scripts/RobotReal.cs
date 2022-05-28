using UnityEngine.UI;
using UnityEngine;

public class RobotReal : MonoBehaviour
{
    /* 预制件 */
    public Slider[] sliders;
    public GameObject[] joints;
    public Toggle toggle;
    public Button stop;

    /* 成员变量 */
    private float speed;
    private float step;
    private SliderControl[] sliderControls;    // Slider类控制对象的集合
    private Vector3[] jointAngles;
    private bool exec = false;
    private bool trace = false;
    private bool danger = false;
    private Color32 colorEnable;        // 表示启用的颜色
    private Color32 colorDisable;       // 表示禁用的颜色
    void Start()
    {
        speed = 60;
        step = 0.01f * speed;
        sliderControls = new SliderControl[6];
        jointAngles = new Vector3[6];

        colorEnable = new Color32(142, 243, 70, 173);
        colorDisable = new Color32(233, 28, 51, 154);

        for (int i = 0; i < 6; i++)
        {
            sliderControls[i] = sliders[i].GetComponent<SliderControl>();
            jointAngles[i] = sliderControls[i].jointAngleZero;
        }
    }
    void Update()
    {
        if (trace)
        {
            exec = true;
        }

        if (exec && !danger)
        {
            for (int i = 0; i < 6; i++)
            {
                var joint = joints[i];
                var sliderControl = sliderControls[i];
                if (jointAngles[i].z < sliderControls[i].jointAngleCur.z)
                {
                    if (sliderControls[i].jointAngleCur.z - jointAngles[i].z < step)
                    {
                        jointAngles[i].z = sliderControls[i].jointAngleCur.z;
                    }
                    else
                    {
                        jointAngles[i].z += step;
                    }
                }
                else if (jointAngles[i].z > sliderControls[i].jointAngleCur.z)
                {
                    if (jointAngles[i].z - sliderControls[i].jointAngleCur.z < step)
                    {
                        jointAngles[i].z = sliderControls[i].jointAngleCur.z;
                    }
                    else
                    {
                        jointAngles[i].z -= step;
                    }
                }

                var rotation = joint.transform.localRotation;
                rotation.eulerAngles = jointAngles[i];
                joint.transform.localRotation = rotation;
            }

            for (int i = 0; i < 6; i++)
            {
                if (jointAngles[i].z != sliderControls[i].jointAngleCur.z)
                {
                    exec = true;
                    break;
                }
                else
                {
                    exec = false;
                }
            }
        }
    }

    public void onExecClick()
    {
        exec = true;
    }

    public void onToggleChanged()
    {
        gameObject.SetActive(toggle.isOn);
    }

    public void onStopClick()
    {
        danger = !danger;
        if(danger)
        {
            stop.image.color = colorDisable;
        }
        else
        {
            stop.image.color = colorEnable;
        }
    }

    public void onDropdownChanged(int value)
    {
        switch (value)
        {
            case 0:
                trace = false;
                Debug.Log("切换到规划模式");
                break;
            case 1:
                trace = true;
                Debug.Log("切换到跟随模式");
                break;
        }
    }
}
