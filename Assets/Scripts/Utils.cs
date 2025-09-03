using UnityEngine;

public static class Utils
{
    private static readonly System.Random s_random = new System.Random();

    public static int GenerateRandomIndex(int max) =>
         s_random.Next(max);

    public static int GenerateRandomNumber(int min, int max) =>
         s_random.Next(min, max + 1);

    public static string UpdateUIText(string valueName, int currentValue) =>
         $"{valueName}: {currentValue}";

    public static void DrawSphereZone(Color color, Vector2 centre, float range)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(centre, range);
    }
    public static class Maths
    {
        public static float Sqr(float value) => value * value;
        public static double Sqr(double value) => value * value;
        public static int Sqr(int value) => value * value;

        /// <summary>
        /// Вычисляет квадрат расстояния между двумя точками
        /// </summary>
        public static float SqrDistance(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;

        /// <summary>
        /// Вычисляет квадрат расстояния между двумя точками (2D версия)
        /// </summary>
        public static float SqrDistance(Vector2 a, Vector2 b) => (a - b).sqrMagnitude;

        /// <summary>
        /// Проверяет, находится ли точка в радиусе от другой точки (используя квадраты расстояний)
        /// </summary>
        public static bool IsInRange(Vector3 point, Vector3 center, float sqrRadius) =>
            SqrDistance(point, center) <= sqrRadius;

        /// <summary>
        /// Проверяет, находится ли точка в радиусе от другой точки (2D версия)
        /// </summary>
        public static bool IsInRange(Vector2 point, Vector2 center, float sqrRadius) =>
            SqrDistance(point, center) <= sqrRadius;

        /// <summary>
        /// Плавное приближение к цели
        /// </summary>
        public static float SmoothApproach(float current, float target, float speed, float deltaTime)
        {
            if (speed <= 0) return target;
            return Mathf.Lerp(current, target, 1 - Mathf.Exp(-speed * deltaTime));
        }
    }
#if UNITY_EDITOR
    public static void DrawDiscZone(Color color, Vector2 centre, float range)
    {
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(centre, Vector3.forward, range);
    }

    public static void DrawSolidDiscZone(Color color, Vector2 centre, float range)
    {
        UnityEditor.Handles.color = new Color(color.r, color.g, color.b, 0.1f);
        UnityEditor.Handles.DrawSolidDisc(centre, Vector3.forward, range);

        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(centre, Vector3.forward, range);
    }
#endif
}