public enum LiquidType
{
    Water,
    Acid,
    Hcl // Can be expanded later
}

[System.Serializable]
public class Liquid
{
    public LiquidType type;
    public float amount; // In milliliters

    public Liquid(LiquidType type, float amount)
    {
        this.type = type;
        this.amount = amount;
    }
}

