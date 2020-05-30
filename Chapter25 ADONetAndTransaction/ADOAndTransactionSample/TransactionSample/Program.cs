using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TransactionSample
{
    class Program
    {
        const string connectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books";
        static async Task Main(string[] args)
        {
            await TransactionSample();
        }

        static async Task TransactionSample()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //使用SqlTransaction.BeginTransaction打开事务
                SqlTransaction tx = connection.BeginTransaction();
                try
                {
                    string sql = "INSERT INTO [ProCSharp].[Books] ([Title], [Publisher], [Isbn], [ReleaseDate]) " +
                        "VALUES (@Title, @Publisher, @Isbn, @ReleaseDate);" +
                        "SELECT SCOPE_IDENTITY()";

                    var command = new SqlCommand
                    {
                        CommandText = sql,
                        Connection = connection,
                        //设置事务，不能把事务分配给不同的connection的command
                        Transaction = tx
                    };

                    var p1 = new SqlParameter("Title", SqlDbType.NVarChar, 50);
                    var p2 = new SqlParameter("Publisher", SqlDbType.NVarChar, 50);
                    var p3 = new SqlParameter("Isbn", SqlDbType.NVarChar, 20);
                    var p4 = new SqlParameter("ReleaseDate", SqlDbType.Date);
                    command.Parameters.AddRange(new SqlParameter[] { p1, p2, p3, p4 });

                    command.Parameters["Title"].Value = "Professional C# 8 and .NET Core 3.0";
                    command.Parameters["Publisher"].Value = "Wrox Press";
                    command.Parameters["Isbn"].Value = "42-08154711";
                    command.Parameters["ReleaseDate"].Value = new DateTime(2020, 9, 2);

                    object id = await command.ExecuteScalarAsync();
                    Console.WriteLine($"record added with id: {id}");

                    string sql1 = "insert into dbo.AddRecord (BookId) values (@BookId)";
                    var p5 = new SqlParameter("BookId", SqlDbType.Int);
                    command.Parameters.Add(p5);
                    command.Parameters["BookId"].Value = Convert.ToInt16(id);
                    command.CommandText = sql1;
                    await command.ExecuteNonQueryAsync();

                    tx.Commit();

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error {ex.Message}, rolling back");
                    tx.Rollback();
                }
            }
        }
    }
}
