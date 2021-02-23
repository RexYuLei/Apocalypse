using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThorAnimation
{ 
    [CustomEditor(typeof(TweenAnimation), true)]
    [ExecuteInEditMode]
    public class TweenAnimationInspector : Editor
    {
        private void OnEnable()
        {
        }
    
        public override void OnInspectorGUI()
        {
            this.DrawTitle();
            GUILayout.Space(20f);
            
            TweenAnimation mTarget = (TweenAnimation) target;
            
            mTarget.Target = (Transform)EditorGUILayout.ObjectField("动画播放对象", mTarget.Target, typeof(Transform), true);
            mTarget.AType = (TweenAnimation.AniType)EditorGUILayout.EnumPopup("动画类型", mTarget.AType);
            
            switch (mTarget.AType)
            {
                case TweenAnimation.AniType.Move:
                    {
                        mTarget.AniPos = EditorGUILayout.Vector3Field("移动位置", mTarget.AniPos);
                    }
                    break;
                case TweenAnimation.AniType.Rotate:
                    {
                        mTarget.AniEule = EditorGUILayout.Vector3Field("旋转角度", mTarget.AniEule);
                    }
                    break;
                case TweenAnimation.AniType.Scale:
                    {
                        mTarget.AniScale = EditorGUILayout.Vector3Field("变化大小", mTarget.AniScale);
                    }
                    break;
                case TweenAnimation.AniType.Color:
                case TweenAnimation.AniType.ParticleStartColor:
                    {
                        mTarget.AniColor = EditorGUILayout.ColorField("渐变颜色", mTarget.AniColor);
                    }
                    break;
                case TweenAnimation.AniType.ColorChild:
                    {
                        mTarget.BornAlpha = EditorGUILayout.FloatField("初始透明度", mTarget.BornAlpha);
                        mTarget.EndAlpha = EditorGUILayout.FloatField("目标透明度", mTarget.EndAlpha);
                    }
                    break;
                case TweenAnimation.AniType.LightIntensity:
                    {
                        mTarget.LightIntensity = EditorGUILayout.Slider("光线范围", mTarget.LightIntensity, 0f, 8f);
                    }
                    break;
                case TweenAnimation.AniType.MeshOffset:
                    {
                        mTarget.MeshOffset = EditorGUILayout.Vector3Field("Mesh速度范围", mTarget.MeshOffset);
                    }
                    break;
                case TweenAnimation.AniType.FillAmount:
                    {
                        mTarget.AniFillAmount = EditorGUILayout.FloatField("填充值", mTarget.AniFillAmount);
                    }
                    break;
            }
            
            GUILayout.Space(20f);
    
            mTarget.Local = EditorGUILayout.Toggle("相对坐标系", mTarget.Local);
    
            mTarget.Forward = EditorGUILayout.Toggle("正向播放", mTarget.Forward);
    
            mTarget.Loop = EditorGUILayout.Toggle("循环播放", mTarget.Loop);
    
            mTarget.AutoPlay = EditorGUILayout.Toggle("自动播放", mTarget.AutoPlay);
    
            mTarget.Symmetry = EditorGUILayout.Toggle("反复播放", mTarget.Symmetry);
    
            mTarget.PlayWithRecover = EditorGUILayout.Toggle("复位播放", mTarget.PlayWithRecover);
    
            GUILayout.Space(20f);
    
            mTarget.Duration = EditorGUILayout.DelayedFloatField("动画时长", mTarget.Duration);
    
            mTarget.Delay = EditorGUILayout.DelayedFloatField("动画延时", mTarget.Delay);
    
            mTarget.UseSelfCurve = EditorGUILayout.Toggle("使用自己的曲线", mTarget.UseSelfCurve);
    
            if(!mTarget.UseSelfCurve)
                mTarget.AniEase = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("动画播放曲线", mTarget.AniEase);
            else
                mTarget.AniCurve = EditorGUILayout.CurveField( "动画播放曲线", mTarget.AniCurve, Color.red,new Rect(0.0f, 0.0f, 1.0f, 1.0f));
            GUILayout.Space(20f);
    
    
            mTarget.DType = (TweenAnimation.DelegateType)EditorGUILayout.EnumPopup("动画回调", mTarget.DType);
    
            switch (mTarget.DType)
            {
                case TweenAnimation.DelegateType.None:
                    break;
                case TweenAnimation.DelegateType.ActiveOtherAni:
                    {
                        mTarget.DTAnimation = (TweenAnimation)EditorGUILayout.ObjectField("动画目标", mTarget.DTAnimation, typeof(TweenAnimation), true);
                    }
                    break;
                case TweenAnimation.DelegateType.SendMessage:
                    {
                        mTarget.DTObject = (GameObject)EditorGUILayout.ObjectField("消息目标", mTarget.DTObject, typeof(GameObject), true);
                        mTarget.DTMes = EditorGUILayout.TextField("Message", mTarget.DTMes);
                    }
                    break;
                case TweenAnimation.DelegateType.Both:
                    {
                        mTarget.DTAnimation = (TweenAnimation)EditorGUILayout.ObjectField("动画目标", mTarget.DTAnimation, typeof(TweenAnimation), true);
                        mTarget.DTObject = (GameObject)EditorGUILayout.ObjectField("消息目标", mTarget.DTObject, typeof(GameObject), true);
                        mTarget.DTMes = EditorGUILayout.TextField("Message", mTarget.DTMes);
                    }
                    break;
            }
    
            GUILayout.Space(40f);
            if (GUILayout.Button("播放动画", GUILayout.Height(40f)))
            {
                mTarget.Play();
            }
    
            GUILayout.Space(20f);
            if (GUILayout.Button("停止动画", GUILayout.Height(40f)))
            {
                mTarget.Stop();
            }
        }
    
        void DrawTitle()
        {
            GUIStyle rGUIStyle = new GUIStyle();
            rGUIStyle.fontSize = 18;
            rGUIStyle.normal.textColor = new Color32(56, 56, 56, 255);
            rGUIStyle.normal.background = this.GetTexture2D();
            rGUIStyle.alignment = TextAnchor.MiddleCenter;
            rGUIStyle.margin = new RectOffset(0, 0, 8, 0);
            GUILayout.Box(" DT Animation ", rGUIStyle);
        }
    
        Texture2D GetTexture2D()
        {
            Texture2D rTexture2D = new Texture2D(4, 4);
            Color32[] rColors = rTexture2D.GetPixels32();
            var rColor = new Color32(138, 255, 0, 255);
            for (int i = 0; i < rColors.Length; i++)
            {
                rColors[i] = rColor;
            }
            rTexture2D.SetPixels32(rColors);
            rTexture2D.Apply();
            
            return rTexture2D;
        }
    }
}

