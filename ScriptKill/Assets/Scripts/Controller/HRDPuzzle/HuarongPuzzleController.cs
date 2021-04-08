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
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    public void InIt()
    {
        foreach (Transform child in transform)
        {

            HuarongBarController bar = child.GetComponent<HuarongBarController>();

            if (bar)
            {
                switch (bar.barType)
                {
                    case HuarongBarType.Vertical2:

                        break;
                    case HuarongBarType.vertical3:
                        break;
                    case HuarongBarType.Horizonal2:
                        break;
                    default:
                        break;
                }
            }

            
        }
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
}
