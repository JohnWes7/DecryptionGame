using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum HuarongBarType
{
    Vertical2 = 0,
    vertical3 = 1,
    Horizonal2 = 2,
    PlayerHorizonal2 = 3
}

public class HuarongBarController : UseItems, IBeDrag, IMouseUp
{
    [Tooltip("该滑块的类别")]
    public HuarongBarType barType;

    private Vector3 beginPoint;
    private Vector3 beginLocalPos;

    [Tooltip("滑块在轴心在数组中的位置")]
    public Vector2Int arrayPos;
    [Tooltip("谜题控制器")]
    public HuarongPuzzleController puzzleController;

    private void Start()
    {
        InIt();   
    }

    /// <summary>
    /// 初始化函数
    /// </summary>
    public void InIt()
    {
        //获取父物体上的谜题控制器
        puzzleController = transform.parent.GetComponent<HuarongPuzzleController>();

        base.lookType = LookType.HuarongPuzzle;
    }

    public override void BeUse(RaycastHit hitInfo)
    {
        //记录初始点的位置
        beginPoint = hitInfo.point;
        beginLocalPos = transform.localPosition;

        //自身位置置0
        List<Vector2Int> posList = SelfArrayPos(this.arrayPos);
        for (int i = 0; i < posList.Count; i++)
        {
            puzzleController.DeleteArrayPos(posList[i]);
        }
    }

    /// <summary>
    /// 由player调用
    /// </summary>
    /// <param name="point"></param>
    /// <param name="playerLookType"></param>
    public void BeDrag(Vector3 point, LookType playerLookType)
    {
        if (playerLookType != this.lookType)
        {
            return;
        }

        //把鼠标滑动的向量转换到父物体的坐标系
        //Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, puzzleController.CameraPos.localPosition.y));
        Vector3 delta = point - beginPoint;
        delta = puzzleController.transform.InverseTransformDirection(delta);
        Debug.Log(delta);
        delta = new Vector3(delta.x / puzzleController.transform.lossyScale.x, 0, delta.z / puzzleController.transform.lossyScale.z);

        

        //拖动
        Vector3 willLocalPos = new Vector3();
        Vector2Int willArrayPosCeil = new Vector2Int();
        Vector2Int willArrayPosFloor = new Vector2Int();
        switch (barType)
        {
            case HuarongBarType.Vertical2:
                willLocalPos = beginLocalPos + new Vector3(0, 0, delta.z);
                willArrayPosCeil = new Vector2Int(arrayPos.x, Mathf.CeilToInt(willLocalPos.z));
                willArrayPosFloor = new Vector2Int(arrayPos.x, Mathf.FloorToInt(willLocalPos.z));
                break;

            case HuarongBarType.vertical3:
                willLocalPos = beginLocalPos + new Vector3(0, 0, delta.z);
                willArrayPosCeil = new Vector2Int(arrayPos.x, Mathf.CeilToInt(willLocalPos.z));
                willArrayPosFloor = new Vector2Int(arrayPos.x, Mathf.FloorToInt(willLocalPos.z));
                break;

            case HuarongBarType.Horizonal2:
                willLocalPos = beginLocalPos + new Vector3(delta.x, 0, 0);
                willArrayPosCeil = new Vector2Int(Mathf.CeilToInt(willLocalPos.x), arrayPos.y);
                willArrayPosFloor = new Vector2Int(Mathf.FloorToInt(willLocalPos.x), arrayPos.y);
                break;

            case HuarongBarType.PlayerHorizonal2:
                willLocalPos = beginLocalPos + new Vector3(delta.x, 0, 0);
                willArrayPosCeil = new Vector2Int(Mathf.CeilToInt(willLocalPos.x), arrayPos.y);
                willArrayPosFloor = new Vector2Int(Mathf.FloorToInt(willLocalPos.x), arrayPos.y);
                break;

            default:
                break;
        }

        bool judgeCeil = true;
        bool judgeFloor = true;

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

        //如果正方向碰壁负方向没有 则直接到负方向整数
        if (!judgeCeil && judgeFloor)
        {
            transform.localPosition = new Vector3(willArrayPosFloor.x, transform.localPosition.y, willArrayPosFloor.y);
        }

        if (!judgeFloor && judgeCeil)
        {
            transform.localPosition = new Vector3(willArrayPosCeil.x, transform.localPosition.y, willArrayPosCeil.y);
        }

        //都没有碰撞的话就可以说明玩家指定的willpos是可以使用的
        if (judgeFloor && judgeCeil)
        {
            transform.localPosition = willLocalPos;
        }


    }

    public void MouseUp()
    {
        //变化位置
        arrayPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));

        //添加移动完后点的位置 也就是arraypos
        List<Vector2Int> posList = SelfArrayPos(arrayPos);
        for (int i = 0; i < posList.Count; i++)
        {
            switch (barType)
            {
                case HuarongBarType.PlayerHorizonal2:
                    puzzleController.AddArrayPos(posList[i], 2);
                    break;
                default:
                    puzzleController.AddArrayPos(posList[i], 1);
                    break;
            }
        }

        puzzleController.DebugPrintMap();

        //根据数组中位置计算应该在的位置
        Vector3 shouldLocalPos = new Vector3(arrayPos.x, transform.localPosition.y, arrayPos.y);
        //利用dotween平移过去
        transform.DOLocalMove(shouldLocalPos, 0.2f);
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

    /// <summary>
    /// 通过轴心位置和bar类型返回所有应该是该bar的点
    /// </summary>
    /// <param name="arrayPos">数组中的位置</param>
    /// <returns>所有该类型bar在该点的arrayPos</returns>
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
