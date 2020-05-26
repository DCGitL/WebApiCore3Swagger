using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApp1
{
    /// <summary>
    /// This class object can return any delimited string fron an enumberable list of any objects,
    /// It also return a data table, xmlRoot element of a xml document and a json string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyGenericEnumerable<T>
    {
        private readonly string _tableName;
        private readonly string _tableNamespace;
        private const char csvdelimitor = ',';
        public MyGenericEnumerable()
        {
            _tableName = typeof(T).Name;
            _tableNamespace = $"http://{_tableName}.com";
        }
        public MyGenericEnumerable(string tableName, string tableNamespace)
        {
            _tableName = string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName;
            _tableNamespace = tableNamespace;
        }

        public string GetDelimitedString(IEnumerable<T> enumlist, char delimitor)
        {
           
            StringBuilder sb = new StringBuilder();
            DataTable table = GetTable(enumlist);
            char localdelimitor = csvdelimitor;
            bool isCsvDelimitor = csvdelimitor == delimitor;

            if (!isCsvDelimitor)
            {
                localdelimitor = delimitor;
            }

            //create the header
            foreach (var col in table.Columns)
            {
                sb.AppendFormat("{0}{1}", col.ToString(), localdelimitor);
            }

            sb.Replace(localdelimitor.ToString(), Environment.NewLine, sb.Length - 1, 1);

            foreach (DataRow dr in table.Rows)
            {
                foreach (var column in dr.ItemArray)
                {
                    if (isCsvDelimitor)
                    {
                        sb.AppendFormat("\"{0}\"{1}", column.ToString(), localdelimitor);

                    }
                    else
                    {
                        sb.AppendFormat("{0}{1}", column.ToString(), localdelimitor);
                    }

                }

                sb.Replace(localdelimitor.ToString(), Environment.NewLine, sb.Length - 1, 1);
            }


            return sb.ToString();
        }

        public XmlElement GetXmlDocumentElement(IEnumerable<T> enumlist)
        {

            DataTable table = GetTable(enumlist);
            StringBuilder xmlsb = new StringBuilder();
            using (MemoryStream mstr = new MemoryStream())
            {
                table.WriteXml(mstr, true);
                mstr.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(mstr, Encoding.UTF8))
                {
                    var str = sr.ReadToEnd();
                    xmlsb.Append(str);
                }

            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlsb.ToString());
            XmlElement xmlElement = xmldoc.DocumentElement;

            return xmlElement;

        }
        public DataTable GetTable(IEnumerable<T> enumlist)
        {

            DataTable table = new DataTable(_tableName, _tableNamespace);

            IList<T> list = enumlist as List<T>;
            if (list == null)
            {
                throw new NullReferenceException();
            }
            //create the table header

            DataColumn column = null;
            var firstItem = list.FirstOrDefault<T>();
            foreach (var property in firstItem.GetType().GetProperties())
            {
                var displayname = property.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().SingleOrDefault();
                var propertyName = displayname == null ? property.Name : displayname.Name;
                column = new DataColumn();
                column.ColumnName = propertyName;
                column.DataType = property.GetValue(firstItem).GetType();
                table.Columns.Add(column);
            }

            //create the rows for the table
            DataRow tablerow = null;
            foreach (var item in list)
            {
                tablerow = table.NewRow();
                var itemproperties = item.GetType().GetProperties();
                object[] itemArray = new object[itemproperties.Length];
                int arrayindex = 0;
                foreach (var propertyItem in itemproperties)
                {
                    var propval = propertyItem.GetValue(item);
                    var colname = propertyItem.Name;
                    itemArray[arrayindex] = propval;

                    arrayindex++;
                }

                tablerow.ItemArray = itemArray;
                table.Rows.Add(tablerow);
            }

            return table;
        }


        public string GetJsonString(IEnumerable<T> enumlist)
        {
            var jsonstr = JsonConvert.SerializeObject(enumlist);

            return jsonstr;

        }

    }
}



       