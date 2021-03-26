using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePipe : MonoBehaviour,IBeUse
{
    [Tooltip("有没有被查找到")]
    public bool isFind = false;
    public Vector2 pos;
    public Vector4 dir;

    [Tooltip("通路的替换颜色（被找到）")]
    public Material green;
    [Tooltip("原本的材质")]
    public Material originMat;

    public void BeUse()
    {
        //改变旋转
        transform.Rotate(0, 90, 0);

        //（真正影响旋转的dir）改变dir
        Vector4 temp = dir;
        dir.x = temp.y;
        dir.y = temp.z;
        dir.z = temp.w;
        dir.w = temp.x;
    }
}
