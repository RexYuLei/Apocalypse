using System;
using System.Collections;
using System.Collections.Generic;
using IcecreamView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewGameCompletePanel : IC_AbstractModule
{
    
    [BindUIPath("ContinueBtn")]
    private Button mContinueBtn;
    
    public override void OnInitView()
    {
        this.mContinueBtn.onClick.AddListener((() =>
        {
            int rIndex = int.Parse(SceneManager.GetActiveScene().name);
            rIndex += 1;
            if (rIndex > 5)
                rIndex = 1;
            SceneManager.LoadScene(rIndex.ToString());
        }));
    }

    public override void OnOpenView(Dictionary<string, IC_ViewData> parameters)
    {
    }

    public override void OnCloseView()
    {
    }

    public override void OnDestoryView()
    {
    }
}
