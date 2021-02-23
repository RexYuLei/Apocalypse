using System;

namespace IcecreamView
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BindUIPath : Attribute
    {
        public string Path;

        private string ItemPath;

        private int StartIndex;

        public bool IsArray;
        
        /// <summary>
        /// 绑定一个UI控件
        /// </summary>
        /// <param name="path">控件相对该组件对应的相对地址</param>
        public BindUIPath(string path)
        {
            this.IsArray = false;
            this.Path = path;
        }

        /// <summary>
        /// 绑定一个List类型的UI控件
        /// </summary>
        /// <param name="path">该list的根节点路径</param>
        /// <param name="itemPath">每个item的路径，如果有递增类型的值请使用{n}关键字代替递增数字， *代表根节点下的所有对象</param>
        /// <param name="startIndex">递增值的初始值,默认为0</param>
        public BindUIPath(string path, string itemPath , int startIndex = 0 )
        {
            this.IsArray = true;
            if (itemPath == null)
            {
                itemPath = "*";
            }
            this.Path = path;
            this.ItemPath = itemPath;
            this.StartIndex = startIndex;
        }

        public string GetElementPath(int Index)
        {
            if (this.IsArray)
            {
                return this.ItemPath.Replace("{n}", Index + this.StartIndex + "");
            }
            return null;
        }
    }
}