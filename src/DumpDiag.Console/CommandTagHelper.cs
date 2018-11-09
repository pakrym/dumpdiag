using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace DumpDiag.Console
{
    [HtmlTargetElement(Attributes = CommandAttributeName)]
    public class CommandTagHelper : TagHelper
    {
        public const string CommandAttributeName = "command-name";
        public const string RouteValuesPrefix = "command-route-";
        public const string RouteValuesDictionaryName = "command-all-route-data";

        private readonly IUrlHelper _url;
        private IDictionary<string, string> _routeValues;

        [HtmlAttributeName(CommandAttributeName)]
        public string Command { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        public CommandTagHelper(IUrlHelperFactory urlFactory, IActionContextAccessor actionContextAccessor)
        {
            _url = urlFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeValues = _routeValues == null ? null : new RouteValueDictionary(_routeValues);
            var url = _url.Action(action: Command, controller: "Commands", routeValues);

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
