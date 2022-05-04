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
    private const string APPID = "4985560f";
    private const string APIKEY = "5e5e4a90ea64a3d394e3b658b3babfdc";
    private const string AUTH_ID = "347c84b84c7a69928eae544553ee439f";
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
    private string direction = "";  // 机械臂末端下一次移动的方向
    private float distance = 0;     // 机械臂末端下一次移动的距离
    private WebSocket server;       // websocket客户端
    private bool hasMessage = false;// 是否收到websocket返回的消息
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
        if (hasMessage)
        {
            /* 判断方向，在相机坐标系下进行移动 */
            if (direction.Equals("上"))
            {
                Debug.Log($"机械臂向上移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, distance, 0), new Vector3(0, 0, 0));
                // position.y += distance;
            }
            else if (direction.Equals("下"))
            {
                Debug.Log($"机械臂向下移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, -distance, 0), new Vector3(0, 0, 0));
                // position.y -= distance;
            }
            else if (direction.Equals("左"))
            {
                Debug.Log($"机械臂向左移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(-distance, 0, 0), new Vector3(0, 0, 0));
                // position.x -= distance;
            }
            else if (direction.Equals("右"))
            {
                Debug.Log($"机械臂向右移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(distance, 0, 0), new Vector3(0, 0, 0));
                // position.x += distance;
            }
            else if (direction.Equals("前"))
            {
                Debug.Log($"机械臂向前移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, 0, distance), new Vector3(0, 0, 0));
                // position.z += distance;
            }
            else if (direction.Equals("后"))
            {
                Debug.Log($"机械臂向后移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, 0, -distance), new Vector3(0, 0, 0));
                // position.z -= distance;
            }

            /* 清除websocket传入的消息 */
            hasMessage = false;
            direction = "";
            distance = 0;
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
                Debug.Log("匹配到意图");
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
                hasMessage = true;
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
