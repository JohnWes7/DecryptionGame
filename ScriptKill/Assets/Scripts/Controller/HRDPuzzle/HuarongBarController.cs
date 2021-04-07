using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HuarongBarType
{
    Vertical2 = 0,
    vertical3 = 1,
    Horizonal2 = 2
}

public class HuarongBarController : MonoBehaviour, IBeDrag
{
    [Tooltip("注视标记")]
    public LookType lookType = LookType.HuarongPuzzle;
    public bool isBeDrag = false;

    public HuarongBarType barType;

    private Vector3 beginPoint;

    public void BeDrag(Vector3 point, LookType playerLookType)
    {
        if (playerLookType != this.lookType)
        {
            return;
        }

        if (!isBeDrag)
        {
            beginPoint = point;
            return;
        }

        //把鼠标滑动的向量转换到父物体的坐标系
        Vector3 delta = point - beginPoint;
        delta = transform.parent.InverseTransformDirection(delta);


    }
}
