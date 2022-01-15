using DecoupledBot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DecoupledBot.Utils
{
    public class CSVConverter
    {
        public static List<Stock> GetValues(string input)
        {

            DataTable dt = new DataTable();

            string[] tableData = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var col = from cl in tableData[0].Split(",".ToCharArray())
                      select new DataColumn(cl);
            dt.Columns.AddRange(col.ToArray());

            (from st in tableData.Skip(1)
             select dt.Rows.Add(st.Split(",".ToCharArray()))).ToList();

            List<Stock> list = new List<Stock>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Stock
                {
                    Symbol = dr["Symbol"].ToString(),
                    Date = dr["Date"].ToString(),
                    Time = dr["Time"].ToString(),
                    Open = dr["Open"].ToString(),
                    High = dr["High"].ToString(),
                    Low = dr["Low"].ToString(),
                    Close = dr["Close"].ToString(),
                    Volume = dr["Volume"].ToString(),
                });
            }

            return list;
        }
    }
}
