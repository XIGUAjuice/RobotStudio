using System;
using System.Text;
using System.Threading;
using WebSocketSharp;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* 实现语音交互的功能 */
class Speech : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /* 讯飞AIUI websocket请求参数 */
    private static string BASE_URL = "ws://wsapi.xfyun.cn/v1/aiui";
    private static string ORIGIN = "http://wsapi.xfyun.cn";
    // 以下信息模糊处理
    private const string APPID = "xxxx";
    private const string APIKEY = "xxxx";
    private const string AUTH_ID = "xxxx";
    private const string SCENE = "main_box";
    private const string DATA_TYPE = "audio";
    private const string SAMPLE_RATE = "16000";
    private const string AUE = "raw";
    private const string RESULT_LEVEL = "complete";
    private const string CLEAN_HISTORY = "user";

    /* 结束数据发送标记 */
    private static string END_FLAG = "--end--";

    /* 构造配置参数 */
    private static string param = $"{{\"auth_id\":\"{AUTH_ID}\", \"scene\":\"{SCENE}\", \"data_type\":\"{DATA_TYPE}\"," +
                                  $"\"sample_rate\":\"{SAMPLE_RATE}\", \"aue\":\"{AUE}\", \"result_level\":\"{RESULT_LEVEL}\"," +
                                  $"\"clean_dialog_history\":\"{CLEAN_HISTORY}\"}}";

    /* 预制件，将需要的对象传入 */
    public GameObject objectEndEffector;  // 受操纵的末端执行器游戏对象 
    public GameObject Base;         // 机械臂基座

    /* 成员变量 */
    private EndEffector endEffector;    // 末端执行器类对象
    private Transform transBase;    // 基座的位姿
    private string direction = string.Empty;  // 机械臂末端下一次移动的方向
    private float distance = 0;     // 机械臂末端下一次移动的距离
    private string axis = string.Empty;     // 机械臂末端下一次旋转的轴
    private float degree = 0;       // 机械臂末端下一次旋转的角度
    private WebSocket server;       // websocket客户端
    private bool hasCartesian = false;  // 是否收到笛卡尔运动的意图
    private bool hasRotation = false;   // 是否收到旋转运动的意图
    private string device;          // 麦克风设备号
    private AudioClip audioClip;    // 录制的音频

    public void Start()
    {
        Debug.Log($"构造请求参数: {param}");
        transBase = Base.transform;     // 获取基座的位姿
        endEffector = objectEndEffector.GetComponent<EndEffector>();    // 获取末端执行器类对象
    }

    public void Update()
    {
        /* 循环检测是否websocket返回语义信息，若有则按指令运动机械臂 */
        if (hasCartesian)   // 笛卡尔运动
        {
            /* 判断方向，在相机坐标系下进行移动 */
            if (direction.Equals("上"))
            {
                endEffector.moveInCameraTrans(new Vector3(0, distance, 0), new Vector3(0, 0, 0));
            }
            else if (direction.Equals("下"))
            {
                endEffector.moveInCameraTrans(new Vector3(0, -distance, 0), new Vector3(0, 0, 0));
            }
            else if (direction.Equals("左"))
            {
                endEffector.moveInCameraTrans(new Vector3(-distance, 0, 0), new Vector3(0, 0, 0));
            }
            else if (direction.Equals("右"))
            {
                endEffector.moveInCameraTrans(new Vector3(distance, 0, 0), new Vector3(0, 0, 0));
            }
            else if (direction.Equals("前"))
            {
                endEffector.moveInCameraTrans(new Vector3(0, 0, distance), new Vector3(0, 0, 0));
            }
            else if (direction.Equals("后"))
            {
                endEffector.moveInCameraTrans(new Vector3(0, 0, -distance), new Vector3(0, 0, 0));
            }
            Debug.Log($"机械臂向{direction}移动了{distance}毫米");

            /* 清除websocket传入的消息 */
            hasCartesian = false;
            direction = string.Empty;
            distance = 0;
        }
        if (hasRotation)     // 旋转运动
        {
            /* 判断旋转轴，在末端执行器坐标系下运动 */
            if (axis.Equals("x"))
            {
                endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(degree, 0, 0));
            }
            else if (axis.Equals("y"))
            {
                endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(0, degree, 0));
            }
            else if (axis.Equals("z"))
            {
                endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(0, 0, degree));
            }
            Debug.Log($"机械臂绕{axis}轴旋转了{degree}度");

            /* 清除websocket传入的消息 */
            hasRotation = false;
            axis = string.Empty;
            degree = 0;
        }
    }

    public static string currentTimeMillis()
    {
        /* 获取时间戳 */
        long currenttimemillis = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        return String.Format("{0}", currenttimemillis / 1000L);
    }

    public static string EncodeBase64(string source)
    {
        /* Base64加密 */
        byte[] bytes = Encoding.UTF8.GetBytes(source);
        return Convert.ToBase64String(bytes);
    }

    public static string SHA256EncryptString(string data)
    {
        /* SHA256加密 */
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            builder.Append(hash[i].ToString("x2"));
        }
        return builder.ToString();
    }

    // 拼接握手参数
    private static string getHandShakeParams()
    {
        string paramBase64 = EncodeBase64(param);
        string curtime = currentTimeMillis();
        string signtype = "sha256";
        string originStr = APIKEY + curtime + paramBase64;
        string checksum = SHA256EncryptString(originStr);
        string handshakeParam = "?appid=" + APPID + "&checksum=" + checksum + "&curtime=" + curtime + "&param=" + paramBase64 + "&signtype=" + signtype;

        return handshakeParam;
    }

    private void serverShakeHand()
    {
        /* 与AIUI服务器握手建立通信 */

        string url = BASE_URL + getHandShakeParams();   //构造请求地址
        Debug.Log($"请求地址为{url}");

        /* 设置客户端参数 */
        server = new WebSocket(url);
        server.EnableRedirection = true;
        server.EmitOnPing = true;
        server.Origin = ORIGIN;

        /* 添加回调函数 */
        server.OnMessage += on_message;
        server.OnOpen += on_open;
        server.OnError += on_error;
        server.OnClose += on_close;

        /* 进行异步连接 */
        server.ConnectAsync();
    }

    private void on_close(object sender, CloseEventArgs e)
    {
        Debug.Log($"on_close: {e.Code} -> {e.Reason}");
    }

    private void on_error(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        Debug.Log($"on_error: {e.Message}");
    }

    private void on_message(object sender, MessageEventArgs e)
    {
        /* 对服务器返回的数据进行解析 */

        string jsonString = e.Data;
        Debug.Log(jsonString);

        Root response = JsonUtility.FromJson<Root>(jsonString);
        if (response.data.sub == "nlp")
        {
            Debug.Log("语义识别成功");
            List<Semantic> semantics = response.data.intent.semantic;
            foreach (var semantic in semantics)
            {
                if (semantic.intent.Equals("cartesian"))
                {
                    Debug.Log("匹配到意图为：笛卡尔运动");
                    hasCartesian = true;
                    List<Slot> slots = semantic.slots;
                    foreach (var slot in slots)
                    {
                        if (slot.name == "direction")
                        {
                            direction = slot.value;
                            Debug.Log("确定方向为" + direction);
                        }
                        else if (slot.name == "distance")
                        {
                            distance = float.Parse(slot.value);
                            Debug.Log("确定距离为" + distance);
                        }
                    }
                }
                else if (semantic.intent.Equals("rotation"))
                {
                    Debug.Log("匹配到意图为：旋转运动运动");
                    hasRotation = true;
                    List<Slot> slots = semantic.slots;
                    foreach (var slot in slots)
                    {
                        if (slot.name.Equals("axis"))
                        {
                            axis = slot.value;
                            Debug.Log("确定旋转轴为" + axis);
                        }
                        else if (slot.name == "degree")
                        {
                            degree = float.Parse(slot.normValue);
                            Debug.Log("确定旋转角为" + degree);
                        }
                    }
                }
            }
        }
    }

    private void on_open(object sender, EventArgs e)
    {
        Debug.Log($"on_open");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /* 处理事件: 鼠标左键按下 */

        serverShakeHand();      // 握手建立通信
        device = Microphone.devices[0];     // 获取麦克风设备
        audioClip = Microphone.Start(device, false, 50, 16000);     // 开始录音
        Debug.Log("device: " + device + " start record.");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /* 处理事件: 鼠标左键弹起 */

        Microphone.End(device);     // 结束录音
        string filepath;
        byte[] bytes = WavUtility.FromAudioClip(audioClip, out filepath, false);    // 将音频转换为PSM编码并写入内存
        server.Send(bytes);     // 发送音频数据
        server.Send(Encoding.UTF8.GetBytes(END_FLAG));      // 发送结束符号
    }
}
