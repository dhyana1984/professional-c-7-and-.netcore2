using System;
namespace ErrorAndException
{
    public static class CatchFilterSmple
    {
        public static void HandleAll()
        {
            var methods = new Action[]
            {
                //HandleAndThrowAgain
                //HandleAndThrowWithInnerException,
                //HandleAndRethrow,
                HandleWithFilter
            };

            foreach (var m in methods)
            {
                try
                {
                    m();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"\tInner Exception{ex.Message}");
                        Console.WriteLine(ex.InnerException.StackTrace);
                    }
                }
            }
        }

#line 8000
        public static void ThrowAnException(string message)
        {
            throw new MyCustomerException(message);
        }

#line 4000
        public static void HandleAndThrowAgain()
        {
            try
            {
                //在这里抛出异常
                ThrowAnException("test 1");
            }
            catch(Exception ex)
            {
                //这里Catch只能catch到HandleAndThrowAgain()方法，不能catch到ThrowAnException()方法
                //最初发生异常的地方丢失了
                Console.WriteLine($"Log exception {ex.Message} and thorw again");
                throw ex;
            }
        }

#line 3000
        //通过使用InnerException，可以抛出内部错误，实现传递了最初异常信息
        public static void HandleAndThrowWithInnerException()
        {
            try
            {
                ThrowAnException("Test 2");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log exception {ex.Message} and throw again");
                throw new AnotherCustomerException("throw with inner exception", ex);
            }
        }

#line 2000
        //使用throw，可以重新抛出相同的异常，只能抛出当前异常，不能传递异常对象
        public static void HandleAndRethrow()
        {
            try
            {
                ThrowAnException("Test 3");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log exception {ex.Message} and rethrow");
                throw;
            }
        }

#line 1000
        public static void HandleWithFilter()
        {
            try
            {
                ThrowAnException("Test 4");
            }
            //过滤器。这里的Filter(ex)方法总是返回false所以不会catch
            catch(Exception ex) when (Filter(ex))
            {
                Console.WriteLine("block never invoked");
            }
        }

        static bool Filter(Exception ex)
        {
            Console.WriteLine($"just log {ex.Message}");
            return false;
        }

        public static void SampleDisplay()
        {
            try
            {
                ThrowWithErrorCode(405);
            }
            //在catch上加filter去Catch一个更加特殊的exception
            catch (MyCustomerException ex) when (ex.ErrorCode == 405)
            {
                Console.WriteLine("Catch the 405 error");
            }
            catch (MyCustomerException ex)
            {
                Console.WriteLine("Catch non 405 error");
            }
        }

       public static void ThrowWithErrorCode(int code)
        {
            throw new MyCustomerException("Error in Foo") { ErrorCode = code };
        }
    }

    public class MyCustomerException : Exception
    {
        public MyCustomerException(string message):base(message)
        {
            
        }
        public int ErrorCode { get; set; }
    }

    public class AnotherCustomerException : Exception
    {
        //innerException是个内部异常
        public AnotherCustomerException(string message, Exception innerException):base(message, innerException)
        {

        }
    }
}
