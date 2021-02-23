using System;
using System.Collections.Generic;

public static class UIPanel
{
    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public const string BeginPanel = "BeginPanel";

    public const string GamingPanel = "GamingPanel";

    public const string GameCompletePanel = "GameCompletePanel";

    public static List<string> GetAllPanelTable()
    {
        List<string> tables = new List<string>();

        Type UIPanelType = typeof(UIPanel);
        var FieldInfos = UIPanelType.GetFields();
        for (int i = 0; i < FieldInfos.Length; i++)
        {
            tables.Add(FieldInfos[i].GetValue(null).ToString());
        }
        return tables;
    }
}