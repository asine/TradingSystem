﻿using Newtonsoft.Json;
using OpenHtmlToPdf;
using Oybab.DAL;
using Oybab.Report.CommonHWP;
using Oybab.Report.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oybab.Report.StatisticsHWP
{
    public sealed partial class ProductProfitReport : HWPReport
    {


        public ProductProfitReport(string PriceSymbol)
        {
            this.PriceSymbol = PriceSymbol;


            WidhtMillimeters = 210 - 40; // html right20 + left20 margin 40
            HeightMillimeters = 280 - 20; // html top10 + bottom10 margin 20
            MarginsMillimeters = PaperMargins.All(Length.Millimeters(0));
        }



        internal override string ProcessHTMLContent(ReportModel reportModel)
        {
          
            string htmlContent = GetResourceFileContentAsString("ProductProfitReport.html", "Oybab.Report.StatisticsHWP.Resources.");


            htmlContent = htmlContent.Replace(@"<!--${DynamicImportJquery}-->", string.Format("<script type=\"text/javascript\" > {0}</script>", GetResourceFileContentAsString("JS.jquery-1.12.4.min.js")));





            StringBuilder newStr = new StringBuilder();

            // 先处理字体
            foreach (var item in reportModel.Fonts)
            {
                newStr.Append(".").Append(item.Key).Append("{").Append("color:#000000; font-family:'").Append(item.Value.FontFamily.Name).Append("'; font-size:").Append(item.Value.Size + "pt;}").AppendLine();
            }
            htmlContent = htmlContent.Replace("/*${DynamicStyles}*/", newStr.ToString());


            // 处理参数
            newStr.Clear();

            reportModel.Parameters.Add("CalcCount", string.Format("{0:0.00}", Math.Round((reportModel.DataSource as List<RecordProfitProducts>).Sum(x => x.CostPrice), 2)));
            reportModel.Parameters.Add("CalcCostPrice", string.Format("{0:" + PriceSymbol + "0.00}", Math.Round((reportModel.DataSource as List<RecordProfitProducts>).Sum(x => x.CostPrice), 2)));
            reportModel.Parameters.Add("CalcPrice", string.Format("{0:" + PriceSymbol + "0.00}", Math.Round((reportModel.DataSource as List<RecordProfitProducts>).Sum(x => x.Price), 2)));
            reportModel.Parameters.Add("CalcProfitPrice", string.Format("{0:" + PriceSymbol + "0.00}", Math.Round((reportModel.DataSource as List<RecordProfitProducts>).Sum(x => x.ProfitPrice), 2)));
            reportModel.Parameters.Add("MaxPrice", Math.Round((reportModel.DataSource as List<RecordProfitProducts>).Max(x => x.ProfitPrice), 2));

            var parametersJson = reportModel.Parameters.Select(x => new Dictionary<string, string>()
            {
                { x.Key, (x.Value is double && (x.Key != "MaxPrice") && x.Key != "CalcCount") ? string.Format("{0:" + PriceSymbol + "0.00}", x.Value) : x.Value.ToString()}
            });



            newStr.Append("var parameters = \'").Append(JsonConvert.SerializeObject(parametersJson)).Append("\';");
            htmlContent = htmlContent.Replace("/*${DynamicParameter}*/", newStr.ToString());


            List<RecordProfitProducts> dataSource = reportModel.DataSource as List<RecordProfitProducts>;
            // 处理DataSource
            if (null != dataSource && dataSource.Count > 0)
            {
                newStr.Clear();
                var detailsJson = dataSource.Select(x => new
                {
                    ProductName = x.Name,
                    Count = x.Count,
                    CostPrice = string.Format("{0:" + PriceSymbol + "0.00}", x.CostPrice),
                    Price = string.Format("{0:" + PriceSymbol + "0.00}", x.Price),
                    ProfitPrice = string.Format("{0:" + PriceSymbol + "0.00}", x.ProfitPrice),
                    ProfitDouble = x.ProfitPrice,
                });

                newStr.Append("var dataSource = \'").Append(JsonConvert.SerializeObject(detailsJson)).Append("\';");
                htmlContent = htmlContent.Replace("/*${DynamicDataSource}*/", newStr.ToString());
            }



            htmlContent = htmlContent.Replace("/*${DynamicIsGenerated}*/", "var isGenerated = true;");
            



            return htmlContent;

        }


        




    }
}
