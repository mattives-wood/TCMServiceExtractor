using Data;

using Domain.Financials;
using Domain.Meta;

using Microsoft.IdentityModel.Tokens;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfLib;

public class StatementPdfDocument
{
    private readonly string _path;
    private readonly MetadataContext _metadataContext;

    public StatementPdfDocument(string path, MetadataContext metadataContext)
    {
        _metadataContext = metadataContext;
        _path = path;
    }

    public void Render(Client client, List<Statement> statements)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Document doc = new Document();
        DefineStyles(doc);
        DefineContentSection(doc, client);
        if (!statements.IsNullOrEmpty())
        {
            Process(doc, statements);
        }
        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
        renderer.Document = doc;
        renderer.RenderDocument();
        Guid fileGuid = Guid.NewGuid();
        renderer.PdfDocument.Save($"{_path}\\{fileGuid.ToString()}.pdf");

        Metadata metadata = new Metadata()
                            {
                                ClientId = client.ClientId,
                                FirstName = client.FirstName,
                                LastName = client.LastName,
                                DocumentType = "Statement",
                                FileNameGuid = fileGuid
                            };

        _metadataContext.Add(metadata);
        _metadataContext.SaveChanges();
    }

    private void Process(Document doc, Statement statement)
    {
        Paragraph paragraph = doc.LastSection.AddParagraph("General Medical Info", "Heading1");

        StringBuilder sb = new StringBuilder();
        sb.Append($"Physician:\t{medInfo.Physician}");
        sb.Append($"\nPharmacy:\t{medInfo.Pharmacy?.Name}");
        sb.Append($"\nAllergies:\t{medInfo.Allergies}");
        sb.Append($"\nMed Orders:\t{medInfo.OtherMedOrdersText?.Replace("\r", "\n\t")}");
        sb.Append($"\nMed Alerts:\t{medInfo.MedAlerts?.Replace("\r", "\n\t")}");

        paragraph = doc.LastSection.AddParagraph(sb.ToString(), "MedInfo");
    }

    //private void ProcessMedList(Document doc, List<Medication> meds)
    //{
    //    foreach (Medication med in meds.OrderBy(m => m.StartDate))
    //    {
    //        StringBuilder bookmark = new StringBuilder();
    //        if (med.StartDate.HasValue)
    //        {
    //            bookmark.Append($"{med.StartDate.Value.ToShortDateString()} - ");
    //        }
    //        bookmark.Append(med.Drug.Combined);
    //        Paragraph paragraph = doc.LastSection.AddParagraph(bookmark.ToString(), "Heading1");

    //        StringBuilder sb = new StringBuilder();

    //        sb.Append($"Effective Start Date:\t{med.StartDate?.ToShortDateString()}");
    //        sb.Append($"\nEffective End Date:\t{med.EndDate?.ToShortDateString()}");
    //        sb.Append($"\nOrdered Date:\t{med.OrderedDate?.ToShortDateString()}");

    //        sb.Append($"\nDrug:\t{med.Drug.Combined}");
    //        sb.Append($"\nDosage:\t{med.MedDosage}");
    //        sb.Append($"\nFrequency:\t{med.Frequency}");
    //        sb.Append($"\nTotal:\t{med.Tablets}");
    //        sb.Append($"\nRefills:\t{med.Refills}");
    //        sb.Append($"\nApplication:\t{med.MedApplication?.Description}");
    //        sb.Append($"\nExplanation:\t{med.MedExplanation?.Description}");
    //        sb.Append($"\nPhysician:\t{med.Physician}");
    //        sb.Append($"\nComments:\t{med.Comment?.Replace("\r", "\n\t")}");
    //        sb.Append($"\nConsent Date:\t{med.ConsentDate?.ToShortDateString()}");

    //        paragraph = doc.LastSection.AddParagraph(sb.ToString(), "MedHistory");
    //    }
    //}

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
        style.ParagraphFormat.PageBreakBefore = false;

        style = doc.Styles[StyleNames.Header];
        style.Font.Size = 12;
        style.Font.Bold = true;

        style = doc.Styles[StyleNames.Footer];
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        style = doc.Styles.AddStyle("MedHistory", "Normal");
        style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        style.ParagraphFormat.SpaceAfter = 12;
        style.ParagraphFormat.AddTabStop("5cm", TabAlignment.Left);
        Border border = new Border();
        border.Width = .5;
        border.Style = BorderStyle.Single;
        style.ParagraphFormat.Borders.Bottom = border;
        style.ParagraphFormat.KeepTogether = true;

        style = doc.Styles.AddStyle("MedInfo", "Normal");
        style.ParagraphFormat.Borders.Style = BorderStyle.Single;
        style.ParagraphFormat.Borders.Width = .5;
        style.ParagraphFormat.Borders.Distance = 3;
        style.ParagraphFormat.SpaceAfter = 12;
        style.ParagraphFormat.AddTabStop("5cm", TabAlignment.Left);

        // style = doc.Styles.AddStyle("GroupHeader", "Normal");
        // style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        // style.Font.Size = 12;
        // style.ParagraphFormat.SpaceAfter = 4;

        // style = doc.Styles.AddStyle("GroupText", "Normal");
        // style.ParagraphFormat.Borders.Style = BorderStyle.Single;
        // style.ParagraphFormat.Borders.Width = .5;
        // style.ParagraphFormat.Borders.Distance = 3;
        // style.ParagraphFormat.SpaceAfter = 12;
        // style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
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