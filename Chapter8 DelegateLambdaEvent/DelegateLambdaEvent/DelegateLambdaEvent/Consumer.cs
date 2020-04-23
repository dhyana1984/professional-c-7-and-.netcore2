using System;
namespace DelegateLambdaEvent
{
    public class Consumer
    {
        private string _name;
        public Consumer(string name) => _name = name;
        //与CarDealer的EventHandler<CarInfoEventArgs>签名一样，所以可以绑到NewCarInfo上
        public void NewCarIsHere(object sender, CarInfoEventArgs e)
        {
            Console.WriteLine($"{_name}: car {e.Car} is new");
        }
    }
}
