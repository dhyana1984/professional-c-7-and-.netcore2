using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebSampleApp
{
    public class SubSection1
    {
        public string Setting4 { get; set; }
    }

    //为配置值得强类型访问创建一个类，将属性映射到配置文件中
    public class AppSettings
    {
        public string Setting2 { get; set; }
        public string Setting3 { get; set; }
        public SubSection1 SubSection1 { get; set; }
    }

    public class ConfigurationSample
    {
        private readonly IConfiguration _configuration;
        //通过依赖注入获得configuration，用来读取appsettings.json的内容
        public ConfigurationSample(IConfiguration configuration) =>
            _configuration = configuration;

        public async Task ShowApplicationSettingsAsync(HttpContext context)
        {
            //使用GetSection访问配置文件中的部分，然后传递Setting1索引器即可访问到"Setting1": "Value1"
            string settings = _configuration.GetSection("SampleSettings")["Setting1"];
            await context.Response.WriteAsync(settings.Div());
        }

        public async Task ShowApplicationSettingsUsingColonsAsync(HttpContext context)
        {
            //不用GetSection，可以使用冒号语法和索引器分隔所有层次结构
            string settings = _configuration["SampleSettings:Setting1"];
            await context.Response.WriteAsync(settings.Div());
        }

        
        public async Task ShowApplicationSettingsStronglyTyped(HttpContext context)
        {
            //通过自定义的强类型属性映射到配置文件中
            AppSettings settings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            await context.Response.WriteAsync($"setting 2: {settings.Setting2}, setting3: {settings.Setting3}, setting4: {settings.SubSection1.Setting4}".Div());
        }

        public async Task ShowConnectionStringSettingAsync(HttpContext context)
        {
            //ConnectionStrings的部分，直接使用GetConnectionString检索这部分设置
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            await context.Response.WriteAsync(connectionString.Div());
        }
    }
}
