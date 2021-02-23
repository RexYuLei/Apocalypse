using System;
using System.Collections.Generic;
using UnityEngine;

namespace IcecreamView
{
    /// <summary>
    /// icecream页面管理器，驱动页面的核心控制器，用于控制页面生成、展示、隐藏、跳转等操作
    /// </summary>
    public sealed class IC_Controller : IDisposable
    {

        public static IC_Controller InstantiateViewManager(IC_IViewConfig gameViewConfig, Transform parent) { return new IC_Controller(gameViewConfig, parent); }

        private IC_IViewConfig Config;

        private List<IC_AbstractView> ViewPool;

        public Transform UIparent;

        public IC_UIEventManager EventManager { get; private set; }

        public IC_Controller(IC_IViewConfig gameViewConfig, Transform parent) { Init(gameViewConfig, parent); }

        private void Init(IC_IViewConfig gameViewConfig, Transform parent)
        {
            UIparent     = parent;
            EventManager = new IC_UIEventManager();
            ViewPool     = new List<IC_AbstractView>();
            this.ResetConfig(gameViewConfig);
        }

        #region UI Method

        /// <summary>
        /// 构建一个View对象
        /// </summary>
        /// <param name="Table">ViewTable</param>
        /// <returns></returns>
        private IC_AbstractView CreateView(string Table)
        {
            var             viewModle        = Config.OnAddView(Table);
            IC_AbstractView gameViewAbstract = UnityEngine.Object.Instantiate(viewModle.GetView(), UIparent);
            gameViewAbstract.VIEWTABLE = Table;
            if (viewModle.IsOnce())
            {
                gameViewAbstract.isOnce = viewModle.IsOnce();
            }
            else
            {
                gameViewAbstract.isOnce = ContainsKeyView(Table);
            }

            gameViewAbstract.SetViewManager(this);
            gameViewAbstract.isOpen = false;
            gameViewAbstract.OnInitView();
            return gameViewAbstract;
        }

        /// <summary>
        /// 获取指定页面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public T GetView<T>(string table) where T : IC_AbstractView
        {
            if (ContainsKeyView(table))
            {
                for (int i = 0; i < ViewPool.Count; i++)
                {
                    if (table.Equals(ViewPool[i].VIEWTABLE))
                    {
                        return (T) ViewPool[i];
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// 获取指定table页面的指定Module
        /// </summary>
        /// <param name="table"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetViewModule<T>(string table) where T : IC_AbstractModule
        {
            var View = GetView<IC_ModuleConnector>(table);
            if (View != null)
            {
                return View.GetViewModule<T>();
            }

            return null;
        }

        public IC_AbstractView OpenView(string table, int sortingLayer, bool isSinge = true, params IC_ViewData[] values)
        {
            int viewCount = getViewIndex(table, isSinge);
            if (viewCount != -1)
            {
                ViewPool[viewCount].isOpen = true;
                ViewPool[viewCount].gameObject.SetActive(true);
                // int layer = ViewPool[viewCount].transform.parent.childCount - sortOrder;
                // if (layer < 0)
                //     layer = 0;
                // ViewPool[viewCount].transform.SetSiblingIndex(layer);
                this.SetSortOrder(this.ViewPool[viewCount], sortingLayer);
                ViewPool[viewCount].OnOpenView(values);
                return ViewPool[viewCount];
            }

            if (Config.ContainsKey(table))
            {
                IC_AbstractView gameViewAbstract = CreateView(table);
                gameViewAbstract.isOpen = true;
                gameViewAbstract.gameObject.SetActive(true);
                // int layer = gameViewAbstract.transform.parent.childCount - sortOrder;
                // if (layer < 0)
                //     layer = 0;
                // gameViewAbstract.transform.SetSiblingIndex(layer);
                this.SetSortOrder(gameViewAbstract, sortingLayer);
                gameViewAbstract.OnOpenView(values);
                ViewPool.Add(gameViewAbstract);
                return gameViewAbstract;
            }

            Debug.LogFormat("<color=yellow>{0}</color>", "IC_Controller : 打开view失败，未找到指定table --- " + table);
            return null;
        }

        public void SetSortOrder(IC_AbstractView view, int sortingLayer)
        {
            view.SortOrder = sortingLayer;
            view.transform.SetAsLastSibling();
            for (int i = this.UIparent.childCount - 2; i >= 0; i--)
            {
                var tView = this.UIparent.GetChild(i).GetComponent<IC_AbstractView>();
                if (tView != null && tView.gameObject.activeSelf)
                {
                    if (view.SortOrder >= tView.SortOrder)
                        break;
                    else
                        view.transform.SetSiblingIndex(tView.transform.GetSiblingIndex());
                }
            }
        }

        public IC_AbstractView OpenView(string table, bool isSinge = true, params IC_ViewData[] values) { return OpenView(table, 0, isSinge, values); }

        public IC_AbstractView OpenView(string table, params IC_ViewData[] values) { return OpenView(table, 0, true, values); }

        /// <summary>
        /// 关闭所有页面 并打开指定页面
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IC_AbstractView OpenViewAndCloseOther(string table, bool isSinge, params IC_ViewData[] values)
        {
            if (table == null)
            {
                return null;
            }

            this.CloseAllView();
            IC_AbstractView view = OpenView(table, isSinge, values);
            return view;
        }

        /// <summary>
        /// 提前缓存指定Table数组
        /// </summary>
        /// <param name="Tables"></param>
        public void CacheView(List<string> Tables)
        {
            foreach (var Table in Tables)
            {
                if (this.Config.ContainsKey(Table))
                {
                    CacheView(Table);
                }
            }
        }

        /// <summary>
        /// 缓存指定页面
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public void CacheView(string Table)
        {
            var view = CreateView(Table);
            view.gameObject.SetActive(false);
            this.ViewPool.Add(view);
        }

        /// <summary>
        /// 判断指定table是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool ContainsKeyView(string table)
        {
            for (int i = 0; i < ViewPool.Count; i++)
            {
                if (table.Equals(ViewPool[i].VIEWTABLE))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 返回一个指定table的View对应下标
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private int getViewIndex(string table, bool isSinge = false, bool objectType = false)
        {
            for (int i = 0; i < ViewPool.Count; i++)
            {
                if (table.Equals(ViewPool[i].VIEWTABLE))
                {
                    if (isSinge)
                    {
                        return i;
                    }
                    else if (ViewPool[i].gameObject.activeSelf == objectType)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 关闭指定Table的页面
        /// </summary>
        /// <param name="table"></param>
        public void CloseView(string table)
        {
            var count = getViewIndex(table, false, true);
            if (count == -1) return;
            ViewPool[count].CloseView();
        }

        /// <summary>
        /// 关闭指定类型的所有页面
        /// </summary>
        /// <param name="table"></param>
        public void CloseTableViews(string table)
        {
            if (table == null) return;
            for (int i = 0; i < ViewPool.Count; i++)
            {
                if (table.Equals(ViewPool[i].VIEWTABLE))
                {
                    ViewPool[i].CloseView();
                }
            }
        }

        /// <summary>
        /// 关闭所有页面
        /// </summary>
        public void CloseAllView()
        {
            for (int i = ViewPool.Count - 1; i >= 0; i--)
            {
                if (this.ViewPool[i].isOpen)
                {
                    ViewPool[i].CloseView();
                }
            }
        }

        public void DestroyView(string table)
        {
            int count = getViewIndex(table, false, true);
            if (count != -1)
            {
                this.ViewPool[count].isOnce = true;
                this.ViewPool[count].CloseView();
            }
        }

        /// <summary>
        /// 直接销毁所有View，不会触发任何生命周期
        /// </summary>
        public void ClearAll()
        {
            this.ViewPool.ForEach(view => UnityEngine.Object.Destroy(view.gameObject));
            this.ViewPool.Clear();
        }

        /// <summary>
        /// 销毁指定的view
        /// </summary>
        /// <param name="hash"></param>
        internal void DestroyViewAtHash(int hash)
        {
            for (int i = 0; i < ViewPool.Count; i++)
            {
                if (ViewPool[i].gameObject.GetHashCode() == hash)
                {
                    GameObject.Destroy(ViewPool[i].gameObject);
                    this.Config.OnRemoveView(this.ViewPool[i].VIEWTABLE);
                    ViewPool.Remove(ViewPool[i]);
                    return;
                }
            }
        }

        #endregion

        /// <summary>
        /// 重置view配置表
        /// </summary>
        /// <param name="ViewConfig"></param>
        public void ResetConfig(IC_IViewConfig ViewConfig)
        {
            if (ViewConfig == null)
            {
                Debug.LogError("IceCreamView Init error, Config is Null");
                return;
            }

            this.ClearAll();
            this.Config?.OnDispose();
            this.Config = ViewConfig;
            this.Config.OnInit();
            //缓存页面
            this.CacheView(this.Config.GetCacheTables());
            if (!string.IsNullOrEmpty(Config.GetDefaultViewTable()))
            {
                OpenView(Config.GetDefaultViewTable());
            }
        }

        public void Dispose()
        {
            if (Config != null)
            {
                Config.OnDispose();
            }
        }

    }
}