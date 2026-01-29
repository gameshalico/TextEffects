#if TEXTEFFECTS_UIELEMENTS_SUPPORT
using TextEffects.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextEffects.Core.UIElements
{
    internal sealed class UIElementsAnimationApplier
    {
        private readonly ITextAnimationHandler _handler;
        private AnimationTextInfo _animationInfo;
        private bool _isSetup;

        public UIElementsAnimationApplier(ITextAnimationHandler handler)
        {
            _handler = handler;
        }

        public void Refresh(int characterCount)
        {
            _animationInfo = new AnimationTextInfo(new AnimationCharacterInfo[characterCount]);
            _handler.Setup(new TextInfo { CharacterCount = characterCount });
            _isSetup = true;
        }

        public void ProcessGlyphs(TextElement.GlyphsEnumerable glyphs)
        {
            if (!_isSetup || _animationInfo.AnimationCharacterInfo == null)
            {
                return;
            }

            var count = glyphs.Count;
            if (count == 0 || count > _animationInfo.AnimationCharacterInfo.Length)
            {
                return;
            }

            // Pass 1: Extract base data and reset to base values
            var index = 0;
            foreach (var glyph in glyphs)
            {
                if (index >= _animationInfo.AnimationCharacterInfo.Length)
                {
                    break;
                }

                var vertices = glyph.vertices;
                ref var charInfo = ref _animationInfo.AnimationCharacterInfo[index];

                if (!charInfo.IsInitialized)
                {
                    charInfo = new AnimationCharacterInfo
                    {
                        IsInitialized = true,
                        CharacterIndex = index,
                        BaseQuad = ExtractQuad(vertices),
                        BaseColor = ExtractColor(vertices)
                    };
                }

                charInfo.Quad = charInfo.BaseQuad;
                charInfo.Color = charInfo.BaseColor;
                index++;
            }

            // Let effects modify the animation info
            _handler.UpdateText(_animationInfo);

            // Pass 2: Apply the modified values to the vertices
            index = 0;
            foreach (var glyph in glyphs)
            {
                if (index >= _animationInfo.AnimationCharacterInfo.Length)
                {
                    break;
                }

                ApplyToGlyph(glyph.vertices, _animationInfo.AnimationCharacterInfo[index]);
                index++;
            }
        }

        private static Quad ExtractQuad(NativeSlice<Vertex> vertices)
        {
            return new Quad(
                vertices[0].position,
                vertices[1].position,
                vertices[2].position,
                vertices[3].position
            );
        }

        private static VertexColor ExtractColor(NativeSlice<Vertex> vertices)
        {
            return new VertexColor(
                vertices[0].tint,
                vertices[1].tint,
                vertices[2].tint,
                vertices[3].tint
            );
        }

        private static void ApplyToGlyph(NativeSlice<Vertex> vertices, in AnimationCharacterInfo info)
        {
            var v0 = vertices[0];
            var v1 = vertices[1];
            var v2 = vertices[2];
            var v3 = vertices[3];

            v0.position = info.Quad.BottomLeft;
            v1.position = info.Quad.TopLeft;
            v2.position = info.Quad.TopRight;
            v3.position = info.Quad.BottomRight;

            v0.tint = info.Color.BottomLeft;
            v1.tint = info.Color.TopLeft;
            v2.tint = info.Color.TopRight;
            v3.tint = info.Color.BottomRight;

            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;
            vertices[3] = v3;
        }
    }
}
#endif
