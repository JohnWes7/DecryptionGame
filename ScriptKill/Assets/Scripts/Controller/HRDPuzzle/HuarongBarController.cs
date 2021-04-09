using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HuarongBarType
{
    Vertical2 = 0,
    vertical3 = 1,
    Horizonal2 = 2,
    PlayerHorizonal2 = 3
}

public class HuarongBarController : UseItems, IBeDrag
{
    public HuarongBarType barType;

    private Vector3 beginPoint;
    private Vector3 beginLocalPos;

    public Vector2Int arrayPos;

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
        Debug.Log("down" + gameObject.name);
        beginPoint = hitInfo.point;
        beginLocalPos = transform.localPosition;

        //自身位置置0
        List<Vector2Int> posList = SelfArrayPos(this.arrayPos);
        for (int i = 0; i < posList.Count; i++)
        {
            puzzleController.DeleteArrayPos(posList[i]);
        }

    }

    public void BeDrag(Vector3 point, LookType playerLookType)
    {
        if (playerLookType != this.lookType)
        {
            return;
        }

        Debug.Log("drag" + gameObject.name);
        //把鼠标滑动的向量转换到父物体的坐标系
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
        delta = transform.parent.InverseTransformDirection(delta);
        delta = delta - beginPoint;

        //拖动
        switch (barType)
        {
            case HuarongBarType.Vertical2:

                Vector3 willLocalPos = beginLocalPos + new Vector3(0, 0, delta.z);
                Vector2Int willArrayPosCeil = new Vector2Int(arrayPos.x, Mathf.CeilToInt(willLocalPos.z));
                Vector2Int willArrayPosFloor = new Vector2Int(arrayPos.x, Mathf.FloorToInt(willLocalPos.z));

                bool judgeCeil = true;
                bool judgeFloor = true;

                Debug.Log(willArrayPosCeil.ToString() + " " + willArrayPosFloor.ToString());

                //判断正方向位移碰撞
                List<Vector2Int> posList = SelfArrayPos(willArrayPosCeil);
                for (int i = 0; i < posList.Count; i++)
                {
                    judgeCeil = judgeCeil && puzzleController.IsPosAvailable(posList[i]);
                }
                //判断负方向位移碰撞
                posList = SelfArrayPos(willArrayPosFloor);
                for (int i = 0; i < posList.Count; i++)
                {
                    judgeFloor = judgeFloor && puzzleController.IsPosAvailable(posList[i]);
                }

                if (!judgeCeil)
                {
                    transform.localPosition = new Vector3(willArrayPosFloor.x * transform.parent.lossyScale.x, transform.localPosition.y, willArrayPosFloor.y * transform.parent.lossyScale.z);
                }

                if (!judgeFloor)
                {
                    transform.localPosition = new Vector3(willArrayPosCeil.x * transform.parent.lossyScale.x, transform.localPosition.y, willArrayPosCeil.y * transform.parent.lossyScale.z);
                }

                if (judgeFloor && judgeCeil)
                {
                    transform.localPosition = willLocalPos;
                }

                break;
            case HuarongBarType.vertical3:
                

                break;
            case HuarongBarType.Horizonal2:
                break;
            case HuarongBarType.PlayerHorizonal2:
                break;
            default:
                break;
        }

    }

    

    #region PC用
    //private void OnMouseDown()
    //{
    //    Debug.Log("down");
    //    beginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
    //    beginPoint = transform.parent.InverseTransformVector(beginPoint);

    //    beginLocalPos = transform.localPosition;
    //}

    //private void OnMouseDrag()
    //{
    //    Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.position.y));
    //    delta = transform.parent.InverseTransformVector(delta);
    //    delta = delta - beginPoint;

    //    transform.localPosition = beginLocalPos + new Vector3(delta.x, 0, delta.z);
    //} 
    #endregion

    public List<Vector2Int> SelfArrayPos(Vector2Int arrayPos)
    {
        List<Vector2Int> pointList = new List<Vector2Int>();

        switch (barType)
        {
            case HuarongBarType.Vertical2:
                pointList.Add(arrayPos);
                pointList.Add(new Vector2Int(arrayPos.x, arrayPos.y + 1));
                break;
            case HuarongBarType.vertical3:
                for (int i = -1; i < 2; i++)
                {
                    pointList.Add(new Vector2Int(arrayPos.x, arrayPos.y + i));
                }
                break;
            case HuarongBarType.Horizonal2:
                pointList.Add(arrayPos);
                pointList.Add(new Vector2Int(arrayPos.x + 1, arrayPos.y));
                break;
            case HuarongBarType.PlayerHorizonal2:
                pointList.Add(arrayPos);
                pointList.Add(new Vector2Int(arrayPos.x + 1, arrayPos.y));
                break;
            default:
                break;
        }

        return pointList;
    }
}
