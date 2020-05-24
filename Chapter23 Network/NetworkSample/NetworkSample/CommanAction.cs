using System;
using System.Linq;

namespace NetworkSample
{
    public class CommandActions
    {
        //返回反向发送的字符串
        public string Reverse(string action) => string.Join("", action.Reverse());

        //返回操作字符
        public string Echo(string action) => action;
    }
}
