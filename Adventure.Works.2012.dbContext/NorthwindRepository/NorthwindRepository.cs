using Adventure.Works._2012.dbContext.Models;
using Adventure.Works._2012.dbContext.ResponseModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public NorthwindRepository(NorthwindContext context)
        {
            this.context = context;

        }
        public async Task<IEnumerable<ResponseEmployee>> GetAllAsyncEmployees()
        {

            var val = await context.Employees.Select(e => new ResponseEmployee
            {
                EmployeeId = e.EmployeeId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                Region = e.Region,
                HomePhone = e.HomePhone

            }).ToListAsync();


            return val;

        }

        public async Task<string> GetAllJsonStringEmployeesAsync()
        {


            var val = await context.Employees.Select(e => new ResponseEmployee
            {
                EmployeeId = e.EmployeeId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                Region = e.Region,
                HomePhone = e.HomePhone

            }).ToListAsync();

            DataTable table = GetEmployeeDataTable(val);
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

            column = new DataColumn();
            column.ColumnName = nameof(ResponseEmployee.Region);
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
                row[nameof(ResponseEmployee.Region)] = item.Region;

                table.Rows.Add(row);
            }

            return table;
        }


        public async Task<IEnumerable<ResponseOrder>> GetAllOrders()
        {
            var orders = await context.Orders.Select(o => new ResponseOrder
            {
                OrderId = o.OrderId,
                ShipAddress = o.ShipAddress,
                ShipCity = o.ShipCity,
                ShipPostalCode = o.ShipPostalCode,
                ShipRegion = o.ShipRegion,
                ShipCountry = o.ShipCountry,
                ShipName = o.ShipName,
                ShippedDate = o.ShippedDate,
                Freight = o.Freight,
                OrderDate = o.OrderDate,
                RequiredDate = o.RequiredDate
            }).ToListAsync();



            return orders;
        }

        public async Task<ResponseEmployee> GetAsyncEmployee(int id)
        {

            var val = await context.Employees.Select(e => new ResponseEmployee
            {
                EmployeeId = e.EmployeeId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                Region = e.Region,
                HomePhone = e.HomePhone

            }).FirstOrDefaultAsync(e => e.EmployeeId == id);

            return val;

        }

        public async Task<string> GetAllXmlStringEmployeesAsync()
        {


            var val = await context.Employees.Select(e => new ResponseEmployee
            {
                EmployeeId = e.EmployeeId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                Region = e.Region,
                HomePhone = e.HomePhone

            }).ToListAsync();




            DataTable table = GetEmployeeDataTable(val);
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


            var val = await context.Employees.Select(e => new ResponseEmployee
            {
                EmployeeId = e.EmployeeId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                Region = e.Region,
                HomePhone = e.HomePhone

            }).ToListAsync();



            DataTable table = GetEmployeeDataTable(val);
            //Create the header
            foreach (var col in table.Columns)
            {
                sb.AppendFormat("{0},", col.ToString());

            }
            sb.Replace(",", Environment.NewLine, sb.Length - 1, 1);

            foreach (DataRow dr in table.Rows)
            {

                foreach (var column in dr.ItemArray)
                {
                    string _val = string.Empty;
                    if (column != null)
                    {
                        _val = column.ToString();
                        //replace single qoutes in value with double quotes to escape the quotes
                        _val = _val.Replace("\"", "\"\"");
                        //Check is the value contains a delimiter and replace it in quotes
                        if (_val.Contains(","))
                        {
                            _val = string.Format("\"{0}\"", _val);
                        }

                        if (_val.Contains("\r"))
                        {
                            _val = _val.Replace("\r", " ");
                        }
                        if (_val.Contains("\n"))
                        {
                            _val = _val.Replace("\n", " ");
                        }



                    }

                    sb.AppendFormat("{0},", _val);
                }

                sb.Replace(",", Environment.NewLine, sb.Length - 1, 1);

            }


            return sb.ToString();
        }

        public async Task<IEnumerable<ResponseCustomerOrdTotals>> GetCustomersOrderTotals()
        {
            var result = await (from a in context.Orders
                         join b in context.OrderDetails on a.OrderId equals b.OrderId
                         group b by a.CustomerId into groupOrders
                         select new ResponseCustomerOrdTotals
                         {   
                             CustomerID = groupOrders.Key,
                             OrderTotal = groupOrders.Sum(o => (o.UnitPrice * o.Quantity))
                         }).ToListAsync();

            return result;

        }
    }
}
