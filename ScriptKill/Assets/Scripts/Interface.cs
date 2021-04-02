using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeUse
{
    void BeUse();
}

public class UseItems : MonoBehaviour, IBeUse
{
    public LookType lookType;

    public virtual void BeUse()
    {

    }
}
