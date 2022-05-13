using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using TMPro;

public class Network : MonoBehaviour
{
    private const string ip = "192.168.1.30";
    private const int port = 2000;

    private IPEndPoint remoteEP;    // ECB的ip与端口
    public IPEndPoint recvEP;      // 存储接收数据时对方的ip与端口
    public UdpClient client;       // UDP客户端
    private TMP_InputField inputField;      // 输入框
    private bool hasMessage;
    private bool waitForMsg;
    public bool WaitForMsg
    {
        get { return waitForMsg; }
        set { waitForMsg = value; }
    }
    private string msg;


    void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();

        client = new UdpClient(10005);   // 开启UDP客户端
        client.Client.ReceiveTimeout = 10000;    // 设置UDP超时时间
        remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);   // 指定ECB的ip与端口号
        recvEP = new IPEndPoint(IPAddress.Any, 0);      // 存储接收数据时对方的ip与端口

        waitForMsg = false;

        // // 在新线程中异步等待消息
        // client.BeginReceive(new AsyncCallback(recvCallback), null);
    }

    void Update()
    {
        /* 按下回车发送消息 */
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.isFocused)
            {
                string[] text = inputField.text.Split('\n');
                string command = text[text.Length - 2];
            }
        }

        /* 收到消息，处理消息 */
        if (hasMessage)
        {
            inputField.text += $"<color=white>{msg}</color>\n";     // 在输入框打印接收到的消息
            inputField.MoveToEndOfLine(false, true);        // 移动光标至末尾
            hasMessage = false;
        }
    }

    public byte[] hexStr2Bytes(string hex)
    {
        /* 将十六进制字符串转换为字节数组 */
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }

    public void sendMsg(string msg)
    {
        /* 将十六进制字符串转换成字节后发送 */
        byte[] bytes = hexStr2Bytes(msg);

        while (waitForMsg) { }  // 等待上一个发送-接收处理完毕再发送下一个消息

        waitForMsg = true;
        client.Send(bytes, bytes.Length, remoteEP);
        Debug.Log($"发送数据: {BitConverter.ToString(bytes)}");
    }

    public string[] recvMsg()
    {
        /* 将收到的消息转换成十六进制字符串 */
        byte[] bytesRecv = client.Receive(ref recvEP);
        string msgRecv = BitConverter.ToString(bytesRecv);
        Debug.Log($"接收数据: {msgRecv}");

        return msgRecv.Split('-');
    }

    private void recvCallback(IAsyncResult ar)
    {
        Debug.Log("接收到数据");
        hasMessage = true;
        byte[] bytes = client.EndReceive(ar, ref recvEP);      // 接收数据报
        msg = BitConverter.ToString(bytes);     // 转换为十六进制字符串
        while (hasMessage) { }                  // 阻塞，等待Update()将消息处理完毕

        // 等待下一个消息
        client.BeginReceive(new AsyncCallback(recvCallback), null);
    }

}
