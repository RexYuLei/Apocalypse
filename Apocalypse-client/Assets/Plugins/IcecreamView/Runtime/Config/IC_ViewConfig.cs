using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IcecreamView {

    [System.Serializable]
    public class IC_ViewInfo : IC_IViewInfo{

        public IC_AbstractView View;
        [Header("可选填，默认为View预制体名称")]
        public string Table;
        [Header("是否提前缓存(用于大型View)")]
        public bool autoLoad;
        [Header("是否作为一次性使用，当页面被关闭时直接销毁")]
        public bool isOnce;

        public string GetTable()
        {
            return Table;
        }

        public bool IsCache()
        {
            return autoLoad;
        }

        public IC_AbstractView GetView()
        {
            return View;
        }

        public bool IsOnce()
        {
            return isOnce;
        }
    }

    /// <summary>
    /// 游戏View配置表
    /// </summary>
    [CreateAssetMenu(fileName = "IC_ViewConfig" , menuName = "IceCreamView/IceView Config（Prefabs link）" , order = 88)]
    public class IC_ViewConfig : ScriptableObject , IC_IViewConfig
    {

        public string ConfigName;

        public string DefaultViewTable;

        public List<IC_ViewInfo> GameViewList;

        private Dictionary<string, IC_IViewInfo> mViewDic;

        public void OnInit()
        {
            Debug.Log("Init ViewConfig : " + ConfigName);
            mViewDic = new Dictionary<string, IC_IViewInfo>();
            mViewDic = GetViewModleDic();
        }

        public Dictionary<string, IC_IViewInfo> GetViewModleDic()
        {
            Dictionary<string, IC_IViewInfo> viewDic = new Dictionary<string, IC_IViewInfo>();

            if (GameViewList != null && GameViewList.Count > 0)
            {
                GameViewList.ForEach(view =>
                {
                    if (string.IsNullOrEmpty(view.Table))
                    {
                        view.Table = view.View.name;
                    }
                    if (!viewDic.ContainsKey(view.Table))
                    {
                        viewDic.Add(view.Table, view);
                    }
                    else {
                        Debug.LogError("[IceCreamView] View数据出现重名数据：" + view.Table);
                    }

                });
            }
            return viewDic;
        }

        public string GetDefaultViewTable()
        {
            return DefaultViewTable;
        }

        public bool ContainsKey(string viewTable)
        {
            return mViewDic.ContainsKey(viewTable);
        }

        public List<string> GetCacheTables()
        {
            List<string> tableList = new List<string>();
            foreach (IC_ViewInfo IcViewInfo in GameViewList)
            {
                if (IcViewInfo.autoLoad)
                {
                    tableList.Add(IcViewInfo.Table);
                }
            }
            return tableList;
        }

        public IC_IViewInfo OnAddView(string viewTable)
        {
            return mViewDic[viewTable];
        }

        public void OnRemoveView(string viewTable) {  }

        public void OnDispose()
        {

        }
    }
}