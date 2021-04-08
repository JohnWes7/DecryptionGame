using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePanelController : MonoBehaviour
{
    [Tooltip("退出注视状态的按钮")]
    public GameObject BackFromLookAtButton;

    public UnityEvent onBackFormLookAt;
    
    

    public void ShowBackFromLookAtButton()
    {
        BackFromLookAtButton.SetActive(true);

        //之后可以添加淡入淡出

    }

    public void HideBackFromLookAtButton()
    {
        BackFromLookAtButton.SetActive(false);
    }

    public void BackFormLookAtCallBack()
    {
        PlayerController.Instance.CancelLookAt();
        HideBackFromLookAtButton();

        //执行委托事件
        onBackFormLookAt.Invoke();
    }
}
