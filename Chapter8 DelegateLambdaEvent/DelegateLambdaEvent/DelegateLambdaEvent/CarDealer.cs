using System;
namespace DelegateLambdaEvent
{
    //作为NewCarInfo的泛型类型，EventHandler<CarInfoEventArgs>，所以要继承于EventArgs
    public class CarInfoEventArgs : EventArgs
    {
        public CarInfoEventArgs(string car) => Car = car;
        public string Car { get; }
    }

    public class CarDealer
    {
        //定义类型为EventHandler<CarInfoEventArgs>的事件
        //EventHandler<T>是一个接受两个参数(object sender, TEventArgs e)，返回类型为void的委托
        //TEventArgs必须为派生自EventArgs类的类型
        public event EventHandler<CarInfoEventArgs> NewCarInfo;

        public void NewCar(string car)
        {
            Console.WriteLine($"CarDealer, new car {car}");
            //触发事件
            RaiseNewCarInfo(car);
        }


        protected virtual void RaiseNewCarInfo(string car)
        {
            EventHandler<CarInfoEventArgs> newCarInfo = NewCarInfo;
            if (newCarInfo != null)
            {
                newCarInfo(this, new CarInfoEventArgs(car));
            }
        }
    }
}
