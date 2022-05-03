using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;

public class MotionClassifier : MonoBehaviour
{
    private DataLoader dataLoader;
    private const string ip = "192.168.1.113";  // 遥控器的ip
    private const int port = 2000;  // 遥控器的端口
    private IPEndPoint remoteEP;    // 遥控器的ip与端口
    private IPEndPoint recvEP;      // 存储接收数据时对方的ip与端口
    private UdpClient client;       // UDP客户端
    private string strRecv;
    private bool hasMessage = false;

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

    void Start()
    {
        dataLoader = new DataLoader();  // 建立数据集
        client = new UdpClient(8889);   // 开启UDP客户端
        remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);   // 指定遥控器的ip与端口号
        recvEP = new IPEndPoint(IPAddress.Any, 0);      // 存储接收数据时对方的ip与端口

        // 发送握手消息
        byte[] bytesSend = Encoding.ASCII.GetBytes("hello");
        client.Send(bytesSend, bytesSend.Length, remoteEP);
        hasMessage = false;

        Task.Run(() =>
        {
            while (true)
            {
                byte[] bytesRecv = client.Receive(ref recvEP);
                strRecv = Encoding.ASCII.GetString(bytesRecv);
                Debug.Log("接收到消息");
                Debug.Log(strRecv);
                hasMessage = true;
            }
        });

        // var costs = Matrix<double>.Build.Dense(dataLoader.Len, dataLoader.Len);
        // for (int i = 0; i < dataLoader.Len; i++)
        // {
        //     var data1 = dataLoader[i];
        //     for (int j = 0; j < dataLoader.Len; j++)
        //     {
        //         var data2 = dataLoader[j];
        //         List<Vector<double>> series1 = data1.Data;
        //         List<Vector<double>> series2 = data2.Data;
        //         double cost = DTW(series1, series2, Distance.Manhattan);
        //         costs[i, j] = cost;
        //     }
        // }
        // Debug.Log(costs.ToString(30, 30));
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMessage)
        {   
            string[] lines = strRecv.Split("\n");
            List<Vector<double>> dataToClassify = new List<Vector<double>>();
            var V = Vector<double>.Build;

            foreach (var line in lines[0..^1])     // 最后一行是空行，需排除
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
            hasMessage = false;
        }
    }
}
