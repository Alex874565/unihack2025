using UnityEngine;

[System.Serializable]
public class Modifiers
{
    public int IncomeModifier;
    public int SpeedModifier;
    public int AirPollutionModifier;
    public int WaterPollutionModifier;
    public int SoilPollutionModifier;
    public bool ArePollutionModifiersPercentage;

    public static Modifiers operator +(Modifiers a, Modifiers b)
    {
        // Null safety — prevents crashes
        if (a == null) return b;
        if (b == null) return a;

        return new Modifiers
        {
            IncomeModifier = a.IncomeModifier + b.IncomeModifier,
            SpeedModifier = a.SpeedModifier + b.SpeedModifier,
            AirPollutionModifier = a.AirPollutionModifier + b.AirPollutionModifier,
            WaterPollutionModifier = a.WaterPollutionModifier + b.WaterPollutionModifier,
            SoilPollutionModifier = a.SoilPollutionModifier + b.SoilPollutionModifier,

            // Keep the bool from "a", or pick whichever logic you prefer
            ArePollutionModifiersPercentage = a.ArePollutionModifiersPercentage
        };
    }

    public static Modifiers operator -(Modifiers a, Modifiers b)
    {
        // Null safety
        if (a == null && b == null) return null;
        if (a == null) return new Modifiers
        {
            IncomeModifier = -b.IncomeModifier,
            SpeedModifier = -b.SpeedModifier,
            AirPollutionModifier = -b.AirPollutionModifier,
            WaterPollutionModifier = -b.WaterPollutionModifier,
            SoilPollutionModifier = -b.SoilPollutionModifier,
            ArePollutionModifiersPercentage = b.ArePollutionModifiersPercentage
        };
        if (b == null) return a;

        return new Modifiers
        {
            IncomeModifier = a.IncomeModifier - b.IncomeModifier,
            SpeedModifier = a.SpeedModifier - b.SpeedModifier,
            AirPollutionModifier = a.AirPollutionModifier - b.AirPollutionModifier,
            WaterPollutionModifier = a.WaterPollutionModifier - b.WaterPollutionModifier,
            SoilPollutionModifier = a.SoilPollutionModifier - b.SoilPollutionModifier,

            // Same bool rule as +
            ArePollutionModifiersPercentage = a.ArePollutionModifiersPercentage
        };
    }

    public override string ToString()
    {
        return
            $"Income: {IncomeModifier}, " +
            $"Speed: {SpeedModifier}, " +
            $"AirPollution: {AirPollutionModifier}, " +
            $"WaterPollution: {WaterPollutionModifier}, " +
            $"SoilPollution: {SoilPollutionModifier}, " +
            $"PollutionIsPercent: {ArePollutionModifiersPercentage}";
    }

}
