
namespace IcecreamView {
    public interface IC_IViewInfo
    {
        string GetTable();
        bool IsOnce();
        bool IsCache();
        IC_AbstractView GetView();
    }
}

