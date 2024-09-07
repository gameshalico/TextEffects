using UnityEngine;

namespace TextEffects.Core
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(TextEffector))]
    [ExecuteAlways]
    public abstract class TextEffectorFeature : MonoBehaviour
    {
        private TextEffector _textEffector;

        protected TextEffector Effector
        {
            get
            {
                if (!_textEffector)
                    _textEffector = GetComponent<TextEffector>();
                return _textEffector;
            }
        }

        private void OnEnable()
        {
            AddFeature(Effector);
            Effector.SetDirty();
        }

        private void OnDisable()
        {
            RemoveFeature(Effector);
            Effector.SetDirty();
        }

        protected abstract void AddFeature(TextEffector textEffector);
        protected abstract void RemoveFeature(TextEffector textEffector);

        protected void SetDirty()
        {
            if (isActiveAndEnabled)
                Effector.SetDirty();
        }
    }
}