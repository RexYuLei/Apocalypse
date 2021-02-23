using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IcecreamView {

    [CreateAssetMenu(fileName = "IC_Resource_ViewConfig", menuName = "IceCreamView/IceView Config（Resource link）", order = 89)]
    public class IC_Resource_ViewConfig : ScriptableObject, IC_IViewConfig
    {
        [Header("UI预制体相对Resources下的根目录")]
        public string ResourceViewPath;

        [Header("默认打开页面")]
        public string DefaultView;

        [Header("需要复用View白名单（关闭View后不会销毁，类似对象池存储）")]
        public List<string> persistenceList = new List<string>();

        private Dictionary<string, IC_IViewInfo> mViewDic;

        public bool ContainsKey(string viewTable)
        {
            if (mViewDic.ContainsKey(viewTable))
                return true;
            var view = Resources.Load<IC_AbstractView>(ResourceViewPath + viewTable);
            if (view != null)
            {
                var viewInfo = new IC_ViewInfo() { autoLoad = this.persistenceList.Contains(viewTable) , isOnce = !persistenceList.Contains(viewTable) , Table = viewTable , View = view};
                mViewDic.Add(viewTable , viewInfo);
                return true;
            }
            return false;
        }

        public List<string> GetCacheTables()
        {
            return this.persistenceList;
        }

        public string GetDefaultViewTable()
        {
            if(string.IsNullOrEmpty(DefaultView))
                return null;
            return DefaultView;
        }

        public IC_IViewInfo OnAddView(string viewTable)
        {
            return mViewDic[viewTable];
        }

        public void OnRemoveView(string viewTable)
        {
            
        }

        public void OnDispose()
        {
            mViewDic.Clear();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public void OnInit()
        {
            mViewDic = new Dictionary<string, IC_IViewInfo>();
        }
    }

}
