using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    internal class TextAnimationApplier
    {
        private readonly TMP_Text _tmpText;
        private readonly ITextAnimationHandler _handler;

        private TMP_MeshInfo[] _cachedMeshInfos;
        private TextAnimationInfo _animationInfo;

        private bool _isPlaying;

        public TextAnimationApplier(TMP_Text tmpText, ITextAnimationHandler handler)
        {
            _tmpText = tmpText;
            _handler = handler;
        }

        public void Refresh(TMP_TextInfo textInfo)
        {
            if (_isPlaying)
                _handler.Release();
            _handler.Setup(textInfo);
            _isPlaying = true;

            var characterCount = textInfo.characterCount;
            _cachedMeshInfos = textInfo.CopyMeshInfoVertexData();
            _animationInfo = new TextAnimationInfo
            {
                TextInfo = textInfo,
                CharacterAnimationInfo = new CharacterAnimationInfo[characterCount]
            };

            UpdateTextInfo(textInfo);
        }

        public void Update()
        {
            if (!_isPlaying)
                return;

            if (_tmpText.textInfo.characterCount == 0)
                return;
            UpdateTextInfo(_tmpText.textInfo);
            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
        }

        private void UpdateTextInfo(TMP_TextInfo textInfo)
        {
            var characterCount = textInfo.characterCount;
            if (_animationInfo.CharacterAnimationInfo.Length != characterCount)
                return;

            for (var characterIndex = 0; characterIndex < characterCount; characterIndex++)
            {
                ref var characterInfo = ref textInfo.characterInfo[characterIndex];
                if (!characterInfo.isVisible) continue;

                // Get information about the character
                var vertexIndex = characterInfo.vertexIndex;
                var materialIndex = characterInfo.materialReferenceIndex;

                var meshInfo = textInfo.meshInfo[materialIndex];
                var cachedMeshInfo = _cachedMeshInfos[materialIndex];

                var sourceVertices = cachedMeshInfo.vertices;
                var sourceColors = cachedMeshInfo.colors32;

                // Initialize character animation state
                if (_animationInfo.CharacterAnimationInfo[characterIndex].IsInitialized == false)
                    _animationInfo.CharacterAnimationInfo[characterIndex] = new CharacterAnimationInfo
                    {
                        IsInitialized = true,
                        CharacterIndex = characterIndex,
                        BaseQuad = new Quad(sourceVertices[vertexIndex + 0], sourceVertices[vertexIndex + 1],
                            sourceVertices[vertexIndex + 2], sourceVertices[vertexIndex + 3]),
                        BaseColor = new VertexColor(sourceColors[vertexIndex], sourceColors[vertexIndex + 1],
                            sourceColors[vertexIndex + 2], sourceColors[vertexIndex + 3])
                    };

                // Update character animation state
                _animationInfo.CharacterAnimationInfo[characterIndex].Quad =
                    _animationInfo.CharacterAnimationInfo[characterIndex].BaseQuad;
                _animationInfo.CharacterAnimationInfo[characterIndex].Color =
                    _animationInfo.CharacterAnimationInfo[characterIndex].BaseColor;
            }

            _handler.UpdateText(_animationInfo);

            // Apply animation state
            for (var characterIndex = 0; characterIndex < characterCount; characterIndex++)
            {
                ref var characterInfo = ref textInfo.characterInfo[characterIndex];
                if (!characterInfo.isVisible) continue;

                var vertexIndex = characterInfo.vertexIndex;
                var materialIndex = characterInfo.materialReferenceIndex;

                var meshInfo = textInfo.meshInfo[materialIndex];
                var animationInfo = _animationInfo.CharacterAnimationInfo[characterIndex];

                var destVertices = meshInfo.vertices;
                var destColors = meshInfo.colors32;

                destVertices[vertexIndex + 0] = animationInfo.Quad.BottomLeft;
                destVertices[vertexIndex + 1] = animationInfo.Quad.TopLeft;
                destVertices[vertexIndex + 2] = animationInfo.Quad.TopRight;
                destVertices[vertexIndex + 3] = animationInfo.Quad.BottomRight;

                destColors[vertexIndex + 0] = animationInfo.Color.BottomLeft;
                destColors[vertexIndex + 1] = animationInfo.Color.TopLeft;
                destColors[vertexIndex + 2] = animationInfo.Color.TopRight;
                destColors[vertexIndex + 3] = animationInfo.Color.BottomRight;
            }
        }
    }
}