using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

namespace ThorAnimation
{
    [AddComponentMenu("CustomAnimation/TweenAnimation")]
    public class TweenAnimation : MonoBehaviour
    {
        public Transform Target;
    
        // 动画类型
        public enum AniType
        {
            None = 0,
            Move,
            Rotate,
            Scale,
            Color,
            ColorChild,
            LightIntensity,
            MeshOffset,
            ParticleStartColor,
            FillAmount,
        }
        public AniType AType;
        public bool Local;
        public Vector3 AniPos;
        public Vector3 AniEule;
        public Vector3 AniScale;
        public float BornAlpha;
        public float EndAlpha;
        public Color AniColor;
        public float AniFillAmount;
        public float LightIntensity;
        public Vector2 MeshOffset;
    
        public float Duration;
        public Ease AniEase;
        public float Delay;
        public bool Forward;
        public bool Loop;
        public bool AutoPlay;
        public bool Symmetry;
        public bool PlayWithRecover;
    
        // 回调类型
        public enum DelegateType
        {
            None = 0,
            ActiveOtherAni,
            SendMessage,
            Both,
        }
        public DelegateType DType;
        [HideInInspector]
        public GameObject DTObject;
        [HideInInspector]
        public string DTMes;
        [HideInInspector]
        public TweenAnimation DTAnimation;
    
        private Vector3 m_BornPos;
        private Vector3 m_BornEule;
        private Vector3 m_BornScale;
        private Color m_BornColor;
        private float m_BornFillAmount;
        private float m_BornIntensity;
        private Vector2 m_BornMeshOffest;
    
        private Tweener m_CurTween;
        private bool m_bSymmetry;
        private Vector2 m_CurMeshOffest;
        public System.Action FinishEvent;
    
        public bool UseSelfCurve;
        public AnimationCurve AniCurve = new AnimationCurve();
    
        public bool IsPlaying
        {
            get
            {
                if (m_CurTween == null)
                    return false;
                return m_CurTween.IsPlaying();
            }
        }
    
        private void Start()
        {
            if (Target == null)
            {
                Target = transform;
            }
            if (Local)
            {
                m_BornPos = Target.transform.localPosition;
                m_BornEule = Target.transform.localRotation.eulerAngles;
            }
            else
            {
                m_BornPos = Target.transform.position;
                m_BornEule = Target.transform.rotation.eulerAngles;
            }
            m_BornScale = Target.transform.localScale;
    
            if (Target.GetComponent<Graphic>() != null)
                m_BornColor =  Target.GetComponent<Graphic>().color;
            else if (Target.GetComponent<SpriteRenderer>() != null)
                m_BornColor = Target.GetComponent<SpriteRenderer>().color;
    
            if (AType == AniType.ParticleStartColor)
                m_BornColor = Target.GetComponent<ParticleSystem>().main.startColor.color;
    
            if (AType == AniType.FillAmount)
                m_BornFillAmount = Target.GetComponent<Image>().fillAmount;
            
    
            Light ss = Target.GetComponent<Light>();
            m_BornIntensity = ss != null ? ss.intensity : 0f;
            m_BornMeshOffest = new Vector2();
            if (Target.GetComponent<Renderer>() != null)
            {
                var matertial = Target.GetComponent<Renderer>().material;
                if (matertial != null)
                {
                    if (matertial.HasProperty("_SpeedX"))
                        m_BornMeshOffest.x = matertial.GetFloat("_SpeedX");
                    if (matertial.HasProperty("_SpeedY"))
                        m_BornMeshOffest.y = matertial.GetFloat("_SpeedY");
                }
            }
    
            if (AutoPlay) Play();
        }
    
        public void ResetBornPos(Vector3 disPos)
        {
            m_BornPos = Target.transform.localPosition;
            AniPos = m_BornPos + disPos;
        }
    
        public void Play(bool forward = true)
        {
            m_bSymmetry = Symmetry;
            Forward = forward;
            if (PlayWithRecover)
            {
                Recover();
            }
            if (Application.isPlaying)
            {
                StartCoroutine(DelayPlayAnimation(0.05f));
            }
            else
            {
                PlayAnimation();
            }
        }
    
        public void Stop()
        {
            m_CurTween.Kill();
        }
    
        private IEnumerator DelayPlayAnimation(float oTime)
        {
            yield return new WaitForSeconds(oTime);
            PlayAnimation();
        }
    
        private void PlayAnimation()
        {
            if (Target == null) Target = transform;
    
            switch (AType)
            {
                case AniType.Move:
                    {
                        if (Local)
                            m_CurTween = Target.DOLocalMove(Forward ? AniPos : m_BornPos, Duration);
                        else
                            m_CurTween = Target.DOMove(Forward ? AniPos : m_BornPos, Duration);
                    }
                    break;
                case AniType.Rotate:
                    {
                        if (Local)
                            m_CurTween = Target.DOLocalRotate(Forward ? AniEule : m_BornEule, Duration, RotateMode.FastBeyond360);
                        else
                            m_CurTween = Target.DORotate(Forward ? AniEule : m_BornEule, Duration, RotateMode.FastBeyond360);
                    }
                    break;
                case AniType.Scale:
                    {
                        m_CurTween = Target.DOScale(Forward ? AniScale : m_BornScale, Duration);
                    }
                    break;
                case AniType.Color:
                    {
                        Graphic gp = Target.GetComponent<Graphic>();
                        if (gp == null) gp = Target.GetComponentInChildren<Graphic>();
                        if (gp != null)
                            m_CurTween = gp.DOColor(Forward ? AniColor : m_BornColor, Duration);
                        else
                        {
                            SpriteRenderer sr = Target.GetComponent<SpriteRenderer>();
                            if (sr == null) sr = Target.GetComponentInChildren<SpriteRenderer>();
                            if (sr == null) return;
                            m_CurTween = sr.DOColor(Forward ? AniColor : m_BornColor, Duration);
                        }
                    }
                    break;
                case AniType.ParticleStartColor:
                    {
                        var par = Target.GetComponent<ParticleSystem>().main;
                        m_CurTween = DOTween.To
                        (
                            () => par.startColor.color,
                            (c) => par.startColor = c,
                            Forward ? AniColor : m_BornColor,
                            Duration
                        );
    
                    }
                    break;
                case AniType.LightIntensity:
                    {
                        Light ss = Target.GetComponent<Light>();
                        m_CurTween = ss.DOIntensity(Forward ? LightIntensity : m_BornIntensity, Duration);
                    }
                    break;
                case AniType.MeshOffset:
                    {
                        var matertial = Target.GetComponent<Renderer>().material;
                        m_CurTween = matertial.DOFloat(Forward ? MeshOffset.x : m_BornMeshOffest.x, "_SpeedX", Duration);
                        m_CurTween = matertial.DOFloat(Forward ? MeshOffset.y : m_BornMeshOffest.y, "_SpeedY", Duration);
                    }
                    break;
                case AniType.ColorChild:
                    {
                        var List = Target.GetComponentsInChildren<Graphic>();
                        foreach (var gp in List)
                        {
                            Color target = gp.color;
                            Color cur = gp.color;
                            target.a = Forward ? EndAlpha : BornAlpha;
                            m_CurTween = gp.DOColor(target, Duration);
                        }
    
                    }
                    break;
                case AniType.FillAmount:
                    {
                        var img = Target.GetComponent<Image>();
                        m_CurTween = DOTween.To
                        (
                            () => img.fillAmount,
                            (c) => img.fillAmount = c,
                            Forward ? AniFillAmount : m_BornFillAmount,
                            Duration
                        );
                    }
                    break;
    
                default:
                    return;
            }
            if (!UseSelfCurve)
                m_CurTween.SetEase(AniEase);
            else
                m_CurTween.SetEase(AniCurve);
    
            if (!Loop)
            {
                m_CurTween.SetUpdate(true);
    
                m_CurTween.OnComplete(delegate ()
                {
                    if (FinishEvent != null)
                        FinishEvent.Invoke();
    
                    if (m_bSymmetry)
                    {
                        AniReverse();
                        m_bSymmetry = !m_bSymmetry;
                        PlayAnimation();
                        return;
                    }
                    if (DTObject == null) return;
    
                    switch (DType)
                    {
                        case DelegateType.ActiveOtherAni:
                            {
                                if (DTAnimation) DTAnimation.Play();
                            }
                            break;
                        case DelegateType.SendMessage:
                            {
                                if (gameObject.activeSelf && DTObject.activeSelf)
                                    DTObject.SendMessage(DTMes);
                            }
                            break;
                        case DelegateType.Both:
                            {
                                if (gameObject.activeSelf && DTObject.activeSelf)
                                    DTObject.SendMessage(DTMes);
                                if (DTAnimation) DTAnimation.Play();
                            }
                            break;
                    }
                });
            }
            else
            {
                m_CurTween.OnComplete(delegate ()
                {
                    if (Symmetry)
                    {
                        AniReverse();
                    }
                    if (PlayWithRecover)
                    {
                        Recover();
                    }
                    PlayAnimation();
                });
            }
        }
    
        public void AniReverse()
        {
            Forward = !Forward;
        }
    
        private void Recover()
        {
            if (Local)
            {
                Target.transform.localPosition = m_BornPos;
                Target.transform.localRotation = Quaternion.Euler(m_BornEule);
            }
            else
            {
                Target.transform.position = m_BornPos;
                Target.transform.rotation = Quaternion.Euler(m_BornEule);
            }
            Target.transform.localScale = m_BornScale;
    
            if (Target.GetComponent<Graphic>() != null)
                Target.GetComponent<Graphic>().color = m_BornColor;
            else if (Target.GetComponent<SpriteRenderer>() != null)
                Target.GetComponent<SpriteRenderer>().color = m_BornColor;
            
            Light ss = Target.GetComponent<Light>();
            if (ss != null) ss.intensity = m_BornIntensity;
    
            m_BornMeshOffest = new Vector2();
            if (Target.GetComponent<Renderer>() != null)
            {
                var matertial = Target.GetComponent<Renderer>().material;
                if (matertial.HasProperty("_SpeedX"))
                    matertial.SetFloat("_SpeedX", m_BornMeshOffest.x);
                if (matertial.HasProperty("_SpeedY"))
                    matertial.SetFloat("_SpeedY", m_BornMeshOffest.y);
            }
    
            if (AType == AniType.FillAmount)
                Target.GetComponent<Image>().fillAmount = m_BornFillAmount;
        }
    }
}

