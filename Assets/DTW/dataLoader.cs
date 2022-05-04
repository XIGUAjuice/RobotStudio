using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;

class MotionData
{
    private int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    private List<Vector<double>> data;
    public List<Vector<double>> Data
    {
        get { return data; }
        set { data = value; }
    }
    private string label;
    public string Label
    {
        get { return label; }
        set { label = value; }
    }
    public MotionData(int _id, List<Vector<double>> _data, string _label)
    {
        id = _id;
        data = _data;
        label = _label;
    }
}

class DataLoader : IEnumerable<MotionData>
{
    List<MotionData> datas;
    private TextAsset[] files;
    private int idCounter = 0;
    public DataLoader()
    {
        datas = new List<MotionData>();

        files = Resources.LoadAll<TextAsset>("datasets");
        var V = Vector<double>.Build;
        foreach (var file in files)
        {
            List<Vector<double>> data = new List<Vector<double>>();
            string[] lines = file.text.Split("\n");
            string motionName = lines[0].Split(",")[^1][0..^1];     // 最后一个字符是换行符 需要排除
            foreach (var line in lines[1..^1])      // 最后一行是空行 需要排除
            {
                double[] values = line.Split(",").Where(x => double.TryParse(x, out _)).Select(double.Parse).ToArray();
                Vector<double> accl = V.DenseOfArray(values);
                data.Add(accl);
            }
            datas.Add(new MotionData(idCounter++, data, motionName));
        }
    }
    public int Len
    {
        get { return datas.Count; }
    }

    public MotionData this[int i]
    {
        get { return datas[i]; }
        set { datas[i] = value; }
    }

    public IEnumerator<MotionData> GetEnumerator()
    {
        return datas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}