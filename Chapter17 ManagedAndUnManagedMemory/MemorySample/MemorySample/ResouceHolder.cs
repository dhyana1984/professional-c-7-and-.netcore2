using System;
namespace MemorySample
{
    public class ResouceHolder : IDisposable
    {
        private bool _isDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            //GC不再调用本类的析构方法
            GC.SuppressFinalize(this);
        }

        //这是真正完成清理工作的方法
        //本方法由析构函数和IDisposable.Dispose()方法调用
        //disposing参数表示本方法是由析构函数调用还是IDispose.Dispose()方法调用
        //本方法不应该从其他地方调用
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                //如果使用者调用IDisposable.Dispose()方法，应该清理托管对象和非托管对象
                if (disposing)
                {
                    //清理托管对象
                    //Dispose()方法
                }
                //清理非托管对象
            }
            _isDisposed = true;
        }

        ~ResouceHolder()
        {
            //在析构函数中，是由GC调用，所以不应该在调用析构函数时访问托管对象,因为无法获确定其状态
            //此时清理非托管对象即可, GC会清理非托管对象
            Dispose(false);
        }

        public void SomeMethod()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ResourceHolder");
            }
            //method implementation
        }
    }
}
