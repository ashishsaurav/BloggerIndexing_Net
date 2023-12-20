using BloggerIndexing;
using ExcelDataReader;
using System.Data;

class Program
{
    public static void Main()
    {
        var googleBatchIndexingService = new GoogleBatchIndexingService();

        Console.WriteLine("Please enter url path");
        var urlPath = Console.ReadLine();
        List<string> urls = new List<string>();
        using (var stream = File.Open(urlPath, FileMode.Open, FileAccess.Read))
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IExcelDataReader reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration();
            var dataSet = reader.AsDataSet(conf);
            var dataTable = dataSet.Tables[0];
            foreach(DataRow dt in dataTable.Rows)
            {
                urls.Add(dt[0].ToString());
            }
        }

        var batchUpdateResult = googleBatchIndexingService.BatchAddOrUpdateGoogleIndex(urls);
        var updatedUrls = batchUpdateResult.Result;

        foreach(var url in updatedUrls)
        {
            Console.WriteLine(url.UrlNotificationMetadata.LatestUpdate.Url + " - " + url.UrlNotificationMetadata.LatestUpdate.Type);
        }
        
        Console.ReadLine();
    }
}