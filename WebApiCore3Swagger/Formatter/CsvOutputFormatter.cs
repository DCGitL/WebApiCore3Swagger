using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Formatter
{
    public class CsvOutputFormatter : OutputFormatter
    {
        private readonly CsvFormatterOptions options;
        private readonly bool UseJsonAttributes = true;
        public string ContentType { get; private set; }
        public CsvOutputFormatter(CsvFormatterOptions formatteroptions)
        {
            ContentType = formatteroptions.ContentType;
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(ContentType));
            this.options = formatteroptions ?? throw new ArgumentNullException(nameof(formatteroptions));

        }

        protected override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return typeof(IEnumerable).IsAssignableFrom(type);

        }

        private string GetDisplayNameFromNewtonsoftJsonAnnotations(PropertyInfo pi)
        {
            if (pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.PropertyName is string value)
            {
                return value;
            }

            return pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            Type type = context.Object.GetType();
            Type itemType;
            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments().First();
            }
            else
            {
                itemType = type.GetElementType();
            }

            var streamWriter = new StreamWriter(response.Body, options.Encoding);

            if (options.IncludeExcelDelimiterHeader)
            {
                await streamWriter.WriteLineAsync($"sep ={options.CsvDelimiter}");
            }
            if (options.UseSingleLineHeaderInCsv)
            {
                var values = UseJsonAttributes ? itemType.GetProperties().Where(p => !p.GetCustomAttributes<JsonIgnoreAttribute>(false).Any())
                    .Select(p => new
                    {
                        Order = p.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                        Prop = p
                    }).OrderBy(o => o.Order).Select(o => GetDisplayNameFromNewtonsoftJsonAnnotations(o.Prop))
                    : itemType.GetProperties().Select(p => p.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? p.Name);

                await streamWriter.WriteLineAsync(string.Join(options.CsvDelimiter, values));
            }

            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                var vals = UseJsonAttributes ? obj.GetType().GetProperties()
                    .Where(p => !p.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                    .Select(p => new
                    {
                        Order = p.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                        Value = p.GetValue(obj, null)
                    }).OrderBy(o => o.Order).Select(o => new { o.Value })
                    : obj.GetType().GetProperties().Select(p => new
                    {
                        Value = p.GetValue(obj, null)
                    });

                string valueLine = string.Empty;
                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {
                        var _val = val.Value.ToString();
                        //escape quotes
                        _val = _val.Replace("\"", "\"\"");
                        //check is the value contains a delimiter and put it in quotes 
                        if (_val.Contains(options.CsvDelimiter))
                        {
                            _val = string.Concat("\"", _val, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                        {
                            _val = _val.Replace("\r", " ");
                        }

                        if (_val.Contains("\n"))
                        {
                            _val = _val.Replace("\n", " ");

                        }
                        valueLine = string.Concat(valueLine, _val, options.CsvDelimiter);
                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, options.CsvDelimiter);
                    }
                }

                await streamWriter.WriteLineAsync(valueLine.Remove(valueLine.Length - options.CsvDelimiter.Length));


            }

            await streamWriter.FlushAsync();


        }
    }
}
