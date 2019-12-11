using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

public class DataTableToCSV
{
	public DataTableToCSV()
	{		
	}
 
    public static void DownloadCSV(DataTable Table, string FileName)
    {
          
        StringBuilder csv = new StringBuilder();
        for (int c = 0; c < Table.Columns.Count; c++)
        {
            if (c > 0)
                csv.Append(",");
            DataColumn dc = Table.Columns[c];
            string columnTitleCleaned = CleanCSVString(dc.ColumnName);
            csv.Append(columnTitleCleaned);
        }
        csv.Append(Environment.NewLine);
        foreach (DataRow dr in Table.Rows)
        {
            StringBuilder csvRow = new StringBuilder();
            for (int c = 0; c < Table.Columns.Count; c++)
            {
                if (c != 0)
                    csvRow.Append(",");

                object columnValue = dr[c];
                if (columnValue == null)
                    csvRow.Append("");
                else
                {
                    string columnStringValue = columnValue.ToString();


                    string cleanedColumnValue = CleanCSVString(columnStringValue);

                   
                    csvRow.Append(cleanedColumnValue);
                }
            }
            csv.AppendLine(csvRow.ToString());
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ClearContent();
        // HttpContext.Current.Response.Charset = "ANSI";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
        HttpContext.Current.Response.ContentType = "application/download";
        HttpContext.Current.Response.Write(csv.ToString());
    }
    protected static string CleanCSVString(string input)
    {
        string output = "\"" + input.Replace (",",";").Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
        return output;
    }
    public static void ExportToCSVFile(DataTable dtTable)
    {
        StringBuilder sbldr = new StringBuilder();
        if (dtTable.Columns.Count != 0)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sbldr.Append(col.ColumnName + ',');
            }
            sbldr.Append("\r\n");
            foreach (DataRow row in dtTable.Rows)
            {
                foreach (DataColumn column in dtTable.Columns)
                {
                    sbldr.Append(row[column].ToString() + ',');
                }
                sbldr.Append("\r\n");
            }
        } 
        HttpContext.Current.Response.ContentType = "Application/x-msexcel";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=test.csv");
        HttpContext.Current.Response.Write(sbldr.ToString());
        HttpContext.Current.Response.End(); 
    }
 
}