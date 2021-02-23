/************************************************************************************
 * 文件名：  Character
 * 版本号：  V1.0.0.0
 * 创建人： Rex_Yu
 * 描述：角色
 * 修改：移动
 * 
 ************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDir
{
    left,
    right,
}

public class Character : MonoBehaviour
{
    public float walkSpeed = 1;
    private Rigidbody mRigidbody;
    
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 行走
    /// </summary>
    public void Walk(EDir rMoveDir)
    {
        switch (rMoveDir)
        {
            case EDir.left:
            {
                transform.forward= Vector3.left;
            }
                break;
            case EDir.right:
            {
                transform.forward = Vector3.right;
            }
                break;
        }

        mRigidbody.velocity = walkSpeed * transform.forward;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        mRigidbody.velocity = Vector3.zero;
    }
}
