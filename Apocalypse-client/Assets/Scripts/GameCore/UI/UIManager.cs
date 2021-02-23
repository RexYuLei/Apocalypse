using System;
using System.Collections;
using System.Collections.Generic;
using HardHead.UI;
using IcecreamView;
using UnityEngine;

public class UIManager
{
    private static IC_Controller mController;

    public static IC_Controller Instance
    {
        get
        {
            if (mIsInit)
                return mController;
            Debug.Log("UIManager Not Init");
            return null;
        }
    }

    private static bool mIsInit;

    public static void Init(Transform rUIRoot, Action rOnInitComplete)
    {
        //if(mIsInit)
            //return;
        var rConfig = new UIConfig();
        rConfig.OnComplete = rOnInitComplete;
        mController = IC_Controller.InstantiateViewManager(rConfig, rUIRoot);
        if (mController != null)
        {
            mIsInit = true;
        }
    }
}
