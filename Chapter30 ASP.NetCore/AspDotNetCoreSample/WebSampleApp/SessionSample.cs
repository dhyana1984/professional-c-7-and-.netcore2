using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSampleApp
{
    public class SessionSample
    {
        private const string SessionVisites = nameof(SessionVisites);
        private const string SessionTimeCreated = nameof(SessionTimeCreated);
        public static async Task SessionAsync(HttpContext context)
        {
            //GetInt32和GetString可以获取会话状态
            int visits = context.Session.GetInt32(SessionVisites) ?? 0;
            string timeCreated = context.Session.GetString(SessionTimeCreated) ?? string.Empty;
            if (string.IsNullOrEmpty(timeCreated))
            {
                timeCreated = DateTime.Now.ToString("t", CultureInfo.InvariantCulture);
                //SetString和SetInt32可以编写回话状态
                context.Session.SetString(SessionTimeCreated, timeCreated);
            }
            DateTime timeCreated2 = DateTime.Parse(timeCreated);
            context.Session.SetInt32(SessionVisites, ++visits);
            await context.Response.WriteAsync(
                $"Number of visits within this session: {visits} " +
                $"that was created at {timeCreated2:T}; " +
                $"current time: {DateTime.Now:T}");
        }
    }
}
