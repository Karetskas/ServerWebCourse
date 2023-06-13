namespace Academits.Karetskas.JsonCountries.JsonClasses
{
    public sealed class Country
    {
        public string Name { get; set; } = null!;

        public string[] TopLevelDomain { get; set; } = null!;

        public string Alpha2Code { get; set; } = null!;

        public string Alpha3Code { get; set; } = null!;

        public string[] CallingCodes { get; set; } = null!;

        public string Capital { get; set; } = null!;

        public string[] AltSpellings { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string Subregion { get; set; } = null!;

        public int Population { get; set; }

        public double[] Latlng { get; set; } = null!;

        public string Demonym { get; set; } = null!;

        public double? Area { get; set; }

        public double? Gini { get; set; }

        public string[] Timezones { get; set; } = null!;

        public string[] Borders { get; set; } = null!;

        public string NativeName { get; set; } = null!;

        public string NumericCode { get; set; } = null!;

        public Currency[] Currencies { get; set; } = null!;

        public Language[] Languages { get; set; } = null!;

        public Translation Translations { get; set; } = null!;

        public string Flag { get; set; } = null!;

        public RegionalBloc[] RegionalBlocs { get; set; } = null!;

        public string? Cioc { get; set; }
    }
}
