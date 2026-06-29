using ERP_BL.ServiceRepos;
using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;



namespace ERPService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer1 = null;
        AutoExchangeRateRepo repo = new AutoExchangeRateRepo();
        private bool isPushRequired = true;
        private DateTime lastPushedA;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //var totalMilliSeconds = TimeSpan.FromMinutes(1).TotalMilliseconds;
            var totalMilliSeconds = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer1 = new Timer();
            this.timer1.Interval = totalMilliSeconds;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            Library.WriteErrorLog("Daily Exchange rate service is started.");

        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            //isPushRequired = repo.ShouldPushNewData();
            Library.WriteErrorLog("repo response got as: " + isPushRequired);
            if (isPushRequired) {
                isPushRequired = false;
                // now check if website is accessible. 
                Library.WriteErrorLog("Getting fileFromNationalBank");
                string filePath = getFileFromNationalBankAsync().Result;

                Library.WriteErrorLog("Got File path fron NBP: " + filePath);
                if ( !string.IsNullOrEmpty( filePath))
                {
                    // parse data from file. 
                    Library.WriteErrorLog("downloading file: " + filePath);
                    filePath = downloadFileToLocalPath(filePath);
                    Library.WriteErrorLog("downloaded file: " + filePath);
                    PdfReader reader = new PdfReader(filePath);
                    Library.WriteErrorLog("reading in pdf reader");
                    string fileText = string.Empty;
                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        Library.WriteErrorLog("reading page " + page);
                        fileText += PdfTextExtractor.GetTextFromPage(reader, page);
                    }
                    reader.Close();

                    Library.WriteErrorLog("File content: \n" + fileText);
                    Console.Write(fileText);


                }
            }
            
        }

        private async Task<string> getFileFromNationalBankAsync()
        {
            string filePath = "";
            Library.WriteErrorLog("client request sending to NBP");
            var client = new RestClient("https://www.nbp.com.pk/RATESHEET/index.aspx");
            var request = new RestRequest("Get");
            request.AddHeader("Cookie", "ASP.NET_SessionId=zscolwuegwwjs345k1e33vas");
            RestResponse response = await client.ExecuteGetAsync(request);
            Library.WriteErrorLog("response Got");

            HtmlDocument htmlDoc = new HtmlDocument();
            HttpStatusCode statusCode = response.StatusCode;
            Library.WriteErrorLog("response status code: " + statusCode);
            if (statusCode == HttpStatusCode.OK)
            if (!string.IsNullOrEmpty(response.Content))
            {
                    Library.WriteErrorLog("Loading content ");
                    htmlDoc.LoadHtml(response.Content);
                    // check if response 
                    Library.WriteErrorLog("Finding nodes");
                    var docNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@id,'ctl00_ContentPlaceHolder1_GridView1_')]");
                    Library.WriteErrorLog("Got total nodes as : " + docNodes.Count);
                    if (docNodes.Count > 0)
                    {

                        filePath = docNodes.First().GetAttributeValue("href", "");
                        filePath = !string.IsNullOrEmpty(filePath) ? "https://www.nbp.com.pk" + filePath.Replace("..", "") : filePath;
                        return filePath;
                    }
            }
            else
                return null;

            return filePath;
        }

        private string downloadFileToLocalPath(string filePath)
        {
            string localPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory , DateTime.Today.ToString("dd-mm-yyyy"));
            WebClient Client = new WebClient();
            Client.DownloadFile(filePath, localPath+".pdf");

            return localPath;

        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.WriteErrorLog("Test window service stopped");
        }
    }
}
