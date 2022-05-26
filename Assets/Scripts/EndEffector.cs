using UnityEngine;
using UnityEngine.UI;
using RTG;

public class EndEffector : MonoBehaviour
{
    /* 预制件 */
    public Slider[] sliders;    // 移动末端执行器需要通过控制Slider
    public GameObject Base;     // 机械臂基座
    public GameObject cameraMain;       // 相机对象


    /* 成员变量 */
    private ObjectTransformGizmo objectUniversalGizmo;  // 附加在末端执行器上的Gizmo控制组件
    private GameObject targetObject;    // 鼠标指向的对象
    private GameObject objectEndEffector;    // 末端执行器游戏对象
    private Transform transBase;        // 机械臂基座的位姿
    private SliderControl[] sliderControls;    // Slider类控制对象的集合

    private void Start()
    {
        /* 初始化Gizmo控件 */
        objectUniversalGizmo = RTGizmosEngine.Get.CreateObjectUniversalGizmo();
        objectUniversalGizmo.Gizmo.SetEnabled(false);
        objectUniversalGizmo.SetTransformSpace(GizmoSpace.Local);
        objectUniversalGizmo.Gizmo.UniversalGizmo.LookAndFeel3D.SetScMidCapVisible(false);

        /* 初始化成员变量 */
        objectEndEffector = gameObject;
        transBase = Base.transform;
        sliderControls = new SliderControl[6];
        for (int i = 0; i < 6; i++)
        {
            sliderControls[i] = sliders[i].GetComponent<SliderControl>();
        }
    }

    private void Update()
    {
        /* 处理鼠标左键点击事件 */
        if (RTInput.WasLeftMouseButtonPressedThisFrame() &&
            RTGizmosEngine.Get.HoveredGizmo == null)
        {
            GameObject pickedObject = PickGameObject();     // 获取鼠标指向的对象
            if (pickedObject != targetObject) OnTargetObjectChanged(pickedObject);      // 修改鼠标指向的对象
        }

    }

    private GameObject PickGameObject()
    {
        // 根据鼠标的位置新建一个Ray对象
        Ray ray = Camera.main.ScreenPointToRay(RTInput.MousePosition);

        // 返回与ray产生相交的对象
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, float.MaxValue))
        {
            GameObject objectHit = rayHit.collider.gameObject;
            return objectHit;
            // if (objectHit == objectEndEffector)
            // {
            //     return objectHit;
            // }
        }

        return null;    // 没有相交的对象
    }

    private void OnTargetObjectChanged(GameObject newTargetObject)
    {
        /* 处理事件：鼠标指向的物体有变化 */

        // 存储新指向的对象
        targetObject = newTargetObject;

        // 新对象是否是存在的游戏对象
        if (targetObject != null)
        {
            // 给新对象添加Gizmo控件
            objectUniversalGizmo.SetTargetObject(targetObject);
            // 启用Gizmo控件
            objectUniversalGizmo.Gizmo.SetEnabled(true);
        }
        else
        {
            // 当新对象是空时，禁用Gizmo控件
            objectUniversalGizmo.Gizmo.SetEnabled(false);
        }
    }

    public void moveEndEffector(Vector3 position, Quaternion rotation)
    {
        /* 获取当前的关节角 */
        double[] jointAnglesCur = new double[6];
        for (int i = 0; i < 6; i++)
        {
            double jointAngle = sliderControls[i].jointAngleCur.z;
            jointAnglesCur[i] = jointAngle;
        }

        /* 计算距离当前关节角最近的逆运动学解 */
        var solution = Algorithm.IkNearest(position, rotation, transBase, jointAnglesCur);

        /* 通过控制Slider来应用逆运动学解 */
        if (solution.Count > 0)
        {

            float[] slidersValues = new float[6];   // 存储需要设置的slider.value

            /* 得到的逆运动学解不一定在工作空间内，需要进行判断 */
            for (int i = 0; i < solution.Count; i++)
            {
                double angleZero = sliderControls[i].jointAngleZero.z;
                slidersValues[i] = (float)(solution[i] - angleZero);
                if (slidersValues[i] > sliders[i].maxValue || slidersValues[i] < sliders[i].minValue)
                {
                    /* 当需要设置的slider.value超出了slider的上下限时即超出了工作空间 */
                    Debug.Log("逆运动学解超出工作空间，无法移动！");
                    Debug.Log($"当前逆运动学解: ({slidersValues[0]}, {slidersValues[1]}, {slidersValues[2]}, {slidersValues[3]}, {slidersValues[4]}, {slidersValues[5]})");
                    return;
                }
            }
            /* 确认所有关节变量都在允许的范围内才进行设置 */
            for (int i = 0; i < 6; i++)
            {
                sliders[i].value = slidersValues[i];
            }
        }
        else
        {
            Debug.Log("逆运动学无解！");
        }
    }

    public void moveInCameraTrans(Vector3 deltaPositions, Vector3 deltaAngles)
    {
        /* 将末端执行器位姿转换到相机坐标系下 */
        Transform transCamera = cameraMain.transform;
        Vector3 position = transCamera.InverseTransformPoint(objectEndEffector.transform.position);
        Quaternion rotation = Quaternion.Inverse(transCamera.rotation) * objectEndEffector.transform.rotation;
        Vector3 angles = rotation.eulerAngles;

        /* 运动增量 */
        position += deltaPositions;
        angles += deltaAngles;
        rotation.eulerAngles = angles;

        /* 将末端执行器位姿从相机坐标系转换回世界坐标系 */
        position = transCamera.TransformPoint(position);
        rotation = transCamera.rotation * rotation;

        moveEndEffector(position, rotation);
    }

    public void moveInLocalTrans(Vector3 deltaPositions, Vector3 deltaAngles)
    {
        /* 获取末端执行器坐标系下的位姿 */
        Transform transEndEffector = objectEndEffector.transform;
        Vector3 position =  deltaPositions;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = deltaAngles;

        /* 将位姿从末端执行器坐标系转换到世界坐标系 */
        position = transEndEffector.TransformPoint(position);
        rotation = transEndEffector.rotation * rotation;

        moveEndEffector(position, rotation);
    }
}