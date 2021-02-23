using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IcecreamView
{
    /// <summary>
    /// 使用模板组合View模式的必要组件
    /// </summary>
    [DisallowMultipleComponent, AddComponentMenu("IceCreamView/Connector")]
    public class IC_ModuleConnector : IC_AbstractView
    {

        private List<IC_AbstractModule> gameViewAbstractModules = null;

        private Dictionary<int, List<Action<EventArg>>> eventDict;

        private RunType awaitType;

        private int awaitCount = 0;

        private bool isAwait = false;

        private Dictionary<string, IC_ViewData> values;

        /// <summary>
        /// 获取指定类型的组件列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetViewModuleList<T>() where T : IC_AbstractModule
        {
            List<T> modules = new List<T>();
            string  cname   = typeof(T).ToString();
            foreach (IC_AbstractModule key in gameViewAbstractModules)
            {
                if (key.GetType().Name.Equals(cname))
                {
                    modules.Add((T) key);
                }
            }

            return modules;
        }

        /// <summary>
        /// 获取该页面上第一个指定类型的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetViewModule<T>() where T : IC_AbstractModule
        {
            string cname = typeof(T).ToString();
            foreach (IC_AbstractModule key in gameViewAbstractModules)
            {
                if (key.GetType().Name.Equals(cname))
                {
                    return (T) key;
                }
            }

            return null;
        }


        //        [Header("是否启用UIPath功能(Bate)")]
        //        [Tooltip("UIPath可以更加方便的绑定场景中的对象，但是在同一View下启用该功能的组件数量越多越会影响效率")]
        //        public bool IsActiveUIPath = true;


        private void OnOpenModel()
        {
            isAwait = false;
            if (awaitType != RunType.OnOpen)
            {
                awaitCount = 0;
                awaitType  = RunType.OnOpen;
            }

            while (awaitCount < gameViewAbstractModules.Count)
            {
                gameViewAbstractModules[awaitCount].OnOpenView(this.values);
                awaitCount++;
                if (isAwait)
                {
                    return;
                }
            }

            awaitCount = 0;
        }

        internal override void OnOpenView(params IC_ViewData[] values)
        {
            this.values = new Dictionary<string, IC_ViewData>();
            foreach (IC_ViewData ViewData in values)
            {
                this.values[ViewData.code] = ViewData;
            }

            OnOpenModel();
        }


        internal override void OnCloseView()
        {
            isAwait = false;
            if (awaitType != RunType.OnClose)
            {
                awaitCount = 0;
                awaitType  = RunType.OnClose;
            }

            while (awaitCount < gameViewAbstractModules.Count)
            {
                gameViewAbstractModules[awaitCount].OnCloseView();
                awaitCount++;
                if (isAwait)
                {
                    return;
                }
            }

            awaitCount = 0;
            _directClose();
        }

        internal override void OnInitView()
        {
            awaitType      = RunType.OnInit;
            this.eventDict = new Dictionary<int, List<Action<EventArg>>>();
            if (gameViewAbstractModules == null)
            {
                gameViewAbstractModules = new List<IC_AbstractModule>();
                //获取当前对象上的所有View模块
                GetComponents(gameViewAbstractModules);
                //重新根据优先级排序
                gameViewAbstractModules.Sort((IC_AbstractModule a, IC_AbstractModule b) =>
                {
                    if (a.prioritylevel < b.prioritylevel)
                    {
                        return -1;
                    }
                    else if (a.prioritylevel > b.prioritylevel)
                    {
                        return 1;
                    }

                    return 0;
                });
                //初始化所有模块
                gameViewAbstractModules.ForEach(m => { m.ViewConnector = this; });
            }

            foreach (var item in gameViewAbstractModules)
            {
                if (item.IsActiveUIBinder)
                {
                    this._initUIPath(item);
                }
                var bindingFlag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
                var mMethods    = item.GetType().GetMethods(bindingFlag);
                //绑定事件
                this._initUIEvent(item, mMethods);
                //绑定按钮事件
                this._initUIButton(item, mMethods);
                item.OnInitView();
            }
        }

        internal override void OnDestoryView()
        {
            isAwait    = false;
            awaitCount = 0;
            this.removeEvent();
            foreach (var mAction in this.eventDict)
            {
            }

            while (awaitCount < gameViewAbstractModules.Count)
            {
                gameViewAbstractModules[awaitCount].OnDestoryView();
                awaitCount++;
            }
        }

        protected override bool _closeHook() { return false; }

        /// <summary>
        /// 停止该页面正在执行的生命周期 ,停止的范围仅限(OnOpen、OnClose)
        /// 不支持多线程操作
        /// </summary>
        /// <returns>用于重新恢复中断的生命周期执行器</returns>
        public Action Await()
        {
            isAwait = true;
            return Continue;
        }

        public void Continue()
        {
            if (!isAwait) return;
            isAwait = false;
            switch (awaitType)
            {
                case RunType.OnClose:
                    OnCloseView();
                    break;
                case RunType.OnOpen:
                    OnOpenModel();
                    break;
                default:
                    break;
            }
        }

        private void _initUIPath(object uiModule)
        {
            var bindingFlag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            var mFields     = uiModule.GetType().GetFields(bindingFlag);
            foreach (var mFieldInfo in mFields)
            {
                var mAttrObjs = mFieldInfo.GetCustomAttributes(typeof(BindUIPath), false);
                if (mAttrObjs.Length > 0)
                {
                    var mUiPath = mAttrObjs[0] as BindUIPath;
                    if (mUiPath != null)
                    {
                        Transform mTransform;
                        if (string.IsNullOrEmpty(mUiPath.Path))
                            mTransform = this.transform;
                        else
                            mTransform = this.transform.Find(mUiPath.Path);

                        if (mTransform)
                        {
                            if (mUiPath.IsArray)
                            {
                                if (typeof(IList).IsAssignableFrom(mFieldInfo.FieldType) && mFieldInfo.FieldType.IsGenericType)
                                {
                                    Type  UIType    = mFieldInfo.FieldType.GenericTypeArguments[0];
                                    IList ListValue = (IList) Activator.CreateInstance(mFieldInfo.FieldType);
                                    int   count     = 0;
                                    while (count < mTransform.childCount)
                                    {
                                        Transform itemTran = null;
                                        if (mUiPath.GetElementPath(count) == "*")
                                            itemTran = mTransform.GetChild(count);
                                        else
                                            itemTran = mTransform.Find(mUiPath.GetElementPath(count));

                                        if (itemTran != null)
                                        {
                                            object mValue = _getComponent(itemTran, UIType);

                                            if (mValue != null)
                                            {
                                                ListValue.Add(mValue);
                                            }
                                            else if (mUiPath.GetElementPath(count) != "*")
                                            {
                                                Debug.LogError("[IceCreamView] 指定路径的对象未包含组件: " + UIType, itemTran);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        count++;
                                    }

                                    mFieldInfo.SetValue(uiModule, ListValue);
                                }
                                else
                                {
                                    Debug.LogError("[IceCreamView] 字段不满足ListUI控件绑定规范，请使用List<T>或者其派生类: " + mFieldInfo.Name);
                                }
                            }
                            else
                            {
                                object mValue = this._getComponent(mTransform, mFieldInfo.FieldType);
                                if (mValue != null)
                                {
                                    mFieldInfo.SetValue(uiModule, mValue);
                                }
                                else
                                {
                                    Debug.LogError("[IceCreamView] 指定路径的对象未包含组件: " + mFieldInfo.FieldType.ToString() + " 字段名称：" + mFieldInfo.Name);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("[IceCreamView] 无效的UI地址: " + mUiPath.Path);
                        }
                    }
                }
            }
        }

        private object _getComponent(Transform targetObj, Type ComponentType)
        {
            if (ComponentType == typeof(GameObject))
                return targetObj.gameObject;
            if (typeof(Component).IsAssignableFrom(ComponentType) || ComponentType.IsInterface)
            {
                return targetObj.GetComponent(ComponentType);
            }

            Debug.LogError("[IceCreamView] 不支持获取该类型（仅支持GameObject、Component派生类、接口）: " + ComponentType.Name);
            return null;
        }

        private void _initUIEvent(object uiModule, MethodInfo[] mMethods)
        {
            
            foreach (var mMethodInfo in mMethods)
            {
                var mAttrObjs = mMethodInfo.GetCustomAttributes(typeof(BindUIEvent), false);
                if (mAttrObjs.Length > 0)
                {
                    var mAttrObj         = mAttrObjs[0] as BindUIEvent;
                    var methodParameters = mMethodInfo.GetParameters();
                    if (methodParameters.Length == 1 & methodParameters[0].ParameterType == typeof(EventArg))
                    {
                        Action<EventArg> action = (EventArg rArg) => { mMethodInfo.Invoke(uiModule, new object[] {rArg}); };
                        viewManager.EventManager.BindEvent(mAttrObj.EventCode, action);
                        addEvent(mAttrObj.EventCode, action);
                    }
                    else
                    {
                        Debug.LogError("[IceCreamView] 指定方法不符合规定，方法必须声明传参（EventArg） : " + mMethodInfo.Name, this);
                    }
                }
            }
        }

        private void _initUIButton(object uiModule, MethodInfo[] mMethods)
        {
            
            foreach (var mMethodInfo in mMethods)
            {
                var mAttrObjs = mMethodInfo.GetCustomAttributes(typeof(BindUIButton), false);
                if (mAttrObjs.Length > 0)
                {
                    var mAttrObj         = mAttrObjs[0] as BindUIButton;
                    var methodParameters = mMethodInfo.GetParameters();
                    if (methodParameters.Length == 0)
                    {
                        mAttrObj.AddListener(this.transform ,() => { mMethodInfo.Invoke(uiModule, null); });
                    }
                    else
                    {
                        Debug.LogError("[IceCreamView] 指定方法不符合规定，点击事件方法不能携带任何参数 : " + mMethodInfo.Name, this);
                    }
                }
            }
        }

        private void addEvent(int code, Action<EventArg> action)
        {
            if (!this.eventDict.ContainsKey(code))
                this.eventDict[code] = new List<Action<EventArg>>();
            this.eventDict[code].Add(action);
        }

        private void removeEvent()
        {
            if (this.eventDict.Count > 0)
            {
                foreach (var mEventListKey in this.eventDict.Keys)
                {
                    foreach (Action<EventArg> mAction in this.eventDict[mEventListKey])
                    {
                        this.viewManager.EventManager.UnBindEvent(mEventListKey, mAction);
                    }
                }
            }
        }

        private enum RunType
        {
            OnInit,
            OnOpen,
            OnClose
        }
    }
}