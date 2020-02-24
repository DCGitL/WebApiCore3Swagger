using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimDate
    {
        public DimDate()
        {
            FactFinance = new HashSet<FactFinance>();
        }

        public int DateKey { get; set; }
        public DateTime FullDateAlternateKey { get; set; }
        public byte DayNumberOfWeek { get; set; }
        public string EnglishDayNameOfWeek { get; set; }
        public string SpanishDayNameOfWeek { get; set; }
        public string FrenchDayNameOfWeek { get; set; }
        public byte DayNumberOfMonth { get; set; }
        public short DayNumberOfYear { get; set; }
        public byte WeekNumberOfYear { get; set; }
        public string EnglishMonthName { get; set; }
        public string SpanishMonthName { get; set; }
        public string FrenchMonthName { get; set; }
        public byte MonthNumberOfYear { get; set; }
        public byte CalendarQuarter { get; set; }
        public short CalendarYear { get; set; }
        public byte CalendarSemester { get; set; }
        public byte FiscalQuarter { get; set; }
        public short FiscalYear { get; set; }
        public byte FiscalSemester { get; set; }

        public virtual ICollection<FactFinance> FactFinance { get; set; }
    }
}
