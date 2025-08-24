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

    public static void DrawZone(Color color, Vector2 centre, float range)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(centre, range);
    }
}