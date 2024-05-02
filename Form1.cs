using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CefSharpTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string htmlContent = @"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><title>Hello World</title></head><body><h1>Hello World</h1></body></html>";
                await CefSharpHelper.GenerateAndPrintPdf(htmlContent);
                //MessageBox.Show("PDF生成成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF生成失败: {ex.Message}");
            }
        }
    }
}
