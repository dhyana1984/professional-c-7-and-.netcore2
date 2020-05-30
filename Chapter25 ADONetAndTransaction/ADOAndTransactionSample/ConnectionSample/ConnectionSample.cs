using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ADOAndTransaction
{
    public static class ConnectionSample
    {
        public static void RunSample()
        {
            //OpenConnection();
            //ConnectionUsingConfig();
            ConnectionInformation();
        }
        const string connectionString = @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books";
        //打开数据库连接
        static void OpenConnection()
        {
           
            var connection = new SqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connection opened!");
            connection.Close();
        }

        //使用
        static void ConnectionUsingConfig()
        {
            var path = @"/Users/yxiong/Repository/professional-c-7-and-.netcore2/Chapter25 ADONetAndTransaction/ADOAndTransaction/ADOAndTransaction";
            //读取json配置文件
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("config.json");
            //返回通过配置文件生成的配置IConfiguration对象
            IConfiguration config = configurationBuilder.Build();
            //检索配置对象
            string connectionString = config["Data:DefaultConnection:ConnectionString"];
            Console.WriteLine(connectionString);
        }

        static void ConnectionInformation()
        {
            using ( var connection = new SqlConnection(connectionString))
            {
                //SQL Server返回一个信息或警告消息时，就触发InfoMessage事件
                connection.InfoMessage += (sender, e) =>
                {
                    Console.WriteLine($"warning or info {e.Message}");
                };
                //连接的状态变化时，就触发StateChange事件
                connection.StateChange += (sender, e) =>
                {
                    Console.WriteLine($"current state: {e.CurrentState}, before: {e.OriginalState}");
                };

                try
                {
                    //打开StatisticsEnabled，可以在RetriverStatistics方法中检索统计信息
                    connection.StatisticsEnabled = true;
                    //如果出现异常，默认不触发InfoMessage事件，但是设置FireInfoMessageEventOnUserErrors为True可以开启触发InfoMessage事件
                    connection.FireInfoMessageEventOnUserErrors = true;
                    connection.Open();
                    Console.WriteLine("connection opened");
                    //读取数据
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}
