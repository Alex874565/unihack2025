using UnityEngine;

[System.Serializable]
public class Modifiers
{
    public float IncomeModifier;
    public float SpeedModifier;
    public float AirPollutionModifier;
    public float WaterPollutionModifier;
    public float SoilPollutionModifier;
    public ModifierTypes PollutionModifierType;

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
            PollutionModifierType = a.PollutionModifierType
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
            PollutionModifierType = b.PollutionModifierType
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
            PollutionModifierType = a.PollutionModifierType
        };
    }

    public static Modifiers operator *(Modifiers a, Modifiers b)
    {
        if (a == null || b == null) return null;
        return new Modifiers
        {
            IncomeModifier = a.IncomeModifier * b.IncomeModifier / 100f,
            SpeedModifier = a.SpeedModifier * b.SpeedModifier / 100f,
            AirPollutionModifier = a.AirPollutionModifier * b.AirPollutionModifier / 100f,
            WaterPollutionModifier = a.WaterPollutionModifier * b.WaterPollutionModifier / 100f,
            SoilPollutionModifier = a.SoilPollutionModifier * b.SoilPollutionModifier / 100f,
            PollutionModifierType = a.PollutionModifierType
        };
    }

    public static Modifiers operator *(Modifiers a, float scalar)
    {
        if (a == null) return null;
        return new Modifiers
        {
            IncomeModifier = a.IncomeModifier * scalar,
            SpeedModifier = a.SpeedModifier * scalar,
            AirPollutionModifier = a.AirPollutionModifier * scalar,
            WaterPollutionModifier = a.WaterPollutionModifier * scalar,
            SoilPollutionModifier = a.SoilPollutionModifier * scalar,
            PollutionModifierType = a.PollutionModifierType
        };
    }

    public static Modifiers operator /(Modifiers a, float scalar)
    {
        if (a == null) return null;
        return new Modifiers
        {
            IncomeModifier = a.IncomeModifier / scalar,
            SpeedModifier = a.SpeedModifier / scalar,
            AirPollutionModifier = a.AirPollutionModifier / scalar,
            WaterPollutionModifier = a.WaterPollutionModifier / scalar,
            SoilPollutionModifier = a.SoilPollutionModifier / scalar,
            PollutionModifierType = a.PollutionModifierType
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
            $"PollutionIsPercent: {PollutionModifierType}";
    }

    public Modifiers Abs()
    {
        return new Modifiers
        {
            IncomeModifier = Mathf.Abs(IncomeModifier),
            SpeedModifier = Mathf.Abs(SpeedModifier),
            AirPollutionModifier = Mathf.Abs(AirPollutionModifier),
            WaterPollutionModifier = Mathf.Abs(WaterPollutionModifier),
            SoilPollutionModifier = Mathf.Abs(SoilPollutionModifier),

            PollutionModifierType = PollutionModifierType
        };
    }
}
