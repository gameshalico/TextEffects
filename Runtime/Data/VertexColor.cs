using System;
using UnityEngine;

namespace TextEffects.Data
{
    public readonly struct VertexColor
    {
        public Color32 BottomLeft { get; init; }
        public Color32 TopLeft { get; init; }
        public Color32 TopRight { get; init; }
        public Color32 BottomRight { get; init; }

        public VertexColor(Color32 color)
        {
            BottomLeft = color;
            TopLeft = color;
            TopRight = color;
            BottomRight = color;
        }

        public VertexColor(Color32 bottomLeft, Color32 topLeft, Color32 topRight, Color32 bottomRight)
        {
            BottomLeft = bottomLeft;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
        }

        public Color32 this[int index]
        {
            get
            {
                return index switch
                {
                    0 => BottomLeft,
                    1 => TopLeft,
                    2 => TopRight,
                    3 => BottomRight,
                    _ => throw new IndexOutOfRangeException()
                };
            }
        }

        public VertexColor WithAlpha(byte alpha)
        {
            return WithAlpha(this, alpha);
        }

        public static VertexColor WithAlpha(in VertexColor vertexColor, byte alpha)
        {
            return new VertexColor
            {
                BottomLeft = new Color32(vertexColor.BottomLeft.r, vertexColor.BottomLeft.g, vertexColor.BottomLeft.b,
                    alpha),
                TopLeft = new Color32(vertexColor.TopLeft.r, vertexColor.TopLeft.g, vertexColor.TopLeft.b, alpha),
                TopRight = new Color32(vertexColor.TopRight.r, vertexColor.TopRight.g, vertexColor.TopRight.b, alpha),
                BottomRight = new Color32(vertexColor.BottomRight.r, vertexColor.BottomRight.g,
                    vertexColor.BottomRight.b, alpha)
            };
        }

        public static VertexColor Lerp(in VertexColor a, in VertexColor b, float t)
        {
            return new VertexColor
            {
                BottomLeft = Color32.Lerp(a.BottomLeft, b.BottomLeft, t),
                TopLeft = Color32.Lerp(a.TopLeft, b.TopLeft, t),
                TopRight = Color32.Lerp(a.TopRight, b.TopRight, t),
                BottomRight = Color32.Lerp(a.BottomRight, b.BottomRight, t)
            };
        }

        public static implicit operator VertexColor(Color32 color)
        {
            return new VertexColor(color);
        }

        public static implicit operator VertexColor(Color color)
        {
            return new VertexColor(color);
        }

        public static implicit operator Color32(in VertexColor vertexColor)
        {
            return vertexColor.BottomLeft;
        }

        public static implicit operator Color(in VertexColor vertexColor)
        {
            return vertexColor.BottomLeft;
        }

        public Color32[] ToArray()
        {
            return new[] { BottomLeft, TopLeft, TopRight, BottomRight };
        }

        public static VertexColor FromArray(Color32[] colors)
        {
            return new VertexColor(colors[0], colors[1], colors[2], colors[3]);
        }
    }
}