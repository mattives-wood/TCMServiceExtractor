using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Data;

using Domain;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PdfLib
{
    public class PDFDocument
    {
        private readonly string _path;
        private readonly string _extension = "pdf";

        public PDFDocument(string path)
        {
            _path = path;
        }

        public void GeneratePdf(Client client)
        {
        }

        private void RenderSingle(Client client)
        {
            string path = $"{_path}\\{client.ClientId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //List<Metadata> metas = new List<Metadata>();
            //int lastSeqNum = Csv.GetLastSeqNum(_metadataFile);

            foreach (Contacts contact in client.Contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, new List<Contacts> { contact });
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string filename = $"{contact.ServDate.Value.ToString("yyyy-MM-dd+HH-mm-ss")}+{contact.ServiceCode.Code}+{contact.KeyId}";

                renderer.PdfDocument.Save($"{path}\\{filename}.{_extension}");

                //metas.Add(
                //    new Metadata()
                //    {
                //        EffectiveDate = contact.ServDate.Value.ToShortDateString(),
                //        LegacyDocumentId = ++lastSeqNum,
                //        LegacyClientId = client.ClientId,
                //        LegacyDocumentCategory = "Service Notes",
                //        LegacyDocumentCodeId = 60078,
                //        LegacyDocumentName = filename,
                //        PathToPDFFile = $"{client.ClientId}\\{filename}.{_extension}"
                //    });
            }

            //Csv.WriteCsvFile(metas, _metadataFile);
        }

        private void RenderDaily(Client client)
        {
            string path = $"{_path}\\{client.ClientId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            IEnumerable<IGrouping<DateTime, Contacts>> contacts = client.Contacts.GroupBy(c => c.ServDate.Value.Date);
            //List<Metadata> metas = new List<Metadata>();
            //int lastSeqNum = Csv.GetLastSeqNum(_metadataFile);

            foreach (IGrouping<DateTime, Contacts> group in contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string filename = $"{group.Key.ToString("yyyy-MM-dd")}";
                renderer.PdfDocument.Save($"{path}\\{filename}.{_extension}");

                //metas.Add(
                //    new Metadata()
                //    {
                //        EffectiveDate = contacts.First().First().ServDate.Value.ToShortDateString(),
                //        LegacyDocumentId = ++lastSeqNum,
                //        LegacyClientId = client.ClientId,
                //        LegacyDocumentCategory = "Service Notes",
                //        LegacyDocumentCodeId = 60078,
                //        LegacyDocumentName = filename,
                //        PathToPDFFile = $"{client.ClientId}\\{filename}.{_extension}"
                //    });
            }

            //Csv.WriteCsvFile(metas, _metadataFile);
        }

        private void RenderMonthly(Client client)
        {
            string path = $"{_path}\\{client.ClientId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            IEnumerable<IGrouping<DateTime, Contacts>> contacts = client.Contacts.GroupBy(c => new DateTime(c.ServDate.Value.Year, c.ServDate.Value.Month, 01));
            //List<Metadata> metas = new List<Metadata>();
            //int lastSeqNum = Csv.GetLastSeqNum(_metadataFile);

            foreach (IGrouping<DateTime, Contacts> group in contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string filename = $"{group.Key.ToString("yyyy-MM")}";
                renderer.PdfDocument.Save($"{path}\\{filename}.{_extension}");

                DateTime effectiveDate = new DateTime(contacts.First().First().ServDate.Value.Year, contacts.First().First().ServDate.Value.Month, 1);
                //metas.Add(
                //    new Metadata()
                //    {
                //        EffectiveDate = effectiveDate.ToShortDateString(),
                //        LegacyDocumentId = ++lastSeqNum,
                //        LegacyClientId = client.ClientId,
                //        LegacyDocumentCategory = "Service Notes",
                //        LegacyDocumentCodeId = 60078,
                //        LegacyDocumentName = filename,
                //        PathToPDFFile = $"{client.ClientId}\\{filename}.{_extension}"
                //    });
            }

            //Csv.WriteCsvFile(metas, _metadataFile);
        }

        private void RenderYearly(Client client)
        {
            string path = $"{_path}\\{client.ClientId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            IEnumerable<IGrouping<DateTime, Contacts>> contacts = client.Contacts.GroupBy(c => new DateTime(c.ServDate.Value.Year, 01, 01));
            //List<Metadata> metas = new List<Metadata>();
            //int lastSeqNum = Csv.GetLastSeqNum(_metadataFile);

            foreach (IGrouping<DateTime, Contacts> group in contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string filename = $"{group.Key.ToString("yyyy")}";
                renderer.PdfDocument.Save($"{path}\\{filename}.{_extension}");

                DateTime effectiveDate = new DateTime(contacts.First().First().ServDate.Value.Year, 1, 1);
                //metas.Add(
                //    new Metadata()
                //    {
                //        EffectiveDate = effectiveDate.ToShortDateString(),
                //        LegacyDocumentId = ++lastSeqNum,
                //        LegacyClientId = client.ClientId,
                //        LegacyDocumentCategory = "Service Notes",
                //        LegacyDocumentCodeId = 60078,
                //        LegacyDocumentName = filename,
                //        PathToPDFFile = $"{client.ClientId}\\{filename}.{_extension}"
                //    });
            }

            //Csv.WriteCsvFile(metas, _metadataFile);
        }

        private void ProcessServiceInfo(Document doc, List<Contacts> contacts)
        {
            foreach (Contacts contact in contacts)
            {
                Paragraph paragraph = doc.LastSection
                                        .AddParagraph(
                                            $"{contact.ServDate}" +
                                            $" | {contact.Activity} - {contact.ServiceCode.Description}" +
                                            $" | {contact.StaffEmployee.LastName}, {contact.StaffEmployee.FirstName}", "Heading1");

                StringBuilder sb = new StringBuilder();
                sb.Append($"Date of Service:\t{contact.ServDate}");
                sb.Append($"\nService:\t{contact.Activity} - {contact.ServiceCode.Description}");
                sb.Append($"\nProgram:\t{contact.ProgramLkp.Name}");
                sb.Append($"\nProvider:\t{contact.StaffEmployee.LastName}, {contact.StaffEmployee.FirstName}");
                sb.Append($"\nLocation:\t{contact.LocationLkp.Description}");
                sb.Append($"\nEntered by:\t{contact.EntryStaffEmployee.LastName}, {contact.EntryStaffEmployee.FirstName}");
                sb.Append($"\nEntered on:\t{contact.CreateDate}");
                sb.Append($"\nSigned on:\t{contact.SignedByDate}");
                sb.Append($"\nSigned by:\t{contact.SignedByStaffEmployee.LastName}, {contact.SignedByStaffEmployee.FirstName}");
                sb.Append($"\nTotal duration:\t{contact.TimeSpent}");
                sb.Append($"\nFace to face duration:\t{contact.FaceToFace}");
                sb.Append($"\nOther consumer contact duration:\t{contact.OtherContactType}");
                sb.Append($"\nCollateral duration:\t{contact.Collateral}");
                sb.Append($"\nRecord keeping duration:\t{contact.RecordKeeping}");
                sb.Append($"\nCoordinating support duration:\t{contact.Support}");
                sb.Append($"\nTravel time:\t{contact.TravelTime}");
                sb.Append($"\nMileage:\t{contact.Mileage}");

                paragraph = doc.LastSection
                                          .AddParagraph(sb.ToString(), "ServiceData");

                paragraph = doc.LastSection.AddParagraph();
                paragraph.Style = "ServiceText";
                paragraph.AddText(contact.ProgressNotes.ProgressNote.Replace("\r", "\n"));
            }
        }

        private void DefineStyles(Document doc)
        {
            // Get the predefined style Normal.
            Style style = doc.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";
            style.Font.Color = Colors.Black;
            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = doc.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 0;
            style.ParagraphFormat.PageBreakBefore = true;

            style = doc.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = doc.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = doc.Styles[StyleNames.Header];
            style.Font.Size = 12;
            style.Font.Bold = true;
            //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = doc.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = doc.Styles.AddStyle("ServiceText", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;

            style = doc.Styles.AddStyle("ServiceData", "Normal");
            style.ParagraphFormat.Borders.Style = BorderStyle.Single;
            style.ParagraphFormat.Borders.Width = .5;
            style.ParagraphFormat.Borders.Distance = 3;
            //style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
            //style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 12;
            style.ParagraphFormat.AddTabStop("5cm", TabAlignment.Left);
        }

        private void DefineContentSection(Document doc, Client client)
        {
            Section section = doc.AddSection();
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.PageFormat = PageFormat.Letter;

            HeaderFooter header = section.Headers.Primary;
            header.AddParagraph($"Client ID: {client.ClientId}\n{client.LastName}, {client.FirstName} {client.MiddleInitial}");

            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
        }
    }
}
