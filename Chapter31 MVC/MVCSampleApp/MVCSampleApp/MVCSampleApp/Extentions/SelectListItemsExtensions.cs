using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCSampleApp.Extentions
{
    public static class SelectListItemsExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IDictionary<int, string> dict, int selectedId) =>
            dict.Select(item =>
              new SelectListItem
              {
                  Selected = item.Key == selectedId,
                  Text = item.Value,
                  Value = item.Key.ToString()
              });
    }
}
