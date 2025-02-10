using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductModel
{
    public static class DBHelper
    {
        public static List<T> Get<T>(string resourceName)
        {
            {   
                using (StreamReader reader = new StreamReader(resourceName, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csvReader.Context.RegisterClassMap<Map>();
                    return csvReader.GetRecords<T>().ToList();
                }
            }
        }
        public static List<T> GetFile<T, S>(string resourceName) where T : class where S : ClassMap<T>
        {
            {
                using (StreamReader reader = new StreamReader(resourceName, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csvReader.Context.RegisterClassMap<S>();
                    return csvReader.GetRecords<T>().ToList();
                }
            }
        }
        public static Assembly GetReferencedAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName().Name == assemblyName);
        }

        public static List<T> GetResource<T, S>(string resourceName, string assemblyName) where T : class where S : ClassMap<T>
        {
            // Get the specified assembly
            Assembly assembly = GetReferencedAssembly(assemblyName);
            if (assembly == null)
            {
                throw new ArgumentException($"Assembly '{assemblyName}' not found.");
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assemblyName}'.");
                }

                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                        MissingFieldFound = null
                    };

                    CsvReader csvReader = new CsvReader(reader, configuration);
                    csvReader.Context.RegisterClassMap<S>();
                    return csvReader.GetRecords<T>().ToList();
                }
            }
        }
        public static List<T> GetResource<T, S>(string resourceName) where T : class where S : ClassMap<T>
        {
            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {   // create a stream reader
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null
                    };

                    // create a csv reader dor the stream
                    CsvReader csvReader = new CsvReader(reader, configuration);
                    csvReader.Context.RegisterClassMap<S>();
                    return csvReader.GetRecords<T>().ToList();
                }
            }
        }
    }

}

