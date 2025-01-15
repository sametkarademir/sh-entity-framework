namespace SoftwareHospital.EntityFramework.Extensions;

public static class EventOrderGeneratorExtensions
{
    private static long _lastOrder;

    public static long GetNext()
    {
        return Interlocked.Increment(ref _lastOrder);
    }
}