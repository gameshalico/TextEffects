using System;
using UnityEngine;

namespace TextEffects.Data
{
    public struct Quad
    {
        public Vector3 BottomLeft;
        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomRight;

        public Vector3 Center => (BottomLeft + TopRight) / 2f;
        public Vector3 Size => TopRight - BottomLeft;

        public Quad(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
        {
            BottomLeft = bottomLeft;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
        }

        public Quad ApplyMatrix(Matrix4x4 matrix)
        {
            return new Quad
            {
                BottomLeft = matrix.MultiplyPoint3x4(BottomLeft),
                TopLeft = matrix.MultiplyPoint3x4(TopLeft),
                TopRight = matrix.MultiplyPoint3x4(TopRight),
                BottomRight = matrix.MultiplyPoint3x4(BottomRight)
            };
        }

        public static Quad ApplyMatrix(in Quad quad, Matrix4x4 matrix)
        {
            return new Quad
            {
                BottomLeft = matrix.MultiplyPoint3x4(quad.BottomLeft),
                TopLeft = matrix.MultiplyPoint3x4(quad.TopLeft),
                TopRight = matrix.MultiplyPoint3x4(quad.TopRight),
                BottomRight = matrix.MultiplyPoint3x4(quad.BottomRight)
            };
        }

        public Vector3 this[int index]
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
            set
            {
                switch (index)
                {
                    case 0:
                        BottomLeft = value;
                        break;
                    case 1:
                        TopLeft = value;
                        break;
                    case 2:
                        TopRight = value;
                        break;
                    case 3:
                        BottomRight = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public static Quad operator +(in Quad quad, Vector3 offset)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft + offset,
                TopLeft = quad.TopLeft + offset,
                TopRight = quad.TopRight + offset,
                BottomRight = quad.BottomRight + offset
            };
        }

        public static Quad operator -(in Quad quad, Vector3 offset)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft - offset,
                TopLeft = quad.TopLeft - offset,
                TopRight = quad.TopRight - offset,
                BottomRight = quad.BottomRight - offset
            };
        }

        public static Quad operator +(in Quad quad, in Quad other)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft + other.BottomLeft,
                TopLeft = quad.TopLeft + other.TopLeft,
                TopRight = quad.TopRight + other.TopRight,
                BottomRight = quad.BottomRight + other.BottomRight
            };
        }

        public static Quad operator -(in Quad quad, in Quad other)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft - other.BottomLeft,
                TopLeft = quad.TopLeft - other.TopLeft,
                TopRight = quad.TopRight - other.TopRight,
                BottomRight = quad.BottomRight - other.BottomRight
            };
        }

        public static Quad operator *(in Quad quad, float scale)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft * scale,
                TopLeft = quad.TopLeft * scale,
                TopRight = quad.TopRight * scale,
                BottomRight = quad.BottomRight * scale
            };
        }

        public static Quad operator /(in Quad quad, float scale)
        {
            return new Quad
            {
                BottomLeft = quad.BottomLeft / scale,
                TopLeft = quad.TopLeft / scale,
                TopRight = quad.TopRight / scale,
                BottomRight = quad.BottomRight / scale
            };
        }
    }
}