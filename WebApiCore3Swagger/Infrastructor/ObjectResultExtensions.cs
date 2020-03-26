using Microsoft.AspNetCore.Mvc;

namespace WebApiCore3Swagger.Infrastructor
{
    public static class ObjectResultExtensions
    {
        public static T ForceResultAsXml<T>(this T result) where T : ObjectResult
        {
            result.Formatters.Clear();
            result.Formatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());

            return result;
        }
    }
}
