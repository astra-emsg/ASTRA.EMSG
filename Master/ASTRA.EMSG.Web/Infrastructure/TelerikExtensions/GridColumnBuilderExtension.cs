using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace ASTRA.EMSG.Web.Infrastructure.TelerikExtensions
{
    public static class GridColumnBuilderExtension
    {
        public static GridDestroyActionCommandBuilder EmsgDelete<T>(this GridActionCommandFactory<T> factory) where T : class
        {
            return factory.Destroy().HtmlAttributes(new { title = Resources.GridLocalization.Delete });
        }

        public static GridBoundColumnBuilder<T> AsEmsgCommand<T>(this GridBoundColumnBuilder<T> builder) where T : class
        {
            return builder.Title("").Sortable(false).Width(10).Filterable(false);
        }

        public static GridBoundColumnBuilder<T> GridCommands<T>(this GridBoundColumnBuilder<T> builder, string firstValue, params string[] values) where T : class
        {
            return GridCommands(builder, null, firstValue, values);
        }

        public static GridBoundColumnBuilder<T> GridCommands<T>(this GridBoundColumnBuilder<T> builder, int? width, string firstValue, params string[] values) where T : class
        {
            var template = new[] { firstValue }.Union(values).Select(v => string.Format("<span class=\"emsg-grid-action\">{0}</span>", v)).ToArray();
            return builder.ClientTemplate(string.Format("<div{0}>", width.HasValue ? " style=\"width: " + width.Value + "px\"" : "") + string.Join("", template) + "</div>")
                .Title("")
                .Sortable(false)
                .Filterable(false);
        }

        public static GridActionColumnBuilder AsEmsgCommand(this GridActionColumnBuilder builder)
        {
            return builder.Title("").Width(10);
        }

        public static GridActionColumnBuilder DeleteCommand<T>(this GridColumnFactory<T> factory) where T : class
        {
            return factory.Command(commands => commands.EmsgDelete()).AsEmsgCommand();
        }
    }
}