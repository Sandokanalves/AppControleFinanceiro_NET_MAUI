using ControleFinaceiro.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinaceiro.Service
{
    public class TransactionSevice
    {
      
            FirebaseClient firebase;

            public TransactionSevice()
            {
                firebase = new FirebaseClient("https://controlefinanceiro-v1-default-rtdb.firebaseio.com/");
            }
            public async Task Add(Transaction transaction)
            {


                var data = (await firebase
                    .Child("Transactions")
                    .OnceAsync<Transaction>()).Select(item => new Transaction
                    {
                        Id = item.Object.Id,
                        Name = item.Object.Name,
                        Type = item.Object.Type,
                        Date = item.Object.Date,
                        Value = item.Object.Value

                    }).ToList();
            var ultimoId = new Transaction();
                var Classid = 0;
            if (data.Count == 0)
            {
                Classid = 0;
            }
            else
            {
                ultimoId = data.Last();
                Classid = ultimoId.Id + 1;
            }
                await firebase.Child("Transactions")
                .PostAsync(
                new Transaction()
                {
                    Id = Classid,
                    Type = transaction.Type,
                    Name = transaction.Name,
                    Date = transaction.Date,
                    Value = transaction.Value

                });

            }

            public async Task<List<Transaction>> GetAll()
            {
                return (await firebase
                    .Child("Transactions")
                    .OnceAsync<Transaction>()).Select(item => new Transaction
                    {
                        Id = item.Object.Id,
                        Name = item.Object.Name,
                        Type = item.Object.Type,
                        Date = item.Object.Date,
                        Value = item.Object.Value

                    }).ToList();


            }

            public async Task Update(Transaction transaction)
            {
                try
                {
                    var toUpadate = (await firebase
                        .Child("Transactions")
                        .OnceAsync<Transaction>())
                        .Where(a => a.Object.Id == transaction.Id).FirstOrDefault();

                    await firebase
                   .Child("Transactions")
                   .Child(toUpadate.Key)
                   .PutAsync(new Transaction()
                   {

                       Type = transaction.Type,
                       Name = transaction.Name,
                       Date = transaction.Date,
                       Value = transaction.Value

                   });
                }
                catch (System.Exception)
                {

                    throw;
                }

            }
            public async Task Delete(Transaction transaction)
            {
                try
                {
                    var toDelete = (await firebase
                        .Child("Transactions")
                        .OnceAsync<Transaction>())
                        .Where(a => a.Object.Id == transaction.Id).FirstOrDefault();

                    await firebase
                   .Child("Transactions")
                   .Child(toDelete.Key)
                   .DeleteAsync();

                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }
    }

