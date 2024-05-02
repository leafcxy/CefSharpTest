using CefSharp;
using CefSharp.OffScreen;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

//using Spire.License;
using Spire.Pdf;

namespace CefSharpTest
{
    public static class CefSharpHelper
    {
        public static async Task GenerateAndPrintPdf(string htmlContent)
        {
            string tempPdfPath = await GeneratePdf(htmlContent);
            // 这里可以添加代码来处理生成的PDF文件，比如直接发送到打印机打印
            // 注意：实际打印PDF到物理打印机的步骤通常涉及到操作系统特定的API或外部打印服务
            // 以下仅为示意，具体打印实现会根据你的应用环境有所不同
            PrintPdf(tempPdfPath);

            // 打印完成后，可以考虑删除临时文件
            //File.Delete(tempPdfPath);
        }

        public static async Task<string> GeneratePdf(string htmlContent)
        {
            // 确保CefSharp已初始化
            if (!Cef.IsInitialized && !Cef.Initialize(new CefSettings()))
            {
                throw new Exception("Unable to Initialize Cef");
            }

            // 使用CefSharp.OffScreen浏览器
            using (var browser = new ChromiumWebBrowser())
            {
                // 加载HTML内容
                Thread.Sleep(100);
                browser.LoadHtml(htmlContent);
                await browser.WaitForInitialLoadAsync();
                // 配置PDF打印设置
                var pdfSettings = new PdfPrintSettings
                {
                    Landscape = false, // 是否横向打印
                    MarginType = CefPdfPrintMarginType.Custom, // 或自定义页边距
                    MarginBottom = 0,
                    MarginLeft = 0,
                    MarginRight = 0,
                    MarginTop = 0,
                    PageWidth = 210 * 1000,//单位微米，网页上的长度单位建议使用毫米mm
                    PageHeight = 297 * 1000,
                    HeaderFooterEnabled = false,
                    BackgroundsEnabled = true, // 是否打印背景颜色和图片
                };

                // 使用临时文件路径来保存PDF
                string tempPdfPath = Path.GetTempFileName() + ".pdf";
                Console.WriteLine(tempPdfPath);

                // 生成PDF文件
                bool success = await browser.PrintToPdfAsync(tempPdfPath, pdfSettings);
                if (!success)
                {
                    Console.WriteLine("-" + tempPdfPath + "-");
                    throw new Exception("Failed to generate PDF.");
                }
                return tempPdfPath;
            }
        }

        // 以下是一个示意性的方法，展示如何使用外部进程或服务来打印PDF文件
        public static void PrintPdf(string pdfFilePath)
        {
            // 这里可以调用系统默认的PDF阅读器或打印服务来打印PDF
            // 例如，在Windows上可能使用Process.Start来启动打印进程
            // 注意：实际实现会根据你的具体需求和环境调整
            // Process.Start("Acrobat.exe", "/t " + pdfFilePath + " \"Printer Name\"");

            //创建PdfDocument类的对象，并加载PDF文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfFilePath);
            //此行代码为选择打印机名称来打印
            //doc.PrintSettings.PrinterName="打印机名称";
            //直接打印会调用电脑的默认打印机进行打印，请在控制面板->设备的打印机中配置默认打印机
            doc.Print();
        }
    }
}
