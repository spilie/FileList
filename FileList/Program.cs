using log4net;
using System;
using System.Configuration;
using System.IO;
using Newtonsoft;
using System.Linq;
using System.Collections.Generic;
using FileList.Model;
using Newtonsoft.Json;

namespace FileList
{
    class Program
    {
        private static ILog mLogger { get; set; }

        private static ILog oLogger { get; set; }

        static void Main(string[] args)
        {
            init();

            string[] folders = ConfigurationManager.AppSettings["Folder"].Split(',');
            List<FolderInfo> folderInfos = new List<FolderInfo>();

            foreach (string folder in folders)
            {
                DirectoryInfo info = new DirectoryInfo(folder);

                DirectoryInfo[] dinfos = info.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo dinfo in dinfos)
                {

                    oLogger.Info(string.Format("資料夾名稱 : {0} 路徑 : {1}", dinfo.Name, dinfo.FullName));

                    FileInfo[] finfos = dinfo.GetFiles("*", SearchOption.AllDirectories);

                    FolderInfo folderinfo = (new FolderInfo() { Name = dinfo.Name, FullName = dinfo.FullName });
                    List<DocInfo> docInfos = new List<DocInfo>();
                    foreach (FileInfo finfo in finfos)
                    {
                        docInfos.Add(new DocInfo() { Name = finfo.Name, FullName = finfo.FullName });

                        oLogger.Info(string.Format("    檔案名稱 : {0} 路徑 : {1}", dinfo.Name, dinfo.FullName));
                    }

                    folderinfo.Docs = docInfos;

                    folderInfos.Add(folderinfo);
                }
            }

            outJson(folderInfos);
        }
        
        private static void init()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            mLogger = LogManager.GetLogger("MainLogger");
            oLogger = LogManager.GetLogger("OutputLogger");

            mLogger.Debug("system start...");
            oLogger.Debug("system start...");
        }

        private static void outJson(List<FolderInfo> infos)
        {
            string allJson = JsonConvert.SerializeObject(infos);

            string exportName = ConfigurationManager.AppSettings["oJsonName"];
            File.Create(exportName).Dispose();

            using (StreamWriter sw = new StreamWriter(exportName))
            {
                sw.Write(allJson);

                sw.Close();
                sw.Dispose();
            }
        }
    }
}
