using UnityEngine;

public static class NavigationMath
{
    public static float CalculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float dLon = (lon2 - lon1) * Mathf.Deg2Rad;
        float lat1Rad = lat1 * Mathf.Deg2Rad;
        float lat2Rad = lat2 * Mathf.Deg2Rad;
        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLon);
        return (Mathf.Atan2(y, x) * Mathf.Rad2Deg + 360) % 360;
    }

    public static float CalculateDistance(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6371000;
        float dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        float dLon = (lon2 - lon1) * Mathf.Deg2Rad;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) + Mathf.Cos(lat1 * Mathf.Deg2Rad) * Mathf.Cos(lat2 * Mathf.Deg2Rad) * Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        return R * (2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a)));
    }

    public static string GetCardinalDirection(float angle)
    {
        string[] directions = { "œ≥‚Ì≥˜ North", "œÌ-—ı NE", "—ı≥‰ East", "œ‰-—ı SE", "œ≥‚‰ÂÌ¸ South", "œ‰-«ı SW", "«‡ı≥‰ West", "œÌ-«ı NW" };
        int index = Mathf.RoundToInt(angle / 45f) % 8;
        return directions[index];
    }
}