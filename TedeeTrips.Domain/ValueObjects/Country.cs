using CSharpFunctionalExtensions;

namespace TedeeTrips.Domain.ValueObjects;

public class Country : EnumValueObject<Country, int>
{
    public static readonly Country Poland = new Country((int)CountryEnum.Poland, "Poland");
    
    public static readonly Country Quatar = new Country((int)CountryEnum.Quatar, "Quatar");
    
    public static readonly Country Germany = new Country((int)CountryEnum.Germany, "Germany");
    
    public static readonly Country France = new Country((int)CountryEnum.France, "France");
    
    public static readonly Country Spain = new Country((int)CountryEnum.Spain, "Spain");

    private enum CountryEnum
    {
        Poland = 1,
        Quatar = 2,
        Germany = 3,
        France = 4,
        Spain = 5,
    }

    private Country(int id, string name) : base(id, name)
    {
    }
}