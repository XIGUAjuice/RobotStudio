using UnityEngine;

public static class Algorithm
{
    public static DoubleVector IkNearest(Vector3 position, Quaternion rotation, Transform transBase, double[] jointAnglesCur)
    {
        // return: C++ vector<double> which is the nearest inverse kinematics solution
        // positon: position of end-effector in **World Space**
        // rotation: rotation of end-effector in **World Space**
        // transBase: Transform of Base

        /* Transforms position from world space to Base space */
        position = transBase.InverseTransformPoint(position);
        rotation = Quaternion.Inverse(transBase.rotation) * rotation;
        Vector3 angles = rotation.eulerAngles;

        Debug.Log($"逆运动学目标位姿(机械臂基座坐标系): Position: ({position.x}, {position.y}, {position.z}), Angles: ({angles.x}, {angles.y}, {angles.z})");

        var T70 = Gluon.poseTotrans(angles.x, angles.y, angles.z, position.x, position.y, position.z);
        var solutions = Gluon.IkSolver(T70);

        DoubleVector jointAnglesVector = new DoubleVector(jointAnglesCur);
        var solution = Gluon.findNearestSolution(solutions, jointAnglesVector);

        return solution;
    }
}