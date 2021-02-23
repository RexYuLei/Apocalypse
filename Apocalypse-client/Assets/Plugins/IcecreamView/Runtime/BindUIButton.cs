using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace IcecreamView
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BindUIButton : Attribute
    {
        private string[] _eventPaths;

        public BindUIButton(params string[] buttonPaths) 
        {
            this._eventPaths = buttonPaths;
        }
        
        internal void AddListener(Transform obj, UnityAction action)
        {
            if (_eventPaths == null || _eventPaths.Length == 0)
                return;
            foreach (var path in this._eventPaths)
            {
                GetButtonComponent(obj , path)?.onClick.AddListener(action);
            }
        }

        private Button GetButtonComponent(Transform obj,  string path)
        {
            Transform mT;
            if (!string.IsNullOrEmpty(path))
                mT = obj.Find(path);
            else
                mT = obj;
            var btn = mT?.GetComponent<Button>();
            if (mT == null)
            {
                Debug.LogError("[IceCreamView] 指定路径无效 : " + path, obj);
            }
            else if (btn == null)
            {
                Debug.LogError("[IceCreamView] 指定路径下没有Button组件: " + path, obj);
            }

            return btn;
        }

    }
}