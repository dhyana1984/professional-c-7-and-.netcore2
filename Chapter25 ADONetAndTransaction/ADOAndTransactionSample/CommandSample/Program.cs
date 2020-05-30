using System;
using System.Data;
using System.Data.SqlClient;

namespace CommandSample
{
    class Program
    {
        const string connectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books";
        static void Main(string[] args)
        {
            //ExecuteNonQuery();
            //ExecuteScalar();
            ExecuteReader("");
        }

        static void CreateCommand()
        {
            using ( var connection = new SqlConnection(connectionString))
            {
                //string sql = "Select title, publisher, releasedate from ProCSharp.Boos";
                //var command = new SqlCommand(sql, connection);

                //使用SqlConnection的CreateCommand方法，赋予ConmmandText也可以创建command
                //SqlCommand command1 = connection.CreateCommand();
                //command.CommandText = sql;

                //如果查询中需要传入参数，不要使用拼接字符串，使用ADO.Net的参数特性
                string sql1 = "select * from ProCSharp.Boos where lower(title) like @title";
                var command2 = new SqlCommand(sql1, connection);
                //Command的Parameters属性返回SqlParametersCollection和AddWithValue方法，这是简单的方式
                //command2.Parameters.AddWithValue("titke", "professional");

                //更有效的添加参数的方式
                command2.Parameters.Add("title", SqlDbType.NVarChar, 50);
                command2.Parameters["title"].Value = "professional";
                connection.Open();
            }
        }

        static void ExecuteNonQuery()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO [ProCSharp].[Books] ([Title], [Publisher], [Isbn], [ReleaseDate]) " +
                        "VALUES (@Title, @Publisher, @Isbn, @ReleaseDate)";

                    var command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("Title", "TestTitle");
                    command.Parameters.AddWithValue("Publisher", "TestPubklisher");
                    command.Parameters.AddWithValue("Isbn", "TestISBN");
                    command.Parameters.AddWithValue("ReleaseDate", DateTime.Now);

                    connection.Open();
                    //command.ExecuteNonQuery()一般用于update, instert, delete语句，返回值是受影响记录条数
                    //如果调用带输出参数存储过程，该方法就有返回值
                    int records = command.ExecuteNonQuery();
                    Console.WriteLine($"{records} record(s) inserted");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ExecuteScalar()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM [ProCSharp].[Books]";
                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;
                connection.Open();
                //command.ExecuteScalar从sql语句返回一个结果，例如表中的记录个数，或者服务器上当前日期等
                //该方法返回一个对象，根据需要可以转化为合适的类型，如果sql只返回一列，最好用command.ExecuteScalar来检索这一列
                //也适用于只返回一个值的存储过程
                object count = command.ExecuteScalar();
                Console.WriteLine($"counted {count} book records");
            }
        }

        static void ExecuteReader(string title)
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

            connection.Open();

            //ExecuteReader方法返回一个DataReader对象，返回的对象用于遍历返回的记录
            //SqlDataReader使用后要销毁
            //给ExecuteReader传递CommandBehavior.CloseConnection，会在关闭读取器的时候自动关闭连接
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
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
