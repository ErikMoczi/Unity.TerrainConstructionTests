using Unity.Mathematics;
using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Utils
{
    internal static class Common
    {
        public static float NoiseValue(Vector2 point, NoiseSettings config)
        {
            var frequency = config.Frequency;
            var amplitude = config.Amplitude;

            var range = 1f * amplitude;
            var value = GetNoise(point, frequency, amplitude);
            for (var o = 1; o < config.Octaves; o++)
            {
                frequency *= config.Lacunarity;
                amplitude *= config.Persistence;
                range += amplitude;
                value += GetNoise(point, frequency, amplitude);
            }

            return value * (1f / range);
        }

        private static float GetNoise(Vector2 point, float frequency, float amplitude)
        {
            return noise.cnoise(point * frequency) * amplitude;
        }

        public static Vector2 MiddlePosition(int x, int z, float stepSize, Vector2 offset)
        {
            var point00 = new Vector2(-0.5f, -0.5f) + offset;
            var point10 = new Vector2(0.5f, -0.5f) + offset;
            var point01 = new Vector2(-0.5f, 0.5f) + offset;
            var point11 = new Vector2(0.5f, 0.5f) + offset;

            var point0 = Vector2.Lerp(point00, point01, z * stepSize);
            var point1 = Vector2.Lerp(point10, point11, z * stepSize);

            return Vector2.Lerp(point0, point1, x * stepSize);
        }

        public static float2 SpiralChunkPosition(int n)
        {
//            https://stackoverflow.com/questions/398299/looping-in-a-spiral
            if (n == 0) return math.float2(0f);
            --n;
            var r = math.floor((math.sqrt(n + 1) - 1) / 2) + 1;
            var p = (8 * r * (r - 1)) / 2;
            var en = r * 2;
            var a = (1 + n - p) % (r * 8);
            var pos = new Vector3(0, 0, r);
            switch ((int) math.floor(a / (r * 2)))
            {
                // find the face : 0 top, 1 right, 2, bottom, 3 left
                case 0:
                {
                    pos.x = a - r;
                    pos.y = -r;
                    break;
                }
                case 1:
                {
                    pos.x = r;
                    pos.y = (a % en) - r;
                    break;
                }
                case 2:
                {
                    pos.x = r - (a % en);
                    pos.y = r;
                    break;
                }
                case 3:
                {
                    pos.x = -r;
                    pos.y = r - (a % en);
                    break;
                }
            }

            return math.float2(pos.x, pos.y);
        }
    }
}