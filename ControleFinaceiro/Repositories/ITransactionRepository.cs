
using ControleFinaceiro.Models;

namespace ControleFinaceiro.Repositories
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction);
        Task Delete(Transaction transaction);
        Task<List<Transaction>> GetAll();
        Task Update(Transaction transaction);
    }
}