using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using TMPro;

public class MotionClassifier : MonoBehaviour
{
    /* 预制件 */
    public GameObject objectEndEffector;
    public TMP_InputField inputStep;
    public Button connectButton;

    /* 成员变量 */
    private EndEffector endEffector;        // 末端执行器类对象
    private DataLoader dataLoader;  // 数据集
    private const string ip = "192.168.1.113";  // 遥控器的ip
    private const int port = 2000;  // 遥控器的端口
    private IPEndPoint remoteEP;    // 遥控器的ip与端口
    private IPEndPoint recvEP;      // 存储接收数据时对方的ip与端口
    private UdpClient client;       // UDP客户端
    private List<string> lines;
    private bool hasMessage = false;
    private bool connected = false;

    private double DTW(List<Vector<double>> series1, List<Vector<double>> series2, Func<Vector<double>, Vector<double>, double> distanceFunc)
    {
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;
        int m = series1.Count;
        int n = series2.Count;

        // 创建时多一行一列，便于第一列与第一行的计算
        Matrix<double> matrix = M.Dense(m + 1, n + 1, 0);
        matrix.SetRow(0, V.Dense(n + 1, Double.PositiveInfinity));
        matrix.SetColumn(0, V.Dense(m + 1, Double.PositiveInfinity));
        matrix[0, 0] = 0;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double cost = distanceFunc(series1[i], series2[j]);
                matrix[i + 1, j + 1] = cost + new double[] { matrix[i, j + 1], matrix[i + 1, j], matrix[i, j] }.Min();
            }
        }
        // 计算完毕去掉第一行和第一列
        matrix = matrix.SubMatrix(1, m, 1, n);

        return matrix[m - 1, n - 1];
    }

    private string classify(List<Vector<double>> series)
    {
        double minCost = Double.MaxValue;
        string label = string.Empty;
        foreach (var motionData in dataLoader)
        {
            double cost = DTW(series, motionData.Data, Distance.Manhattan);
            if (cost < minCost)
            {
                minCost = cost;
                label = motionData.Label;
            }
        }
        return label;
    }

    public void onConnectClick()
    {
        // 发送握手消息
        Debug.Log("发送握手消息");
        byte[] bytesSend = Encoding.ASCII.GetBytes("hello");
        client.Send(bytesSend, bytesSend.Length, remoteEP);

        Task.Run(() =>
        {
            byte[] bytesRecv = client.Receive(ref recvEP);
            string strRecv = Encoding.ASCII.GetString(bytesRecv);
            Debug.Log(strRecv);
            if (strRecv.Equals("hello"))
            {
                connected = true;
                Debug.Log("连接成功");
            }
        });
    }

    void Start()
    {
        endEffector = objectEndEffector.GetComponent<EndEffector>();        // 获取末端执行器类对象
        dataLoader = new DataLoader();  // 建立数据集
        client = new UdpClient(8889);   // 开启UDP客户端
        remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);   // 指定遥控器的ip与端口号
        recvEP = new IPEndPoint(IPAddress.Any, 0);      // 存储接收数据时对方的ip与端口
        lines = new List<string>();     // 初始化消息列表

        Task.Run(() =>
        {
            while (true)
            {
                if (connected)
                {
                    byte[] bytesRecv = client.Receive(ref recvEP);
                    string strRecv = Encoding.ASCII.GetString(bytesRecv);
                    if (strRecv.Equals("start"))
                    {
                        lines.Clear();
                    }
                    else if (strRecv.Equals("end"))
                    {
                        hasMessage = true;
                    }
                    else
                    {
                        lines.Add(strRecv);
                    }
                }
            }
        });
    }

    void Update()
    {
        if (connected)
        {
            connectButton.image.color = new Color32(142, 243, 70, 173);
        }
        if (hasMessage)
        {
            List<Vector<double>> dataToClassify = new List<Vector<double>>();
            var V = Vector<double>.Build;

            foreach (var line in lines)
            {
                double[] values = line.Split(',').Select(Double.Parse).ToArray();
                Vector<double> accl = V.DenseOfArray(values);
                dataToClassify.Add(accl);
            }

            double minCost = Double.MaxValue;
            string label = string.Empty;

            foreach (var motionData in dataLoader)
            {
                double cost = DTW(dataToClassify, motionData.Data, Distance.Manhattan);
                if (cost < minCost)
                {
                    minCost = cost;
                    label = motionData.Label;
                }
            }
            Debug.Log($"动作识别为{label}");
            float distance = float.Parse(inputStep.text);
            /* 判断方向，在相机坐标系下进行移动 */
            if (label.Equals("up"))
            {
                Debug.Log($"机械臂向上移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, distance, 0), new Vector3(0, 0, 0));
            }
            else if (label.Equals("down"))
            {
                Debug.Log($"机械臂向下移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, -distance, 0), new Vector3(0, 0, 0));
            }
            else if (label.Equals("left"))
            {
                Debug.Log($"机械臂向左移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(-distance, 0, 0), new Vector3(0, 0, 0));
            }
            else if (label.Equals("right"))
            {
                Debug.Log($"机械臂向右移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(distance, 0, 0), new Vector3(0, 0, 0));
            }
            else if (label.Equals("front"))
            {
                Debug.Log($"机械臂向前移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, 0, distance), new Vector3(0, 0, 0));
            }
            else if (label.Equals("back"))
            {
                Debug.Log($"机械臂向后移动了{distance}毫米");
                endEffector.moveInCameraTrans(new Vector3(0, 0, -distance), new Vector3(0, 0, 0));
            }
            hasMessage = false;
        }
    }
}
