using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TransactionWebApp.Models;

namespace TransactionWebApp
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            RegisterLog4Net();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void RegisterLog4Net()
        {
            var log4NetConfig = new XmlDocument();
            log4NetConfig.Load(File.OpenRead("log4net.config"));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);
            Logger.Log = Log;
        }
    }
}
