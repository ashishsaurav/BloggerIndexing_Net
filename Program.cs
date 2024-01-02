using BloggerIndexing;
using ExcelDataReader;
using System.Data;
using Microsoft.Extensions.Configuration;

class Program
{
    public static IConfigurationRoot ConfigurationFile { get; set; }
    public static void Main()
    {
        ConfigurationFile = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        var googleBatchIndexingService = new GoogleBatchIndexingService();

        var urlPath = ConfigurationFile.GetValue<string>("AppSettings:urlpath");
        List<string> urls = new List<string>();
        using (var stream = File.Open(urlPath, FileMode.Open, FileAccess.Read))
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IExcelDataReader reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration();
            var dataSet = reader.AsDataSet(conf);
            var dataTable = dataSet.Tables[0];
            foreach (DataRow dt in dataTable.Rows)
            {
                urls.Add(dt[0].ToString());
            }
        }

        var batchUpdateResult = googleBatchIndexingService.BatchAddOrUpdateGoogleIndex(urls);
        var updatedUrls = batchUpdateResult.Result;

        foreach (var url in updatedUrls)
        {
            Console.WriteLine(url.UrlNotificationMetadata.LatestUpdate.Url + " - " + url.UrlNotificationMetadata.LatestUpdate.Type);
        }

        Console.ReadLine();
    }
}
