using UnityEngine;

public static class NavigationMath
{
    // Calculates the horizontal angle (bearing) between two GPS points
    public static float CalculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        // Convert degrees to radians for mathematical functions
        float dLon = (lon2 - lon1) * Mathf.Deg2Rad;
        float lat1Rad = lat1 * Mathf.Deg2Rad;
        float lat2Rad = lat2 * Mathf.Deg2Rad;

        // Spherical trigonometry to find the direction
        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2Rad);
        float x = Mathf.Cos(lat1Rad) * Mathf.Sin(lat2Rad) - Mathf.Sin(lat1Rad) * Mathf.Cos(lat2Rad) * Mathf.Cos(dLon);

        // Convert radians back to degrees and normalize to 0-360 range
        return (Mathf.Atan2(y, x) * Mathf.Rad2Deg + 360) % 360;
    }

    // Calculates the physical distance in meters between two GPS coordinates
    public static float CalculateDistance(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6371000; // Earth's radius in meters
        float dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        float dLon = (lon2 - lon1) * Mathf.Deg2Rad;

        // Haversine formula to calculate distance on a sphere
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1 * Mathf.Deg2Rad) * Mathf.Cos(lat2 * Mathf.Deg2Rad) * Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        return R * (2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a)));
    }

    // Converts a numerical angle (0-360) into a human-readable compass direction
    public static string GetCardinalDirection(float angle)
    {
        // Array of direction names divided every 45 degrees
        string[] directions = { "œ≥‚Ì≥˜ North", "œÌ-—ı NE", "—ı≥‰ East", "œ‰-—ı SE", "œ≥‚‰ÂÌ¸ South", "œ‰-«ı SW", "«‡ı≥‰ West", "œÌ-«ı NW" };

        // Find the closest direction index based on the input angle
        int index = Mathf.RoundToInt(angle / 45f) % 8;
        return directions[index];
    }
}