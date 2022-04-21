using UnityEngine;
using RTG;

public class GizmoListener : MonoBehaviour, IRTTransformGizmoListener
{   
    public GameObject endEffector;

    public bool OnCanBeTransformed(Gizmo transformGizmo)
    {
        return true;
    }

    public void OnTransformed(Gizmo transformGizmo)
    {   
        endEffector.GetComponent<EndEffector>().moveEndEffector(transform.position, transform.rotation);
    }
}
