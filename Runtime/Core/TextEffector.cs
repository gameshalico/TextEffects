using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TextEffects.Core
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public partial class TextEffector : MonoBehaviour
    {
        private TextAnimationApplier _animationApplier;
        private TextAnimationHandler _animationHandler;
        private TextPreprocessor _textPreprocessor;

        public TMP_Text TMPText { get; private set; }

        private void Update()
        {
            _animationApplier?.Update();
        }

        private void OnEnable()
        {
            InitializeIfNeeded();

            TMPText.textPreprocessor = _textPreprocessor;

            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
            TMPText.OnPreRenderText += OnPreRenderText;

            TMPText.SetAllDirty();
        }

        private void OnDisable()
        {
            TMPText.textPreprocessor = null;

            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
            TMPText.OnPreRenderText -= OnPreRenderText;

            TMPText.SetAllDirty();
        }

        private void OnRenderObject()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
#endif
        }

        private void InitializeIfNeeded()
        {
            if (_animationApplier != null)
                return;

            TMPText = GetComponent<TMP_Text>();
            _animationHandler = new TextAnimationHandler();
            _animationApplier = new TextAnimationApplier(TMPText, _animationHandler);
            _textPreprocessor = new TextPreprocessor(this);
        }

        public void SetDirty()
        {
            InitializeIfNeeded();
            TMPText.SetAllDirty();
        }

        private void OnTextChanged(Object obj)
        {
            if (obj != TMPText)
                return;
        }

        private void OnPreRenderText(TMP_TextInfo info)
        {
            _animationApplier.Refresh(TMPText.textInfo);
        }
    }
}