using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace DumpDiag.Console
{
    [HtmlTargetElement(Attributes = CommandAttributeName)]
    public class CommandTagHelper : TagHelper
    {
        public const string CommandAttributeName = "command-name";
        public const string ArgumentsAttributeName = "command-args";

        private readonly IUrlHelper _url;

        [HtmlAttributeName(CommandAttributeName)]
        public string Command { get; set; }

        [HtmlAttributeName(ArgumentsAttributeName)]
        public string Arguments { get; set; }

        public CommandTagHelper(IUrlHelperFactory urlFactory, IActionContextAccessor actionContextAccessor)
        {
            _url = urlFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = _url.Action(action: "Index", controller: "Commands", new
            {
                command = Command,
                arguments = Arguments
            });

            output.Attributes.SetAttribute("data-action", "command");
            if (context.TagName.Equals("a"))
            {
                output.Attributes.SetAttribute("href", url);
            }
            else
            {
                output.Attributes.SetAttribute("data-href", url);
            }
        }
    }
}
