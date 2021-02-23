using System.Collections.Generic;

namespace IcecreamView {
    public interface IC_IViewConfig 
    {
        void OnInit();

        string GetDefaultViewTable();

        bool ContainsKey(string viewTable);

        List<string> GetCacheTables();

        IC_IViewInfo OnAddView(string viewTable);

        void OnRemoveView(string viewTable);

        void OnDispose();
    }
}

