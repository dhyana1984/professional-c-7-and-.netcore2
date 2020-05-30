using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AsyncSamples
{
    class Program
    {
        const string connectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books";
        static async Task Main(string[] args)
        {
            await ReadAsync("Professional");
            Console.WriteLine("Another things...");
            Console.ReadLine();
        }

        static async Task ReadAsync(string title)
        {
            string GetBookQuery() =>
               "SELECT [Id], [Title], [Publisher], [ReleaseDate] " +
               "FROM [ProCSharp].[Books] WHERE lower([Title]) LIKE @Title ORDER BY [ReleaseDate] DESC";

            var connection = new SqlConnection(connectionString);

            var command = new SqlCommand(GetBookQuery(), connection);
            var parameter = new SqlParameter("Title", SqlDbType.NVarChar, 50)
            {
                Value = $"{title}%"

            };
            command.Parameters.Add(parameter);

            await connection.OpenAsync();

            //ExecuteReader方法返回一个DataReader对象，返回的对象用于遍历返回的记录
            //SqlDataReader使用后要销毁
            //给ExecuteReader传递CommandBehavior.CloseConnection，会在关闭读取器的时候自动关闭连接
            using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (await reader.ReadAsync())
                {
                    //GetInt32(0) 0索引对应Sql Select语句检索的列
                    int id = reader.GetInt32(0);
                    string bookTitle = reader.GetString(1);
                    //也可以直接在reader上使用索引
                    string publisher = reader[2].ToString();
                    //使用IsDBNull判断是否为空
                    DateTime? releaseDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
                    Console.WriteLine($"{id,5}. {bookTitle,-40} {publisher,-15} {releaseDate:d}");
                }
            }
        }
    }
}
