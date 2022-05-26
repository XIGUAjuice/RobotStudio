using UnityEngine.UI;
using UnityEngine;

public class RobotReal : MonoBehaviour
{
    /* 预制件 */
    public Slider[] sliders;
    public GameObject[] joints;
    public Toggle toggle;

    /* 成员变量 */
    private float speed;
    private float step;
    private SliderControl[] sliderControls;    // Slider类控制对象的集合
    private Vector3[] jointAngles;
    private bool exec = false;
    void Start()
    {
        speed = 100;
        step = 0.01f * speed;
        sliderControls = new SliderControl[6];
        jointAngles = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            sliderControls[i] = sliders[i].GetComponent<SliderControl>();
            jointAngles[i] = sliderControls[i].jointAngleZero;
        }
    }
    void Update()
    {
        if (exec)
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
                if(jointAngles[i].z != sliderControls[i].jointAngleCur.z)
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
}
