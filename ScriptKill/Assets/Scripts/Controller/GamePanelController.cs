using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour
{
    public GameObject BackFromLookAtButton;
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
    }
}
