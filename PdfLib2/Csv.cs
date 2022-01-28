using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;

namespace PdfLib
{
    public static class Csv
    {
        public static void WriteCsvFile(List<Metadata> metadata, string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };

            using (FileStream stream = File.Open(path, FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (CsvWriter csv = new CsvWriter(writer, config))
                    {
                        csv.WriteRecords(metadata);
                    }
                }
            }
        }

        public static int GetLastSeqNum(string path)
        {
            if (!File.Exists(path))
            {
                CreateNewFile(path);
                return 0;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Metadata lastRecord = csv.GetRecords<Metadata>().Last();
                    return lastRecord.LegacyDocumentId;
                }
            }
        }

        private static void CreateNewFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Metadata>();
                    csv.NextRecord();
                }
            }
        }
    }
}
