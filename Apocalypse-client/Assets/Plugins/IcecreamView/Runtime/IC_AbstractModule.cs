using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IcecreamView {
    /// <summary>
    /// gameView模板类
    /// </summary>
    [RequireComponent(typeof(IC_ModuleConnector))]
    public abstract class IC_AbstractModule : MonoBehaviour
    {
        [Tooltip("执行优先级（越小越优先）")]
        public int prioritylevel = 1;
        
        [Tooltip("UIBinder可以更加方便的绑定场景中的对象，但在同一View下启用该功能的组件数量越多越会影响效率")]
        public bool IsActiveUIBinder = true;
        
        [HideInInspector]
        public IC_ModuleConnector ViewConnector;

        public virtual void OnOpenView(Dictionary<string,IC_ViewData> parameters) { }

        public virtual void OnCloseView() { }

        public virtual void OnInitView() { }

        public virtual void OnDestoryView() { }

    }
}

