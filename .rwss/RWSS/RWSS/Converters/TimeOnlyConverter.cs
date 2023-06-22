using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RWSS.Converters
{
    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter() : base(
            d => d.ToTimeSpan(),
            d => TimeOnly.FromTimeSpan(d))
        { }
    }
}
