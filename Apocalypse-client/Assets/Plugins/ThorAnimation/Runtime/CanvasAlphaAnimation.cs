using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ThorAnimation
{
    [AddComponentMenu("CustomAnimation/CanvasAlphaAnimation")]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasAlphaAnimation : MonoBehaviour
    {
        private CanvasGroup mCanvasGroup;

        public float DurTime;
        public bool IsAutoPlay;
        public bool IsLoop;
        public Ease AniEase;

        public bool IsAutoStartValue;
        public float StarValue;
        public float EndValue;
        
        private Tweener mCurTween;

        private void Start()
        {
            this.mCanvasGroup = this.GetComponent<CanvasGroup>();

            if (IsAutoPlay)
            {
                this.Play();
            }
        }

        public void Play()
        {
            if (this.IsAutoStartValue)
            {
                this.mCanvasGroup.alpha = this.StarValue;
            }
            this.mCurTween = DOTween.To(() => this.StarValue, x => this.mCanvasGroup.alpha = x, this.EndValue,
                this.DurTime);
            this.mCurTween.SetEase(AniEase);
            if (this.IsLoop)
            {
                this.mCurTween.onComplete = () => this.Play();
            }
        }

        public void Stop()
        {
            this.mCurTween.Kill();
        }
    }
}
