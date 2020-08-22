using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sino.LogisticsWorkOrderManage.Web.Controllers
{
    public class BaseController: Controller
    {
        public static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 一级结构数据通用导出
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">文件名</param>
        /// <param name="menus">表头中文名</param>
        /// <param name="dataList">数据</param>
        /// <param name="getCellValue">单元格数据表达式</param>
        protected MemoryStream ExportWithOneTier<T>(string name, List<string> menus, IEnumerable<T> dataList, Func<T, int, string> getCellValue)
        {
            //创建工作薄
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个表
            ISheet tb = wk.CreateSheet();

            ICellStyle cellStyle = wk.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
            cellStyle.Alignment = HorizontalAlignment.Center;//水平对齐
            cellStyle.IsHidden = false;
            IFont fontStyle = wk.CreateFont();
            fontStyle.FontName = "微软雅黑";//字体
            fontStyle.FontHeightInPoints = 11;//字号
            fontStyle.IsBold = true;//粗体
            cellStyle.SetFont(fontStyle);
            int maxColumn = menus.Count;

            //创建表头

            IRow header = tb.CreateRow(0);
            header.ZeroHeight = false;
            header.HeightInPoints = 24;
            for (int i = 0; i < maxColumn; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(menus[i]);
                cell.CellStyle = cellStyle;
            }

            int count = 0;
            foreach (var item in dataList)
            {
                count++;
                IRow row = tb.CreateRow(count);
                row.ZeroHeight = false;
                for (int i = 0; i < maxColumn; i++)
                {
                    //创建单元格
                    ICell cell = row.CreateCell(i);
                    string value = getCellValue(item, i);
                    cell.SetCellValue(value);//循环往单元格中添加数据
                }
            }

            string fileName = name + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            HttpResponse response = HttpContext.Response;
            MemoryStream ms = new MemoryStream();
            wk.Write(ms);
            ms.Position = 0;
            response.Clear();
            response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Method", "*");
            response.Headers.Add("Access-Control-Allow-Header", "*");
            response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return ms;
        }
    }
}
