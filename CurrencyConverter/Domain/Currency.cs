using SQLite;

namespace CurrencyConverter.Domain
{
    public class Currency
    {
        public Currency() 
        {
            Amount = 0.00;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public double Amount { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
