using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IBeUse的校验会在player，但要用类来封装来获取look属性
//IBeDrag的校验会在BeDrag方法里，需要player传入自己的looktype属性进行校验

public interface IBeUse
{
    void BeUse();
}

public interface IBeDrag
{
    void BeDrag(Vector3 point, LookType playerLookType);
}

public class UseItems : MonoBehaviour, IBeUse
{
    public LookType lookType;

    public virtual void BeUse()
    {

    }
}
