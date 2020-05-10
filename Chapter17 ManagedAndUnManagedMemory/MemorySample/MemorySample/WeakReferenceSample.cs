namespace MemorySample
{
    public static class WeakReferenceSample
    {
        //var myCache = new MyCache();
        //myCache.Add(myClassVariable);

        //...

        //使用完myClassVariable以后可以设置为null
        //但是此时myClassVariable不会被回收，因为该对象仍然在缓存对象中引用，此时就要用弱引用
        //myClassVariable= null
        public static void WeakReferenceSampleDisplay()
        {
            var myWeakReference = new WeakReference(new DataObject());
            //检查弱引用是否被回收
            if (myWeakReference.IsAlive)
            {
                //通过Target属性声明强引用
                DataObject strongReference = myWeakReference.Target as DataObject;
                if (strongReference != null)
                {
                    //use the strong reference
                }
                {
                    //strong reference not available
                }
            }
        }
    }

    public class DataObject
    {

    }
}