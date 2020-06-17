using Markdig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Threading.Tasks;




namespace MVCSampleApp.TagHelperSamples
{
    //定义了用于指定Tag Helper的元素或特性名称，<markdown>
    [HtmlTargetElement("markdown", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement(Attributes = "markdownfile")]
    public class MarkdownTagHelper : TagHelper  //自定义TagHelper,派生于TagHelper
    {
        private readonly IWebHostEnvironment _env;
        public MarkdownTagHelper(IWebHostEnvironment env) => _env = env;

        //需要重写ProcessAsyn方法
        //TagHelperOutput 是<markdown>内部的内容
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            string markdown = string.Empty;
            if (MarkdownFile != null)
            {
                string filename = Path.Combine(_env.WebRootPath, MarkdownFile);
                markdown = File.ReadAllText(filename);
            }
            else
            {
                //通过GetChildContentAsync读取内容，然后用GetContent返回最终html内容
                markdown = (await output.GetChildContentAsync()).GetContent();
            }
            //然后将<markdown>的内容转化为MarkDown格式html
            output.Content.SetHtmlContent(Markdown.ToHtml(markdown));
        }

        [HtmlAttributeName("markdownfile")]
        public string MarkdownFile { get; set; }
    }
}
