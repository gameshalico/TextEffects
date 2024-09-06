using UnityEngine;

namespace TextEffects.Data
{
    public struct AnimationCharacterInfo
    {
        public bool IsInitialized { get; init; }

        public int CharacterIndex { get; init; }

        public Quad BaseQuad { get; init; }
        public VertexColor BaseColor { get; init; }

        public Quad Quad { get; set; }
        public VertexColor Color { get; set; }

        public void ApplyMatrixOnCenter(Matrix4x4 matrix)
        {
            var center = BaseQuad.Center;
            Quad = Quad.ApplyMatrix(Quad - center, matrix) + center;
        }
    }
}