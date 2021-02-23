/************************************************************************************
 * 文件名：  GameManager
 * 版本号：  V1.0.0.0
 * 创建人：Rex_Yu
 * 描述： 管理器
 * 修改:
 * 
 ************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        var rUIRoot = GameObject.Find("Canvas").transform;
        UIManager.Init(rUIRoot,null);
    }

    void Update()
    {
        
    }
}
