using UnityEngine;

/// <summary>
/// Rotates a directional light over time to simulate a full day cycle.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Real seconds it takes for one full day/night cycle.")]
    [Min(0.1f)]
    public float dayLengthSeconds = 120.0f;

    private void Update()
    {
        float degreesPerSecond = 360.0f / dayLengthSeconds;
        transform.Rotate(Vector3.right, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
