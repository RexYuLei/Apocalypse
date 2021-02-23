using System;

namespace IcecreamView
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BindUIEvent : Attribute
    {
        public int EventCode;

        public BindUIEvent (int eventCode)
        {
            this.EventCode = eventCode;
        }
    }
}