namespace Academits.Karetskas.JsonCountries
{
    public sealed class Currency
    {
        private string? _code;
        private string? _name;
        private string? _symbol;

        public string? Code
        {
            get => CheckString(_code);

            set => _code = value;
        }

        public string? Name
        {
            get => CheckString(_name);

            set => _name = value;
        }

        public string? Symbol
        {
            get => CheckString(_symbol);

            set => _symbol = value;
        }

        private static string CheckString(string? text)
        {
            return text ?? "null";
        }

        public override string ToString()
        {
            return $"Code: {Code}; Name: {Name}; Symbol: {Symbol}.";
        }
    }
}
