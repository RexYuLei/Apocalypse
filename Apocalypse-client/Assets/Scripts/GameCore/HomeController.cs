/************************************************************************************
 * 文件名：  HomeController
 * 版本号：  V1.0.0.0
 * 创建人： Rex_Yu
 * 描述：玩家在家中的控制行为
 * 修改：
 * 
 ************************************************************************************/

using System;
using UnityEngine;

namespace GameCore
{
    public class HomeController : MonoBehaviour
    {
        private Character mCharacter;

        private void Start()
        {
            mCharacter = GetComponent<Character>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                //左移
                mCharacter.Walk(EDir.left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //右移
                mCharacter.Walk(EDir.right);
            }
            else
            {
                mCharacter.StopMove();
            }
        }
    }
}