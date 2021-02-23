using System.Collections.Generic;

namespace IcecreamView
{
    public class IC_ViewData
    {
        public string code;
        public List<object> values;

        public IC_ViewData (string code , params object[] values)
        {
            this.code = code;
            this.values = new List<object>();
            this.values.AddRange(values);
        }
        public T GetValue<T>(int index = 0)
        {
            return (T) this.values[index];
        }
        
        public object GetValue(int index = 0)
        {
            return  this.values[index];
        }
    }
}