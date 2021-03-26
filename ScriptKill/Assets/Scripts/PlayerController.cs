﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float rotateSpeed = 2f;
    public float moveSpeed = 3f;

    float m_CameraVerticalAngle; //旋转的最大最小值
    [Tooltip("反转Y轴")]
    public bool invertYAxis;
    [Tooltip("反转X轴")]
    public bool invertXAxis;
    [Tooltip("鼠标灵敏度")]
    public float lookSensitivity = 1; //鼠标灵敏度

    public Rigidbody m_rigidbody;
    public CharacterController m_characterController;

    public const string k_Horizontal = "Horizontal";
    public const string k_Vertical = "Vertical";
    public const string k_MouseX = "Mouse X";
    public const string k_MouseY = "Mouse Y";

    public Vector3 characterVelocity;

    void Start()
    {

    }


    void Update()
    {
        FPSMode();
    }

    public void RotateCameraMod()
    {
        if (Input.GetMouseButton(0))
        {
            // 水平旋转
            {
                transform.Rotate(new Vector3(0, GetLookInputsHorizontal() * rotateSpeed, 0), Space.World);
            }

            //垂直旋转
            {
                float tempAngle = m_CameraVerticalAngle - GetLookInputsVertical() * rotateSpeed;
                tempAngle = Mathf.Clamp(tempAngle, -89f, 89f);
                transform.Rotate(new Vector3(tempAngle - m_CameraVerticalAngle, 0, 0), Space.Self);
                m_CameraVerticalAngle = tempAngle;

            }
        }
    }

    public void FPSMode()
    {
        // 水平旋转
        {
            transform.Rotate(new Vector3(0, -GetLookInputsHorizontal() * rotateSpeed, 0), Space.World);
        }

        //垂直旋转
        {
            float tempAngle = m_CameraVerticalAngle + GetLookInputsVertical() * rotateSpeed;
            tempAngle = Mathf.Clamp(tempAngle, -89f, 89f);
            transform.Rotate(new Vector3(tempAngle - m_CameraVerticalAngle, 0, 0), Space.Self);
            m_CameraVerticalAngle = tempAngle;

        }



        Vector3 worldMoveDir = transform.TransformDirection(GetMoveInput());
        characterVelocity = Time.deltaTime * worldMoveDir * moveSpeed;

        transform.Translate(characterVelocity, Space.World);
        //m_characterController.Move(characterVelocity);

    }

    public Vector3 GetMoveInput()
    {
        float x = Input.GetAxis(k_Horizontal);
        float y = Input.GetAxis(k_Vertical);

        Vector3 move = new Vector3(x, 0, y);

        move = Vector3.ClampMagnitude(move, 1);

        return move;
    }

    public float GetLookInputsHorizontal()
    {
        return invertXAxis ? GetMouseOrStickLookAxis(k_MouseX) : -GetMouseOrStickLookAxis(k_MouseX);
    }

    public float GetLookInputsVertical()
    {
        return invertYAxis ? GetMouseOrStickLookAxis(k_MouseY) : -GetMouseOrStickLookAxis(k_MouseY);
    }

    float GetMouseOrStickLookAxis(string mouseInputName)
    {
        // Check if this look input is coming from the mouse
        //bool isGamepad = Input.GetAxis(stickInputName) != 0f;
        //float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

        float i = Input.GetAxisRaw(mouseInputName);

        // handle inverting vertical input
        //if (invertYAxis)
        //    i *= -1f;

        // apply sensitivity multiplier
        i *= lookSensitivity;

//        if (isGamepad)
//        {
//            // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
//            i *= Time.deltaTime;
//        }
//        else
//        {
//            // reduce mouse input amount to be equivalent to stick movement
//            i *= 0.01f;
//#if UNITY_WEBGL
//                // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
//                i *= webglLookSensitivityMultiplier;
//#endif
//        }

        return i;
    }
}
