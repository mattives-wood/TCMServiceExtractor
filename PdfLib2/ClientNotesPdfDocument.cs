using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Data;

using Domain.Meta;
using Domain.Services;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PdfLib
{
    public class ClientNotesPdfDocument
    {
        private readonly string _path;
        private readonly MetadataContext _metadataContext;

        public ClientNotesPdfDocument(string path, MetadataContext metadataContext)
        {
            _metadataContext = metadataContext;
            _path = path;
        }

        public void RenderMonthly(Client client, List<Contacts> contacts)
        {
            IEnumerable<IGrouping<int, Contacts>> programGroupedContacts = contacts.GroupBy(c => c.Program);

            foreach (IGrouping<int, Contacts> programContactGroup in programGroupedContacts)
            {
                IEnumerable<IGrouping<DateTime, Contacts>> orderedContacts = programContactGroup.GroupBy(c => new DateTime(c.ServDate.Value.Year, c.ServDate.Value.Month, 01));

                foreach (IGrouping<DateTime, Contacts> group in orderedContacts)
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Document doc = new Document();
                    DefineStyles(doc);
                    DefineContentSection(doc, client);
                    ProcessServiceInfo(doc, group.ToList());
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = doc;
                    renderer.RenderDocument();
                    Guid fileGuid = Guid.NewGuid();
                    renderer.PdfDocument.Save($"{_path}\\{fileGuid.ToString()}.pdf");

                    Metadata metadata = new Metadata()
                    {
                        DocumentYear = group.First().ServDate.Value.Year,
                        DocumentMonth = group.First().ServDate.Value.Month,
                        ClientId = client.ClientId,
                        FirstName = client.FirstName,
                        LastName =  client.LastName,
                        DocumentType = "Service Notes",
                        Program = group.First().ProgramLkp.Name,
                        FileNameGuid = fileGuid
                    };

                    _metadataContext.Add(metadata);
                    _metadataContext.SaveChanges();
                }
            }
        }

        public void RenderYearly(Client client, List<Contacts> contacts)
        {
            IEnumerable<IGrouping<int, Contacts>> programGroupedContacts = contacts.GroupBy(c => c.Program);

            foreach (IGrouping<int, Contacts> programContactGroup in programGroupedContacts)
            {
                IEnumerable<IGrouping<DateTime, Contacts>> orderedContacts = programContactGroup.GroupBy(c => new DateTime(c.ServDate.Value.Year, 01, 01));

                foreach (IGrouping<DateTime, Contacts> group in orderedContacts)
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Document doc = new Document();
                    DefineStyles(doc);
                    DefineContentSection(doc, client);
                    ProcessServiceInfo(doc, group.ToList());
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = doc;
                    renderer.RenderDocument();
                    Guid fileGuid = Guid.NewGuid();
                    renderer.PdfDocument.Save($"{_path}\\{fileGuid.ToString()}.pdf");

                    Metadata metadata = new Metadata()
                    {
                        DocumentYear = group.First().ServDate.Value.Year,
                        ClientId = client.ClientId,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        DocumentType = "Service Notes",
                        Program = group.First().ProgramLkp.Name,
                        FileNameGuid = fileGuid
                    };
                    _metadataContext.Add(metadata);
                    _metadataContext.SaveChanges();
                }
            }
        }

        private void ProcessServiceInfo(Document doc, List<Contacts> contacts)
        {
            foreach (Contacts contact in contacts)
            {
                StringBuilder header = new StringBuilder();
                header.Append($"{contact.ServDate}");
                if (contact.Activity > 0)
                {
                    header.Append($" | {contact.Activity}");
                    if (contact.ServiceCode != null)
                    {
                        header.Append($"- {contact.ServiceCode.Description}");
                    }
                }
                if (contact.StaffEmployee != null)
                {
                    header.Append($" | {contact.StaffEmployee.LastName}, {contact.StaffEmployee.FirstName}");
                }
                                            
                Paragraph paragraph = doc.LastSection
                                        .AddParagraph(header.ToString(), "Heading1");

                StringBuilder sb = new StringBuilder();
                sb.Append($"Contact ID:\t{contact.KeyId}");
                sb.Append($"\nDate of Service:\t{contact.ServDate}");
                sb.Append($"\nService:\t{contact.Activity} - {contact.ServiceCode?.Description}");
                sb.Append($"\nProgram:\t{contact.ProgramLkp.Name}");
                if (contact.StaffEmployee != null)
                {
                    sb.Append($"\nProvider:\t{contact.StaffEmployee.LastName}, {contact.StaffEmployee.FirstName}");
                }
                else
                {
                    sb.Append($"\nProvider:");
                }
                sb.Append($"\nLocation:\t{contact.LocationLkp?.Description}");
                sb.Append($"\nEntered by:\t{contact.EntryStaffEmployee.LastName}, {contact.EntryStaffEmployee.FirstName}");
                sb.Append($"\nEntered on:\t{contact.CreateDate}");
                if (contact.SignedByStaffId == 0)
                {
                    sb.Append($"\nSigned on:\tUnsigned");
                    sb.Append($"\nSigned by:\tUnsigned");
                }
                else
                {
                    sb.Append($"\nSigned on:\t{contact.SignedByDate}");
                    sb.Append($"\nSigned by:\t{contact.SignedByStaffEmployee.LastName}, {contact.SignedByStaffEmployee.FirstName}");
                }
                sb.Append($"\nTotal duration:\t{contact.TimeSpent}");
                sb.Append($"\nFace to face duration:\t{contact.FaceToFace}");
                sb.Append($"\nOther consumer contact duration:\t{contact.OtherContactType}");
                sb.Append($"\nCollateral duration:\t{contact.Collateral}");
                sb.Append($"\nRecord keeping duration:\t{contact.RecordKeeping}");
                sb.Append($"\nCoordinating support duration:\t{contact.Support}");
                sb.Append($"\nTravel time:\t{contact.TravelTime}");
                sb.Append($"\nMileage:\t{contact.Mileage}");

                paragraph = doc.LastSection.AddParagraph(sb.ToString(), "ServiceData");

                if (contact.ProgressNoteKey != 0)
                {
                    paragraph = doc.LastSection.AddParagraph();
                    paragraph.Style = "ServiceText";
                    if (contact.ProgressNotes != null)
                    {
                        paragraph.AddText(contact.ProgressNotes.ProgressNote.Replace("\r", "\n"));
                    }
                    else
                    {
                        paragraph.AddText("*THIS PERSON HAS NO INDIVIDUAL NOTE FOR THIS SERVICE*");
                    }
                }

                if (contact.GroupNoteKey != 0)
                {
                    paragraph = doc.LastSection.AddParagraph();
                    paragraph.Style = "GroupHeader";
                    paragraph.AddText("Group Note:");

                    paragraph = doc.LastSection.AddParagraph();
                    paragraph.Style = "GroupText";
                    paragraph.AddText(contact.GroupProgressNotes.ProgressNote.Replace("\r", "\n"));
                }
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

            style = doc.Styles[StyleNames.Header];
            style.Font.Size = 12;
            style.Font.Bold = true;

            style = doc.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            style = doc.Styles.AddStyle("ServiceText", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.SpaceAfter = 12;

            style = doc.Styles.AddStyle("ServiceData", "Normal");
            style.ParagraphFormat.Borders.Style = BorderStyle.Single;
            style.ParagraphFormat.Borders.Width = .5;
            style.ParagraphFormat.Borders.Distance = 3;
            style.ParagraphFormat.SpaceAfter = 12;
            style.ParagraphFormat.AddTabStop("5cm", TabAlignment.Left);

            style = doc.Styles.AddStyle("GroupHeader", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.Font.Size = 12;
            style.ParagraphFormat.SpaceAfter = 4;

            style = doc.Styles.AddStyle("GroupText", "Normal");
            style.ParagraphFormat.Borders.Style = BorderStyle.Single;
            style.ParagraphFormat.Borders.Width = .5;
            style.ParagraphFormat.Borders.Distance = 3;
            style.ParagraphFormat.SpaceAfter = 12;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
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
