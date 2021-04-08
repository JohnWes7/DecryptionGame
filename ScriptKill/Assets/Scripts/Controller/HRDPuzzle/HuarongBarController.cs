using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HuarongBarType
{
    Vertical2 = 0,
    vertical3 = 1,
    Horizonal2 = 2
}

public class HuarongBarController : UseItems, IBeDrag, IMouseUp
{
    public HuarongBarType barType;

    private Vector3 beginPoint;
    private Vector3 beginLocalPos;

    public HuarongPuzzleController puzzleController;

    private void Start()
    {
        InIt();   
    }

    public void InIt()
    {
        //获取父物体上的谜题控制器
        puzzleController = transform.parent.GetComponent<HuarongPuzzleController>();

        base.lookType = LookType.HuarongPuzzle;
    }

    public override void BeUse(RaycastHit hitInfo)
    {
        Debug.Log("down");
        beginPoint = hitInfo.point;
        beginLocalPos = transform.localPosition;
    }

    public void BeDrag(Vector3 point, LookType playerLookType)
    {
        if (playerLookType != this.lookType)
        {
            return;
        }

        Debug.Log("drag");
        return;
        //把鼠标滑动的向量转换到父物体的坐标系
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
        delta = transform.parent.InverseTransformDirection(delta);
        delta = delta - beginPoint;
        
        transform.localPosition = beginLocalPos + new Vector3(delta.x, 0, delta.z);

    }

    public void MouseUp()
    {
        Debug.Log("up");
    }


    private void OnMouseDown()
    {
        Debug.Log("down");
        beginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
        beginPoint = transform.parent.InverseTransformVector(beginPoint);

        beginLocalPos = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
        delta = transform.parent.InverseTransformVector(delta);
        delta = delta - beginPoint;

        transform.localPosition = beginLocalPos + new Vector3(delta.x, 0, delta.z);
    }


}
