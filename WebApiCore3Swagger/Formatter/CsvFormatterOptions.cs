using System.Text;

namespace WebApiCore3Swagger.Formatter
{
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;

        public string CsvDelimiter { get; set; } = ",";

        public string ContentType { get; set; } = "text/csv";
        public Encoding Encoding { get; set; } = Encoding.Default;

        public bool IncludeExcelDelimiterHeader { get; set; }
    }
}
