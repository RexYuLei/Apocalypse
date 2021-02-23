using System;
using System.Collections.Generic;
using IcecreamView;
using UnityEngine;

namespace HardHead.UI
{
    public class UIConfig : IC_IViewConfig
    {
        public static readonly string AB_UIPath = "UIPrefabs";

    /// <summary>
    /// 资源预加载白名单 （仅会预加载ab包资源，不会预先创建到对象池）
    /// </summary>
    //public static readonly string[] AB_CacheView = new string[] {UIPanel.MobileInputPanel, UIPanel.LoadingPanel, UIPanel.MenuPanel, UIPanel.SettingPanel , UIPanel.GuideCompletePanel , UIPanel.GuideTipPanel , UIPanel.GuideShowImgPanel};

    private Dictionary<string, IC_IViewInfo> mViewInfoDict;

    /// <summary>
    /// View引用计数
    /// </summary>
    private Dictionary<string, int> mViewReferenceCount;

    public Action OnComplete;

    public void OnInit()
    {
        // Debug.Log("UIResourceHandler 加载中...");
        this.mViewInfoDict       = new Dictionary<string, IC_IViewInfo>();
        this.mViewReferenceCount = new Dictionary<string, int>();

        var tables = UIPanel.GetAllPanelTable();
        for (int i = 0; i < tables.Count; i++)
        {
            this.mViewInfoDict[tables[i]]       = null;
            this.mViewReferenceCount[tables[i]] = 0;
        }
        // foreach (var cache in AB_CacheView)
        // {
        //     this.LoadView(cache);
        // }
        this.OnComplete?.Invoke();
    }

    private void LoadView(string viewName)
    {
        if (this.mViewInfoDict[viewName] == null)
        {
            var viewObject = Resources.Load($"{AB_UIPath}/{viewName}") as GameObject;
            var view       = viewObject.GetComponent<IC_AbstractView>();
            if (view != null)
            {
                // Debug.Log("加载View资源(AB包)： " + viewName);
                var viewInfo = new IC_ViewInfo() {autoLoad = false, isOnce = true, Table = viewName, View = view};
                this.mViewInfoDict[viewName]       = viewInfo;
                this.mViewReferenceCount[viewName] = 0;
            }
        }
    }

    public string GetDefaultViewTable() { return null; }

    public bool ContainsKey(string viewTable) { return this.mViewInfoDict.ContainsKey(viewTable); }

    /// <summary>
    /// 页面预加载
    /// </summary>
    /// <returns></returns>
    public List<string> GetCacheTables() { return new List<string>(); }

    public void OnRemoveView(string viewTable)
    {
        if (this.mViewReferenceCount[viewTable] > 0)
        {
            this.mViewReferenceCount[viewTable] -= 1;
            if (this.mViewReferenceCount[viewTable] == 0)
            {
                //没有任何场景引用该页面,这里可以对该资源做内存管理
                // if (AB_CacheView.Length > 0)
                // {
                //     foreach (var cache in AB_CacheView)
                //     {
                //         if (viewTable.Equals(cache))
                //         {
                //             return;
                //         }
                //     }
                // }

                this.mViewInfoDict[viewTable] = null;
                //UnityResLoader.UnloadAsset(AB_UIPath + ":" + viewTable);
                // Debug.Log("销毁View资源(AB包)：" + viewTable);
            }
        }
    }

    public IC_IViewInfo OnAddView(string viewTable)
    {
        if (this.mViewInfoDict[viewTable] == null)
            this.LoadView(viewTable);

        this.mViewReferenceCount[viewTable] += 1;
        return this.mViewInfoDict[viewTable];
    }

    public void OnDispose()
    {
        this.mViewInfoDict.Clear();
        //ABLoader.Instance.UnloadAsset(AB_UIPath);
        // Resources.UnloadUnusedAssets();
    }
    }
}