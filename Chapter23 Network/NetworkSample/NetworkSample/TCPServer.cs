using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NetworkSample.CustomProtocol;

namespace NetworkSample
{

    public static class TCPServer
    {

        private enum ParseResponse
        {
            OK,
            CLOSE,
            ERROR,
            TIMEOUT
        }

        private static readonly SessionManager _sessionManager = new SessionManager();
        private const int PortNumber = 8800;
        private static readonly CommandActions _commandActions = new CommandActions();

        public static void Run()
        {
            //每分钟删除最近没有使用的回话
            using (var timer = new Timer(TimerSessionCleanup, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)))
            {
                RunServerAsync().Wait();
            }
        }

        static async Task RunServerAsync()
        {
            try
            {
                //构造TCP监听器，在IP地址和端口号上监听
                var listener = new TcpListener(IPAddress.Any, PortNumber);
                Console.WriteLine($"listener started at port {PortNumber}");
                listener.Start();
                while (true)
                {
                    Console.WriteLine("waiting for client...");
                    //等待客户端的连接
                    //如果客户端连接，返回TcpClient实例，允许与客户沟通
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Task t = RunClientRequestAsync(client);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception of type {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        private static Task RunClientRequestAsync(TcpClient client)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (client)
                    {
                        Console.WriteLine("client connected");
                        //获取client的NetworkStream
                        using (NetworkStream stream = client.GetStream())
                        {
                            bool completed = false;

                            do
                            {
                                byte[] readBuffer = new byte[1024];

                                //stream.ReadAsync读取来自客户机的请求到readBuffer
                                int read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
                                //获得readBuffer中的信息转成字符串并且写入Console
                                string request = Encoding.ASCII.GetString(readBuffer, 0, read);
                                Console.WriteLine($"received {request}");

                                byte[] writeBuffer = null;
                                string response = string.Empty;

                                //根据ParseRequest的结果创建客户端的回应
                                ParseResponse resp = ParseRequest(request, out string sessionId, out string result);
                                switch (resp)
                                {
                                    case ParseResponse.OK:
                                        string content = $"{STATUSOK}::{SESSIONID}::{sessionId}";
                                        if (!string.IsNullOrEmpty(result)) content += $"{SEPARATOR}{result}";
                                        response = $"{STATUSOK}{SEPARATOR}{SESSIONID}{SEPARATOR}{sessionId}{SEPARATOR}{content}";
                                        break;
                                    case ParseResponse.CLOSE:
                                        response = $"{STATUSCLOSED}";
                                        completed = true;
                                        break;
                                    case ParseResponse.TIMEOUT:
                                        response = $"{STATUSTIMEOUT}";
                                        break;
                                    case ParseResponse.ERROR:
                                        response = $"{STATUSINVALID}";
                                        break;
                                    default:
                                        break;
                                }
                                writeBuffer = Encoding.ASCII.GetBytes(response);
                                //通过stream.WriteAsync写入NetworkStream返回给客户端
                                await stream.WriteAsync(writeBuffer, 0, writeBuffer.Length);
                                await stream.FlushAsync();
                                //打印出返回的内容
                                Console.WriteLine($"returned {Encoding.ASCII.GetString(writeBuffer, 0, writeBuffer.Length)}");
                            } while (!completed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in client request handling of type {ex.GetType().Name}, Message: {ex.Message}");
                }
                Console.WriteLine("client disconnected");
            });
        }

        //解析请求，过滤掉会话标识符
        private static ParseResponse ParseRequest(string request, out string sessionId, out string response)
        {
            sessionId = string.Empty;
            response = string.Empty;
            string[] requestColl = request.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            //如果请求是HELO，则创建一个Session
            if (requestColl[0] == COMMANDHELO)  // first request
            {
                sessionId = _sessionManager.CreateSession();

            }
            //后来的请求中requestColl[0]必须是SESSIONID
            else if (requestColl[0] == SESSIONID)  // any other valid request
            {
                sessionId = requestColl[1];
                //检查Session是否有效
                ////如果Session无效返回超时
                if (!_sessionManager.TouchSession(sessionId))
                {
                    return ParseResponse.TIMEOUT;
                }

                if (requestColl[2] == COMMANDBYE)
                {
                    return ParseResponse.CLOSE;
                }
                if (requestColl.Length >= 4)
                {
                    response = ProcessRequest(requestColl);
                }
            }
            else
            {
                return ParseResponse.ERROR;
            }
            return ParseResponse.OK;
        }

        //处理不同请求
        private static string ProcessRequest(string[] requestColl)
        {
            if (requestColl.Length < 4) throw new ArgumentException("invalid length requestColl");

            string sessionId = requestColl[1];
            string response = string.Empty;
            string requestCommand = requestColl[2];
            string requestAction = requestColl[3];


            switch (requestCommand)
            {
                case COMMANDECHO:
                    response = _commandActions.Echo(requestAction);
                    break;
                case COMMANDREV:
                    response = _commandActions.Reverse(requestAction);
                    break;
                case COMMANDSET:
                    response = _sessionManager.ParseSessionData(sessionId, requestAction);
                    break;
                case COMMANDGET:
                    response = $"{_sessionManager.GetSessionData(sessionId, requestAction)}";
                    break;
                default:
                    response = STATUSUNKNOWN;
                    break;
            }
            return response;
        }

        private static void TimerSessionCleanup(object o) =>
            _sessionManager.CleanupAllSessions();
    }
}



   





