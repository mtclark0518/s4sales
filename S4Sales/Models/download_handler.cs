using System.Net;
using Microsoft.Extensions.Configuration;

namespace S4Sales.Models
{
    public class DownloadHandler
    {
        private const string pdf = "PDF";
        private readonly string imageRootUrl;
        private readonly string imageHttpUser;
        private readonly string imageHttpPassword;
        private readonly string warehouseConnStr;

        public DownloadHandler(IConfiguration config)
        {
            imageHttpPassword = config["AppSettings:ImageHttpPassword"];
            imageHttpUser = config["AppSettings:ImageHttpUser"];
            imageRootUrl = config["AppSettings:ImageRootUrl"];
            warehouseConnStr = config["ConnectionStrings:Warehouse"];
        }

        /// <summary>
        /// custom httphandler to process download requests
        /// .net core 2.1 doesn't support ihttphandler
        /// uses FolderManager to locate report
        /// does not need to add a watermark
        /// <param name="token">encrypted token string</param>
        /// </summary>

        // public void ProcessRequest(string hsmv_report)
        // {
            // var fm = new FolderManager(imageRootUrl, warehouseConnStr);
            // fm.TargetPathSeparator = @"/";

            // var url = fm.GetFilePath(int.Parse(hsmv_report));
            // if(!url.ToLower().EndsWith(".pdf"))
        //     {
        //         // TODO handle unknown image extensions
        //         return;
        //     }
        //     using(var client = new WebClient())
        //     {
        //         client.Credentials = new NetworkCredential(imageHttpUser, imageHttpPassword);
        //         var data = client.DownloadData(url);
        //     }
        // }
    }
}