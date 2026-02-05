using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Data;

using Domain.Hotline;
using Domain.Meta;
using Domain.Services;

using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PdfLib
{
    public class HotlinePdfDocument
    {
        private readonly string _path;
        private readonly MetadataContext _metadataContext;

        public HotlinePdfDocument(string path, MetadataContext metadataContext)
        {
            _metadataContext = metadataContext;
            _path = path;
        }

        public void RenderMonthly(Client client, List<HotLineHist> calls)
        {
            IEnumerable<IGrouping<DateTime, HotLineHist>> orderedCalls = calls.GroupBy(c => new DateTime(c.CallDateTime.Year, c.CallDateTime.Month, 01));

            foreach (IGrouping<DateTime, HotLineHist> group in orderedCalls)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessHotlineInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                Guid fileGuid = Guid.NewGuid();
                renderer.PdfDocument.Save($"{_path}\\{fileGuid.ToString()}.pdf");

                Metadata metadata = new Metadata()
                {
                    DocumentYear = group.First().CallDateTime.Year,
                    DocumentMonth = group.First().CallDateTime.Month,
                    ClientId = client.ClientId,
                    FirstName = client.FirstName,
                    LastName =  client.LastName,
                    DocumentType = "Hot Line",
                    FileNameGuid = fileGuid
                };

                _metadataContext.Add(metadata);
                _metadataContext.SaveChanges();
            }
        }

        public void RenderYearly(Client client, List<HotLineHist> calls)
        {
            IEnumerable<IGrouping<DateTime, HotLineHist>> orderedCalls = calls.GroupBy(c => new DateTime(c.CallDateTime.Year, 01, 01));

            foreach (IGrouping<DateTime, HotLineHist> group in orderedCalls)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessHotlineInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                Guid fileGuid = Guid.NewGuid();
                renderer.PdfDocument.Save($"{_path}\\{fileGuid.ToString()}.pdf");

                Metadata metadata = new Metadata()
                {
                    DocumentYear = group.First().CallDateTime.Year,
                    ClientId = client.ClientId,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    DocumentType = "Hot Line",
                    FileNameGuid = fileGuid
                };

                _metadataContext.Add(metadata);
                _metadataContext.SaveChanges();
            }
        }

        private void ProcessHotlineInfo(Document doc, List<HotLineHist> calls)
        {
            foreach (HotLineHist call in calls)
            {
                Paragraph paragraph = doc.LastSection
                                        .AddParagraph(
                                            $"{call.CallDateTime}", "Heading1");

                StringBuilder sb = new StringBuilder();
                sb.Append($"\nCall Time:\t{call.CallDateTime}");
                sb.Append($"\nCall Duration:\t{call.CallMinutes}");
                switch (call.CallerTable)
                {
                    case ("Employees"): 
                        sb.Append($"\nCaller:\t{call.StaffCaller.LastName}, {call.StaffCaller.FirstName}");
                        break;
                    case ("Client"):
                        if (call.ClientCaller != null)
                        {
                            sb.Append($"\nCaller:\t{call.ClientCaller.LastName}, {call.ClientCaller.FirstName}");
                        }                        
                        break;
                    case ("EMClient"):
                        sb.Append($"\nCaller:\t{call.EmClientCaller.LastName}, {call.EmClientCaller.FirstName}"); 
                        break;
                    case ("HotlineClient"):
                        sb.Append($"\nCaller:\t{call.HLClientCaller.LastName}, {call.HLClientCaller.FirstName}");
                        break;
                }


                sb.Append($"\nCaller Relationship:\t{call.CallerRelationship?.Description}");

                sb.Append($"\nProgram:\t{call.ProgramLkp.Name}");
                sb.Append($"\nResponsible County:\t{call.RespCounty?.Description}");
                sb.Append($"\nLocation:\t{call.Location.Description}");
                sb.Append($"\nEntered by:\t{call.InsertStaff.LastName}, {call.InsertStaff.FirstName}");
                sb.Append($"\nEntered on:\t{call.InsertDate}");
                sb.Append($"\nCall Type:\t{call.CallType?.Description}");
                
                if (call.SignedByStaffId == 0)
                {
                    sb.Append($"\nSigned on:\tUnsigned");
                    sb.Append($"\nSigned by:\tUnsigned");
                }
                else
                {
                    sb.Append($"\nSigned on:\t{call.SignedDate?.ToShortDateString()}");
                    sb.Append($"\nSigned by:\t{call.SignedByStaff.LastName}, {call.SignedByStaff.FirstName}");
                }
                
                sb.Append($"\nFace to face duration:\t{call.FaceToFace}");
                sb.Append($"\nOther consumer contact duration:\t{call.OtherContactType}");
                sb.Append($"\nCollateral duration:\t{call.Collateral}");
                sb.Append($"\nRecord keeping duration:\t{call.RecordKeeping}");
                sb.Append($"\nCoordinating support duration:\t{call.Support}");
                sb.Append($"\nTravel time:\t{call.Travel}");
                sb.Append($"\nTotal duration:\t{call.DistributedTotalTime}");
                sb.Append($"\nBillable time:\t{call.DistributedBillableTime}");
                sb.Append($"\nUnit type:\t{call.DistributedTimeUnitType}");

                paragraph = doc.LastSection.AddParagraph(sb.ToString(), "ServiceData");

                //if (call.ProgressNoteKey != 0)
                //{
                //    paragraph = doc.LastSection.AddParagraph();
                //    paragraph.Style = "ServiceText";
                //    string text = call.ProgressNotes?.ProgressNote.Replace("\r", "\n");
                //    if (!text.IsNullOrEmpty())
                //    {
                //        paragraph.AddText(text);
                //    }
                //}

                if (call.CommentsTextId != 0)
                {
                    paragraph = doc.LastSection.AddParagraph();
                    paragraph.Style = "GroupHeader";
                    paragraph.AddText("Comments:");

                    paragraph = doc.LastSection.AddParagraph();
                    paragraph.Style = "ServiceText";
                    paragraph.AddText(call.Comment.TextBlob.Replace("\r", "\n"));
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
