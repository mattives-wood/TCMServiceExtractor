using System;

namespace PdfLib
{
    public class Metadata
    {
        public int LegacyDocumentCodeId { get; set; }
        public int LegacyDocumentId { get; set; }
        public string LegacyDocumentName { get; set; }
        public int LegacyClientId { get; set; }
        public string EffectiveDate { get; set; }
        public string LegacyDocumentCategory { get; set; }
        public string PathToPDFFile { get; set; }
    }
}
