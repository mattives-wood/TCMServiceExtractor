//using MigraDoc.DocumentObjectModel;
//using MigraDoc.Rendering;

//namespace PdfLib
//{
//    public class PDFDocument
//    {
//        private readonly Document _doc;
//        public PDFDocument()
//        {
//            _doc = new Document();
//        }

//        public void AddHeader(string text)
//        {
//            Section header = _doc.AddSection();
//            Paragraph paragraph = header.AddParagraph();
//            paragraph.AddText(text);
//        }

//        public void Save()
//        {
//            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
//            renderer.Document = _doc;
//            renderer.RenderDocument();

//            renderer.PdfDocument.Save(@"C:\temp\testPdf.pdf");
//        }
//    }
//}
