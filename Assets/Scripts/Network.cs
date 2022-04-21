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
    private IPEndPoint recvEP;      // 存储接收数据时对方的ip与端口
    private UdpClient client;       // UDP客户端
    private TMP_InputField inputField;      // 输入框
    private bool hasMessage;
    private string msg;


    void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();

        client = new UdpClient(8888);   // 开启UDP客户端
        client.Client.ReceiveTimeout = 5000;
        remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);   // 指定ECB的ip与端口号
        recvEP = new IPEndPoint(IPAddress.Any, 0);      // 存储接收数据时对方的ip与端口

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
                if (command == "connect")
                {
                    connect();
                }
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

    private byte[] hexStr2Bytes(string hex)
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
        client.Send(bytes, bytes.Length, remoteEP);
        Debug.Log($"发送数据: {BitConverter.ToString(bytes)}");
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

    private async void connect()
    {
        sendMsg("EE00440000ED");
        try
        {
            byte[] bytes = await Task.Run(() => client.Receive(ref recvEP));
            string msg = BitConverter.ToString(bytes);
            if (msg == "EE-00-44-00-01-01-00-00-ED")
            {
                Debug.Log("连接成功");
            }
            else
            {
                throw new TimeoutException("连接超时");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
