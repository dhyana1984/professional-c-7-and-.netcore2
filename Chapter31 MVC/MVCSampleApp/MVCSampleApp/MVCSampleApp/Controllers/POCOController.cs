
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace MVCSampleApp.Controllers
{
    //POCOController，没有集成Controller类
    public class POCOController
    {
        public string Index() =>
          "this is a POCO controller";

        //需要应用ActionContext Attribute
        [ActionContext]
        public ActionContext ActionContext { get; set; }

        public HttpContext HttpContext => ActionContext.HttpContext;

        public ModelStateDictionary ModelState => ActionContext.ModelState;

        public string UserAgentInfo()
        {
            //通过ActionContext的HttpContext来访问Request和Response
            if (HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                return HttpContext.Request.Headers["User-Agent"];
            }
            return "No user-agent information";
        }
    }
}
