using System;
using System.Collections.Generic;

namespace IcecreamView
{

    public class IC_UIEventManager
    {
        private Dictionary<int, List<Action<EventArg>>> eventDict = new Dictionary<int, List<Action<EventArg>>>();
        
        public void BindEvent(int code , Action<EventArg> eventCallback)
        {
            if (this.eventDict.ContainsKey(code))
            {
                if (!this.eventDict[code].Contains(eventCallback))
                    this.eventDict[code].Add(eventCallback);
            }
            else
            {
                this.eventDict[code] = new List<Action<EventArg>>();
                this.eventDict[code].Add(eventCallback);
            }
        }

        public void UnBindEvent(int code , Action<EventArg> eventCallback)
        {
            if (this.eventDict.ContainsKey(code))
            {
                this.eventDict[code].Remove(eventCallback);
            }
        }

        public void SendEvent(int code , params object[] values)
        {
            this.SendEvent(code, new EventArg(values));
        }

        public void SendEvent(int code , EventArg eventArg)
        {
            if (this.eventDict.ContainsKey(code))
            {
                this.eventDict[code].ForEach(action =>
                {
                    action.Invoke(eventArg);
                });
            }
        }
    }
}