using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawerController : MonoBehaviour, IBeUse
{
    [Tooltip("抽屉实例")]
    public GameObject drawer;
    [Tooltip("抽屉被打开的距离")]
    public float dis;
    [Tooltip("抽屉打开的方向（抽屉坐标系下）")]
    public Vector3 dir;

    public float duringTime = 0.2f;

    public Ease curve = Ease.OutQuad;

    [Tooltip("抽屉是否上锁状态")]
    public bool isLocked = false;
    [Tooltip("抽屉开关状态")]
    public bool isOpen = false;

    public void BeUse()
    {
        //被上锁了的话直接进行被上锁回调
        if (isLocked)
        {
            LockedCallback();
            return;
        }

        if (!isOpen)
        {
            //抽屉被打开的use重写
            drawer.transform.DOLocalMove(dir * dis, duringTime).SetEase(curve);

            isOpen = true;
        }
        else
        {
            drawer.transform.DOLocalMove(Vector3.zero, duringTime).SetEase(curve);

            isOpen = false;
        }
        
    }

    public void LockedCallback()
    {

    }
}
