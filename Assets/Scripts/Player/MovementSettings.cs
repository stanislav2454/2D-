using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Settings/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 6f;
    [field: SerializeField, Min(0)] public float CrawlSpeed { get; private set; } = 3f;
}