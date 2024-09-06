using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    internal class TextAnimationApplier
    {
        private readonly TMP_Text _tmpText;
        private readonly ITextAnimationHandler _handler;

        private TMP_MeshInfo[] _cachedMeshInfos;
        private CharacterAnimationInfo[] _animationInfos;

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
            _animationInfos = new CharacterAnimationInfo[characterCount];

            UpdateTextInfo(textInfo);
        }

        public void Update()
        {
            if (!_isPlaying)
                return;

            if (_tmpText.textInfo.characterCount == 0)
                return;
            UpdateTextInfo(_tmpText.textInfo);
            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        private void UpdateTextInfo(TMP_TextInfo textInfo)
        {
            var characterCount = textInfo.characterCount;
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

                var destVertices = meshInfo.vertices;
                var destColors = meshInfo.colors32;

                // Initialize character animation state
                if (_animationInfos[characterIndex].IsInitialized == false)
                    _animationInfos[characterIndex] = new CharacterAnimationInfo
                    {
                        IsInitialized = true,
                        CharacterIndex = characterIndex,
                        BaseQuad = new Quad(sourceVertices[vertexIndex + 0], sourceVertices[vertexIndex + 1],
                            sourceVertices[vertexIndex + 2], sourceVertices[vertexIndex + 3]),
                        BaseColor = new VertexColor(sourceColors[vertexIndex], sourceColors[vertexIndex + 1],
                            sourceColors[vertexIndex + 2], sourceColors[vertexIndex + 3])
                    };

                // Update character animation state
                _animationInfos[characterIndex].Quad = _animationInfos[characterIndex].BaseQuad;
                _animationInfos[characterIndex].Color = _animationInfos[characterIndex].BaseColor;

                _handler.UpdateCharacter(ref characterInfo, ref _animationInfos[characterIndex]);

                // Apply animation state
                for (var i = 0; i < 4; i++)
                    destColors[vertexIndex + i] = _animationInfos[characterIndex].Color[i];

                for (var i = 0; i < 4; i++)
                    destVertices[vertexIndex + i] = _animationInfos[characterIndex].Quad[i];
            }
        }
    }
}