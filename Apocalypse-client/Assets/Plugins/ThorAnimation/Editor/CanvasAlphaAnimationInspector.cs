using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThorAnimation
{
    [CustomEditor(typeof(CanvasAlphaAnimation),true)]
    [ExecuteInEditMode]
    public class CanvasAlphaAnimationInspector : Editor
    {
        
        public override void OnInspectorGUI()
        {
            this.DrawTitle();
            GUILayout.Space(20f);

            CanvasAlphaAnimation rTarget = (CanvasAlphaAnimation) target;

            rTarget.DurTime = EditorGUILayout.FloatField("动画时长", rTarget.DurTime);

            rTarget.IsAutoPlay = EditorGUILayout.Toggle("自动播放", rTarget.IsAutoPlay);
            rTarget.IsLoop = EditorGUILayout.Toggle("是否循环", rTarget.IsLoop);
            rTarget.AniEase = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("动画播放曲线", rTarget.AniEase);
            


            rTarget.IsAutoStartValue = EditorGUILayout.Toggle("是否自动填入起始值", rTarget.IsAutoStartValue);

            if (!rTarget.IsAutoStartValue)
            {
                rTarget.StarValue = EditorGUILayout.Slider("起始值", rTarget.StarValue, 0f, 1f);
            }
            rTarget.EndValue = EditorGUILayout.Slider("结束值", rTarget.EndValue, 0f, 1f);
            
            GUILayout.Space(20f);

            if (GUILayout.Button("播放动画", GUILayout.Height(40f)))
            {
                rTarget.Play();
            }
            if (GUILayout.Button("停止动画", GUILayout.Height(40f)))
            {
                rTarget.Stop();
            }
        }

        public void DrawTitle()
        {
            GUIStyle rGUIStyle = new GUIStyle();
            rGUIStyle.fontSize = 18;
            rGUIStyle.normal.textColor = new Color32(56, 56, 56, 255);
            rGUIStyle.normal.background = this.GetTexture2D();
            rGUIStyle.alignment = TextAnchor.MiddleCenter;
            rGUIStyle.margin = new RectOffset(0, 0, 8, 0);
            GUILayout.Box(" Canvas Alpha Animation ", rGUIStyle);
        }

        private Texture2D GetTexture2D()
        {
            Texture2D rTexture2D = new Texture2D(4, 4);
            Color32[] rColors = rTexture2D.GetPixels32();
            var rColor32 = new Color32(255, 0, 200, 255);
            for (int i = 0; i < rColors.Length; i++)
            {
                rColors[i] = rColor32;
            }
            rTexture2D.SetPixels32(rColors);
            rTexture2D.Apply();

            return rTexture2D;
        }
    
    
    }
}

