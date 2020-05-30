using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;


namespace SystemTransactionSample
{
    public class BookData
    {
        const string connectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books";

        public async Task AddBookAsync(Book book, Transaction tx)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO [ProCSharp].[Books] ([Title], [Publisher], [Isbn], [ReleaseDate]) " +
                    "VALUES (@Title, @Publisher, @Isbn, @ReleaseDate)";

                await connection.OpenAsync();
                if (tx != null)
                {
                    //使用connection.EnlistTransaction来调用System.Transaction
                    //connection参与该事务的结果, 此处tx是CommittableTransaction
                    connection.EnlistTransaction(tx);
                }
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("Title", book.Title);
                command.Parameters.AddWithValue("Publisher", book.Publisher);
                command.Parameters.AddWithValue("Isbn", book.Isbn);
                command.Parameters.AddWithValue("ReleaseDate", book.ReleaseDate);

                await command.ExecuteNonQueryAsync();
            }
        }

        public void AddBook(Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO [ProCSharp].[Books] ([Title], [Publisher], [Isbn], [ReleaseDate]) " +
                    "VALUES (@Title, @Publisher, @Isbn, @ReleaseDate)";

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("Title", book.Title);
                command.Parameters.AddWithValue("Publisher", book.Publisher);
                command.Parameters.AddWithValue("Isbn", book.Isbn);
                command.Parameters.AddWithValue("ReleaseDate", book.ReleaseDate);

                command.ExecuteNonQuery();
            }
        }
    }
}
