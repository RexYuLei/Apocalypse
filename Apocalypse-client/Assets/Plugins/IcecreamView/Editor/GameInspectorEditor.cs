using UnityEngine;
using UnityEditor;

namespace IcecreamView.Editor
{
    [CustomEditor(typeof(IcecreamView.IC_AbstractView), true), CanEditMultipleObjects]
    public class GameViewAbstractEditor : UnityEditor.Editor
    {
        public static Texture2D GetTexture2D(Color32 color32)
        {
            Texture2D texturesss = new Texture2D(4, 4);
            Color32[] colors = texturesss.GetPixels32();
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color32;
            }
            texturesss.SetPixels32(colors);
            texturesss.Apply();
            return texturesss;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle gUIStyle = new GUIStyle();

            gUIStyle.fontSize = 20;
            gUIStyle.normal.textColor = Color.black;
            gUIStyle.normal.background = GameViewAbstractEditor.GetTexture2D(new Color32(0, 205, 255, 100));
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            gUIStyle.margin = new RectOffset(0, 0, 10, 0);

            GUILayout.Space(10);
            GUILayout.Box(" Game View ", gUIStyle);

            gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 16;
            gUIStyle.normal.textColor = Color.white;
            gUIStyle.normal.background = GameViewAbstractEditor.GetTexture2D(new Color32(77, 77, 77, 255));
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Box(this.target.GetType().Name, gUIStyle);

            EditorGUILayout.HelpBox("游戏界面基础组件", MessageType.None);
            GUILayout.Space(10);
            gUIStyle = null;
            base.DrawDefaultInspector();
            this.serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(IcecreamView.IC_ModuleConnector), true), CanEditMultipleObjects]
    public class GameViewModuleConnectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle gUIStyle = new GUIStyle();

            gUIStyle.fontSize = 20;
            gUIStyle.normal.textColor = Color.black;
            gUIStyle.normal.background = GameViewAbstractEditor.GetTexture2D(new Color32(255, 0, 99, 100));
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            gUIStyle.margin = new RectOffset(0, 0, 10, 0);


            GUILayout.Space(10);
            GUILayout.Box(" Game View Connect ", gUIStyle);
            EditorGUILayout.HelpBox("模块管理器,用于关联并激活该对象上的所有< Game View Module >", MessageType.None);
            GUILayout.Space(10);
            gUIStyle = null;
            this.serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(IcecreamView.IC_AbstractModule), true), CanEditMultipleObjects]
    public class GameViewAbstractModuleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle gUIStyle = new GUIStyle();

            gUIStyle.fontSize = 20;
            gUIStyle.normal.textColor = Color.black;
            gUIStyle.normal.background = GameViewAbstractEditor.GetTexture2D(new Color32(147, 255, 0, 100));
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            gUIStyle.margin = new RectOffset(0, 0, 10, 0);


            GUILayout.Space(10);
            GUILayout.Box(" Game View Module ", gUIStyle);

            gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 16;
            gUIStyle.normal.textColor = Color.white;
            gUIStyle.normal.background = GameViewAbstractEditor.GetTexture2D(new Color32(77, 77, 77, 255));
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Box(this.target.GetType().Name, gUIStyle);

            EditorGUILayout.HelpBox("界面功能模块组件,允许多模块自由组合", MessageType.None);
            GUILayout.Space(10);
            gUIStyle = null;
            base.DrawDefaultInspector();
            this.serializedObject.ApplyModifiedProperties();
        }
    }

//    public static class IceViewConfigTool{
//        [MenuItem("Assets/Create/IceCreamView/AutoView Config", false, 88)]
//        public static void CreatConfig() {
//            Object[] arr = Selection.GetFiltered(typeof(IC_AbstractView), SelectionMode.TopLevel);
//            var GameConfig = ScriptableObject.CreateInstance<IC_ViewConfig>();
//            List<IC_ViewInfo> gameInfos = new List<IC_ViewInfo>();
//            foreach (var item in arr)
//            {
//                var a = new IC_ViewInfo();
//                a.View = (IC_AbstractView)item;
//                a.Table = item.name;
//                gameInfos.Add(a);
//            }
//            GameConfig.GameViewList = gameInfos;
//            GameConfig.ConfigName = "defaultConfig";
//            if (arr.Length > 0) {
//                var filePath = AssetDatabase.GetAssetPath(arr[0]).Replace(arr[0].name + ".prefab", "viewConfig.asset");
//                AssetDatabase.CreateAsset(GameConfig, filePath);
//                Debug.LogFormat("Create Config : {0}" , filePath);
//            }
//            AssetDatabase.Refresh();
//        }
//    }

}

