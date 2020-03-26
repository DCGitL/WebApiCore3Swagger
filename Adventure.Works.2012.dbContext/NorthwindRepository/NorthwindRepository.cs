using Adventure.Works._2012.dbContext.Models;
using Adventure.Works._2012.dbContext.ResponseModels;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure.Works._2012.dbContext.Northwind.Repository
{
    public class NorthwindRepository : INorthwindRepository
    {
        private readonly NorthwindContext context;
        private readonly IMapper map;

        public NorthwindRepository(NorthwindContext context, IMapper map)
        {
            this.context = context;
            this.map = map;
        }
        public async Task<IEnumerable<ResponseEmployee>> GetAllAsyncEmployees()
        {
            var results = await Task.Run(() =>
            {
                var val = context.Employees;
                var mapperemployees = map.Map<List<ResponseEmployee>>(val);

                return mapperemployees;
            });

            return results;
        }

        public async Task<string> GetAllJsonStringEmployeesAsync()
        {
            var memployees = await Task.Run(() =>
            {
                var val = context.Employees;
                var mapperemployees = map.Map<List<ResponseEmployee>>(val);
                return mapperemployees;
            });

            DataTable table = GetEmployeeDataTable(memployees);
            string JsonString = string.Empty;
            if (table.Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(table);
            }
            return JsonString;
        }

        private static DataTable GetEmployeeDataTable(List<ResponseEmployee> memployees)
        {
            DataTable table = new DataTable("Employee");
            table.Namespace = "http://employee.namespase";


            DataColumn column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.EmployeeId);
            column.DataType = typeof(int);
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.FirstName);
            column.DataType = typeof(string);
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.LastName);
            column.DataType = typeof(string);
            table.Columns.Add(column);


            column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.Title);
            column.DataType = typeof(string);
            table.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.Address);
            column.DataType = typeof(string);
            table.Columns.Add(column);

            DataRow row = null;

            foreach (var item in memployees)
            {
                row = table.NewRow();
                row[nameof(ResponseEmployee.EmployeeId)] = item.EmployeeId;
                row[nameof(ResponseEmployee.FirstName)] = item.FirstName;
                row[nameof(ResponseEmployee.LastName)] = item.LastName;
                row[nameof(ResponseEmployee.Title)] = item.Title;
                row[nameof(ResponseEmployee.Address)] = item.Address;

                table.Rows.Add(row);
            }

            return table;
        }


        public async Task<IEnumerable<ResponseOrder>> GetAllOrders()
        {
            var orders = await Task.FromResult(context.Orders);

            var mapperOrders = map.Map<List<ResponseOrder>>(orders);

            return mapperOrders;
        }

        public async Task<ResponseEmployee> GetAsyncEmployee(int id)
        {
            var result = await Task.Run(() =>
            {
                var val = context.Employees.FirstOrDefault(e => e.EmployeeId == id);
                var mapemployee = map.Map<ResponseEmployee>(val);
                return mapemployee;

            });


            return result;
        }

        public async Task<string> GetAllXmlStringEmployeesAsync()
        {

            var memployees = await Task.Run(() =>
            {
                var val = context.Employees;
                var mapperemployees = map.Map<List<ResponseEmployee>>(val);
                return mapperemployees;
            });

            DataTable table = GetEmployeeDataTable(memployees);
            string xmlstr = string.Empty;
            StringBuilder xmlstrb = new StringBuilder(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            using (MemoryStream str = new MemoryStream())
            {
                try
                {
                    table.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(str, Encoding.UTF8))
                    {
                        xmlstr = sr.ReadToEnd();
                        xmlstrb.Append(xmlstr);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw ex;
                }

            }


            return xmlstrb.ToString();
        }

        public async Task<string> GetAllCSVStringEmployeesAsync()
        {
            StringBuilder sb = new StringBuilder();

            var memployees = await Task.Run(() =>
            {
                var val = context.Employees;
                var mapperemployees = map.Map<List<ResponseEmployee>>(val);
                return mapperemployees;
            });

            DataTable table = GetEmployeeDataTable(memployees);
            //Create the header
            foreach(var col in table.Columns)
            {
                sb.AppendFormat("{0},", col.ToString());

            }
            sb.Replace(",", Environment.NewLine, sb.Length -1, 1);

            foreach (DataRow dr in table.Rows)
            {
               
                foreach(var column in dr.ItemArray)
                {
                    sb.AppendFormat("\"{0}\",", column.ToString());
                }

                sb.Replace(",", Environment.NewLine, sb.Length - 1, 1);

            }


            return sb.ToString();
        }
    }
}
