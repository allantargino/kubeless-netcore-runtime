namespace Kubeless.WebAPI.Utils
{
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static void AppendKeyValue(this StringBuilder builder, string key, string value)
        {
            if(builder != null) 
            {
                builder.AppendLine($"# {key}: {value}");
            }
        }

        public static void AppendCode(this StringBuilder builder, string key, string code)
        {
            if(builder != null)
            {
                builder.AppendLine($"# {key}:");
                builder.AppendLine(code);
            }
        }
    }
}
