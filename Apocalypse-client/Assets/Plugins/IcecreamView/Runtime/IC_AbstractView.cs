using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IcecreamView
{
    [DisallowMultipleComponent, System.Serializable]
    public abstract class IC_AbstractView : MonoBehaviour
    {
        /// <summary>
        /// ViewTable标识 不能修改
        /// </summary>
        [HideInInspector]
        public string VIEWTABLE;

        internal int SortOrder;

        [HideInInspector]
        ///用于判断页面是否开启
        public bool isOpen = false;

        [HideInInspector]
        public bool isOnce = false;

        /// <summary>
        /// 对应View管理器
        /// </summary>
        [System.NonSerialized] protected IC_Controller viewManager;

        public void SetViewManager(IC_Controller viewManager)
        {
            if (this.viewManager == null)
            {
                this.viewManager = viewManager;
            }
        }
        
        public void SendEvent(int code , params object[] value)
        {
            this.viewManager.EventManager.SendEvent(code , value);
        }

        /// <summary>
        /// 页面被创建初始化时触发该方法
        /// </summary>
        internal virtual void OnInitView() { }

        /// <summary>
        /// 页面打开时触发该方法
        /// </summary>
        internal virtual void OnOpenView(params IC_ViewData[] values) { }

        /// <summary>
        /// 页面被关闭前触发该方法
        /// </summary>
        internal virtual void OnCloseView() { }

        /// <summary>
        /// 页面被彻底销毁时触发
        /// </summary>
        internal virtual void OnDestoryView() { }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        public void CloseView()
        {
            if (!isOpen) {
                Debug.Log(VIEWTABLE +" view is Closed");
                return;
            }
            isOpen = false;
            OnCloseView();
            if (_closeHook())
            {
                _directClose();
            }
        }

        /// <summary>
        /// 用于截断页面关闭的钩子方法,默认返回true，如果返回false转为手动模式，需要自行close页面，适用于各种骚操作，请谨慎使用，可能会引起未知错误
        /// </summary>
        /// <returns></returns>
        protected virtual bool _closeHook()
        {
            return true;
        }

        /// <summary>
        /// 直接关闭页面，不会触发OnClose事件，通常情况下请使用CloseView方法，直接使用该方法可能导致不稳定出现位置问题
        /// </summary>
        protected void _directClose()
        {
            if (isOnce)
            {
                viewManager.DestroyViewAtHash(gameObject.GetHashCode());
            }
            else
            {
                gameObject.SetActive(false);
                transform.SetAsFirstSibling();
            }
        }

        /// <summary>
        /// 打开指定页面
        /// </summary>
        /// <param name="ViewTable">页面table</param>
        /// <param name="isCloseThis">是否同时关闭自己</param>
        public IC_AbstractView OpenView(string ViewTable, bool isCloseThis = false, bool isSinge = true , params IC_ViewData[] values)
        {
            if (viewManager != null)
            {
                if (isCloseThis)
                {
                    CloseView();
                }
                return viewManager.OpenView(ViewTable, isSinge, values);
            }
            return null;
        }

        public IC_AbstractView OpenView(string ViewTable, bool isCloseThis = false,  params IC_ViewData[] values)
        {
            return OpenView(ViewTable , isCloseThis , true , values);
        }
        
        public IC_AbstractView OpenView(string ViewTable,  params IC_ViewData[] values)
        {
            return OpenView(ViewTable , false , true , values);
        }

        private void OnDestroy()
        {
            OnDestoryView();
        }
    }
}


