using System.Collections.Generic;

namespace IcecreamView
{
    public class EventArg
    {
        
        private List<object> values;

        public EventArg(params object[] values)
        {
            this.values = new List<object>(values);
        }

        public T GetValue<T>(int index = 0)
        {
            if (this.values == null || index >= this.values.Count)
            {
                return default;
            }
            return (T)this.values[index];
        }
    }
}