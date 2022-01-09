using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XmlReader.FileWatcher.XmlFileHandling;

namespace XmlReader.FileWatcher
{
    class Program
    {
        private static IServiceProvider serviceProvider { get; set; }
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = GetConfiguredServiceCollection(configuration);
            serviceProvider = services
                .BuildServiceProvider();

            //Calling Services
            var Filet = serviceProvider.GetService<IFileContentHandler>();
            Filet.ConvertXmlToCsv();
               
        }

        private static IServiceCollection GetConfiguredServiceCollection(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IXmlContentReader, XmlContentReader>();
            serviceCollection.AddScoped<IFileContentHandler, FileContentHandler>();
            serviceCollection.AddSingleton(configuration);
            return serviceCollection;
        }
    }
}
