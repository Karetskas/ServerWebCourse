namespace Academits.Karetskas.JsonCountries.JsonClasses
{
    public sealed class RegionalBloc
    {
        public string Acronym { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string[] OtherAcronyms { get; set; } = null!;

        public string[] OtherNames { get; set; } = null!;
    }
}
