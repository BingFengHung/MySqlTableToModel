using System.Globalization;

namespace MySqlTableToModel
{
    static class TextFormat
    {
        public static string SnakeCaseToPascalCase(string text)
        {
            string convert = text.Replace("_", " ");
            convert = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(convert);
            convert = convert.Replace(" ", "");
            return convert;
        }
    }
}
