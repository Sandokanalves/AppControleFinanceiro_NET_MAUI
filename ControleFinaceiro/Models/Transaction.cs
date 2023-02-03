


namespace ControleFinaceiro.Models
{
    public class Transaction
    {
      
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public String Name { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Value { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}
