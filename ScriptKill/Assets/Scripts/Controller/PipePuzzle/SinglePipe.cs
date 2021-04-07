using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class SinglePipe : UseItems
{
    [Tooltip("有没有被查找到")]
    public bool isFind = false;
    public Vector2 pos;
    public Vector4 dir;

    [Tooltip("通路的替换颜色（被找到）")]
    public Material green;
    [Tooltip("原本的材质")]
    public Material originMat;

    [Tooltip("旋转时间")]
    public float rotateTime = 0.2f;

    private void Awake()
    {
        base.lookType = LookType.PipePuzzle;
    }

    public override void BeUse()
    {
        //改变旋转
        //transform.Rotate(0, 90, 0);
        transform.DOKill(true);//如果之前的旋转还没到位直接kill true到位，保证连续点击不会出事
        transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(0, 90, 0), rotateTime);

        //（真正影响旋转的dir）改变dir
        Vector4 temp = dir;
        dir.x = temp.w;
        dir.y = temp.x;
        dir.z = temp.y;
        dir.w = temp.z;

        transform.parent.GetComponent<PipeController>().ScaneCorrect();
    }

    public void BeChangeColor()
    {
        GetComponent<MeshRenderer>().material = green;
    }
    public void BeNormalColor()
    {
        GetComponent<MeshRenderer>().material = originMat;
    }
}
