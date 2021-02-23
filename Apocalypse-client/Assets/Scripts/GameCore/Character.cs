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
using GameCore;
using UnityEngine;

public enum EDir
{
    left,
    right,
}

public enum ECharacterState
{
    None,
    Running,     //跑步中
    Climbing,    //攀爬中
}

public class Character : MonoBehaviour
{
    public float walkSpeed = 1;
    private Rigidbody mRigidbody;
    private Animator mAnimator;
    private ECharacterState mCurState;
    
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 行走
    /// </summary>
    public void Walk(EDir rMoveDir)
    {
        mCurState = ECharacterState.Running;

        
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
        mAnimator.SetBool("Run", true);
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        mCurState = ECharacterState.None;
        mRigidbody.velocity = Vector3.zero;
        mAnimator.SetBool("Run", false);
    }

    /// <summary>
    /// 爬梯子
    /// </summary>
    public void ClimbLadder()
    {
        mCurState = ECharacterState.Climbing;
        mRigidbody.velocity = Vector3.down;
        mAnimator.SetBool("Climb", false);
    }

    /// <summary>
    /// 检测最近的道具
    /// </summary>
    public void DetectClosetProp()
    {
        var rCharacterPos = transform.position;
        var rAllColliders = Physics.OverlapBox(transform.position + Vector3.up, new Vector3(0.3f, 1, 0.3f));
        for (int i = 0; i < rAllColliders.Length; i++)
        {
            if (rAllColliders[i].GetComponent<Prop>())
            {
                
            }
        }
    }
}
