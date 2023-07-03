namespace RaccoonNinjaToolbox.Scripts.Interfaces
{
    public interface IMinMaxRangeAttribute<out T> where T: struct
    {
        T Min { get; }
        T Max { get; }
    }
}