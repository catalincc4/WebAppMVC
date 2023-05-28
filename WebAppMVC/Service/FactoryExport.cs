using System.Data;
using System.Windows.Input;

namespace WebAppMVC.Service
{
    public class FactoryExport
    {
        public static IExporter Create(string type)
        {
            switch (type)
            {
                case "csv":
                    return new CsvExporter();
                case "xml":
                    return new XMLExporte();
                default:
                    throw new ArgumentException("Invalid command type.");
            }
        }
    }
}
