using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuarongPuzzleController : UseItems
{
    public bool state = false;
    [Tooltip("固定相级位置")]
    public Transform CameraPos;
    [Tooltip("世界坐标系下的unscale长宽")]
    public Vector2Int Rect = new Vector2Int(6, 6);

    int[,] map = new int[6, 6];

    private void Start()
    {
        base.lookType = LookType.None;
        InIt();
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    public void InIt()
    {
        //将所有的方块加入数组
        foreach (Transform child in transform)
        {

            HuarongBarController bar = child.GetComponent<HuarongBarController>();

            if (bar)
            {
                switch (bar.barType)
                {
                    case HuarongBarType.Vertical2:
                        map[bar.arrayPos.x, bar.arrayPos.y] = 1;
                        map[bar.arrayPos.x, bar.arrayPos.y + 1] = 1;
                        break;
                    case HuarongBarType.vertical3:
                        for (int i = -1; i < 2; i++)
                        {
                            map[bar.arrayPos.x, bar.arrayPos.y + i] = 1;
                        }
                        break;
                    case HuarongBarType.Horizonal2:
                        map[bar.arrayPos.x, bar.arrayPos.y] = 1;
                        map[bar.arrayPos.x + 1, bar.arrayPos.y] = 1;
                        break;
                    case HuarongBarType.PlayerHorizonal2:
                        map[bar.arrayPos.x, bar.arrayPos.y] = 2;
                        map[bar.arrayPos.x + 1, bar.arrayPos.y] = 2;
                        break;
                    default:
                        break;
                }
            }

        }

        //输出数组到控制台
        DebugPrintMap();
    }

    public override void BeUse(RaycastHit hitInfo)
    {
        //如果被解开了就不能再被use
        if (state)
        {
            return;
        }
        

        //注视动作
        PlayerController.Instance.LookAt(CameraPos.position, CameraPos.rotation, LookType.HuarongPuzzle);

        //找到ui
        GamePanelController ui = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanelController>();
        //调用方法显示回退按钮
        ui.ShowBackFromLookAtButton();
        //添加事件
        ui.onBackFormLookAt.AddListener(ReactiveBoxCollider);

        //关闭collider
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    public void ReactiveBoxCollider()
    {
        Debug.Log("重新开启碰撞器");
        GetComponent<BoxCollider>().enabled = true;

        //移除自身委托事件
        GamePanelController ui = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanelController>();
        ui.onBackFormLookAt.RemoveListener(ReactiveBoxCollider);
    }

    /// <summary>
    /// 判断该点是否可用
    /// </summary>
    /// <param name="pos">指定位置</param>
    /// <returns>是否可用</returns>
    public bool IsPosAvailable(Vector2Int pos)
    {

        if (pos.x < 0 || pos.y < 0 || pos.x >= map.GetLength(0) || pos.y >= map.GetLength(1))
        {
            return false;
        }

        if (map[pos.x, pos.y] != 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 将指定位置变为0 表示没有障碍
    /// </summary>
    /// <param name="pos">指定位置</param>
    public void DeleteArrayPos(Vector2Int pos)
    {
        map[pos.x, pos.y] = 0;
    }
    
    /// <summary>
    /// 将指定位置变为type（int） 表示有物体
    /// </summary>
    /// <param name="pos">轴心点的位置</param>
    /// <param name="type">需要填入的int数值</param>
    public void AddArrayPos(Vector2Int pos, int type)
    {
        map[pos.x, pos.y] = type;
    }

    /// <summary>
    /// 打印当前地图到控制台
    /// </summary>
    public void DebugPrintMap()
    {
        string de = "\n";

        for (int y = map.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                de = de + map[x, y].ToString() + " ";
            }

            de += "\n";
        }

        Debug.Log(de);
    }
}
