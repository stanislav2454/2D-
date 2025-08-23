using System;

public static class Utils
{
    private static readonly Random s_random = new Random();

    public static int GenerateRandomIndex(int max) =>
         s_random.Next(max);

    public static int GenerateRandomNumber(int min, int max) =>
         s_random.Next(min, max + 1);

    public static string UpdateUIText(string valueName, int currentValue) =>
         $"{valueName}: {currentValue}";
}