using System;
using System.Threading.Tasks;
using System.Transactions;
using static SystemTransactionSample.Utilities;

namespace SystemTransactionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //await CommittableTransactionAsync();
            //DependentTransactions();
            AmbientTransactions();
        }

        static async Task CommittableTransactionAsync()
        {
            //Transaction类不能以编程方式提交，没有提交事务的方法，只支持终止事务
            //唯一支持事务的类是CommittableTransaction
            var tx = new CommittableTransaction();
            DisplayTransactionInformation("TX created", tx.TransactionInformation);

            try
            {
                var b = new Book
                {
                    Title = "A Dog in The House",
                    Publisher = "Pet Show",
                    Isbn = RandomIsbn(),
                    ReleaseDate = new DateTime(2018, 11, 24)
                };
                var data = new BookData();
                await data.AddBookAsync(b, tx);

                if (AbortTx())
                {
                    throw new ApplicationException("transaction abort by the user");
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                tx.Rollback();
            }

            DisplayTransactionInformation("TX completed", tx.TransactionInformation);
        }

        //依赖事务
        static void DependentTransactions()
        {
            async Task UsingDependentTransactionAsync(DependentTransaction dtx)
            {
                dtx.TransactionCompleted += (sender, e) =>
                    DisplayTransactionInformation("Depdendent TX completed", e.Transaction.TransactionInformation);

                DisplayTransactionInformation("Dependent Tx", dtx.TransactionInformation);

                await Task.Delay(2000);

                //根事务也可以标记成Complete
                dtx.Complete();
                DisplayTransactionInformation("Dependent Tx send complete", dtx.TransactionInformation);
            }

            //创建根事务
            var tx = new CommittableTransaction();
            DisplayTransactionInformation("Root Tx created", tx.TransactionInformation);

            try
            {
                //使用CommittableTransaction根事务的DependentClone创建一个依赖事务，需要传入Option参数
                //BlockCommitUntilComplete 方法Commit会等待，直到所有依赖事务都定义了结果未知。即等待依赖事务
                //RollbackIfNotComplete 如果依赖事务没有在根事务的commit方法之前调用Complete方法，则事务将终止，即不等待依赖事务
                DependentTransaction depTx = tx.DependentClone(DependentCloneOption.BlockCommitUntilComplete);
                //传递根事务给一个单独的任务
                Task t1 = Task.Run(() => UsingDependentTransactionAsync(depTx));

                if (AbortTx())
                {
                    throw new ApplicationException("transaction abort by the user");
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx.Rollback();
            }

            DisplayTransactionInformation("TX completed", tx.TransactionInformation);
        }

        //环境事务
        static void AmbientTransactions()
        {
            //使用TransactionScope使用环境事务，不需要手动获取与事务的连接，由支持环境事务的资源自动完成的
            using (var scope = new TransactionScope())
            {
                Transaction.Current.TransactionCompleted += (sender, e) =>
                    DisplayTransactionInformation("TX completed", e.Transaction.TransactionInformation);

                DisplayTransactionInformation("Ambient TX created", Transaction.Current.TransactionInformation);

                var b = new Book
                {
                    Title = "Cats in The House",
                    Publisher = "Pet Show",
                    Isbn = RandomIsbn(),
                    ReleaseDate = new DateTime(2019, 11, 24)
                };
                var data = new BookData();
                //这里的AddBook方法没有传递事务
                data.AddBook(b);

                if (!AbortTx())
                {
                    //提交
                    scope.Complete();
                }
                else
                {
                    Console.WriteLine("transaction abort by the user");
                }

            }  // scope.Dispose();
        }

        static void NestedScopes()
        {
            using (var scope = new TransactionScope())
            {
                Transaction.Current.TransactionCompleted += (sender, e) =>
                    DisplayTransactionInformation("TX completed", e.Transaction.TransactionInformation);

                DisplayTransactionInformation("Ambient TX created", Transaction.Current.TransactionInformation);

                var b = new Book
                {
                    Title = "Dogs in The House",
                    Publisher = "Pet Show",
                    Isbn = RandomIsbn(),
                    ReleaseDate = new DateTime(2020, 11, 24)
                };
                var data = new BookData();
                data.AddBook(b);

                //内部作用域TransactionScopeOption.RequiresNew 表示建立一个新作用域，和外部独立
                //TransactionScopeOption.Required表示作用域需要一个事务，如果外部作用域有一个环境事务，内部就用外部的事务，如果没有就新建一个
                //TransactionScopeOption.Required当所有作用域设置成功时，事务才能提交，只要有一个作用域在根作用域被释放之前没有调用Complete方法，事务就会终止
                //TransactionScopeOption.Suppress作用域不包含环境事务
                using (var scope2 = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Transaction.Current.TransactionCompleted += (sender, e) =>
                        DisplayTransactionInformation("Inner TX completed", e.Transaction.TransactionInformation);

                    DisplayTransactionInformation("Inner TX scope", Transaction.Current.TransactionInformation);

                    var b1 = new Book
                    {
                        Title = "Dogs and Cats in The House",
                        Publisher = "Pet Show",
                        Isbn = RandomIsbn(),
                        ReleaseDate = new DateTime(2021, 11, 24)
                    };
                    var data1 = new BookData();
                    data1.AddBook(b1);

                    scope2.Complete();
                }

                scope.Complete();
            }
        }
    }


}
