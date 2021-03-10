using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Domain;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace PdfLib
{
    public class PDFDocument
    {
        public PDFDocument() { }

        public void GeneratePdf(Client client, Mode mode)
        {
            switch (mode)
            {
                case Mode.Single:
                    RenderSingle(client);
                    break;
                case Mode.Daily:
                    RenderDaily(client);
                    break;
                case Mode.Monthly:
                    RenderMonthly(client);
                    break;
                case Mode.Yearly:
                    RenderYearly(client);
                    break;
            }
        }

        private void RenderSingle(Client client)
        {
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
                string path = $"C:\\testpdfs\\{client.ClientId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = $"{path}\\{contact.ServDate.Value.ToString("yyyyMMdd-HHmmss")}-{contact.ServiceCode.Code}-{contact.KeyId}.pdf";
                renderer.PdfDocument.Save(filename);
            }
        }

        private void RenderDaily(Client client)
        {
            IEnumerable<IGrouping<DateTime, Contacts>> contacts = client.Contacts.GroupBy(c => c.ServDate.Value.Date);

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
                string path = $"C:\\testpdfs\\{client.ClientId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = $"{path}\\{group.Key.ToString("yyyyMMdd-HHmmss")}.pdf";
                renderer.PdfDocument.Save(filename);
            }
        }

        private void RenderMonthly(Client client)
        {
            var contacts = client.Contacts.GroupBy(c => new DateTime(c.ServDate.Value.Year,c.ServDate.Value.Month,01));

            foreach (var group in contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string path = $"C:\\testpdfs\\{client.ClientId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = $"{path}\\{group.Key.ToString("yyyyMM")}.pdf";
                renderer.PdfDocument.Save(filename);
            }
        }

        private void RenderYearly(Client client)
        {
            var contacts = client.Contacts.GroupBy(c => new DateTime(c.ServDate.Value.Year, 01, 01));

            foreach (var group in contacts)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Document doc = new Document();
                DefineStyles(doc);
                DefineContentSection(doc, client);
                ProcessServiceInfo(doc, group.ToList());
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                string path = $"C:\\testpdfs\\{client.ClientId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = $"{path}\\{group.Key.ToString("yyyy")}.pdf";
                renderer.PdfDocument.Save(filename);
            }
        }

        private void ProcessServiceInfo(Document doc, List<Contacts> contacts)
        {
            foreach(Contacts contact in contacts)
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
                paragraph.AddText(contact.ProgressNotes.ProgressNote.Replace("\r", "\n"););
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

        public enum Mode
        {
            Single,
            Daily,
            Monthly,
            Yearly
        }

        //private void DefineParagraphs(Document document)
        //{
        //    Paragraph paragraph = document.LastSection.AddParagraph("Paragraph Layout Overview", "Heading1");
        //    BookmarkField bookmarkField = paragraph.AddBookmark("Paragraphs");
        //    DemonstrateAlignment(document);
        //    DemonstrateIndent(document);
        //    DemonstrateFormattedText(document);
        //    DemonstrateBordersAndShading(document);
        //}

        //private void DemonstrateAlignment(Document document)
        //{
        //    document.LastSection.AddParagraph("Alignment", "Heading2");

        //    document.LastSection.AddParagraph("Left Aligned", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Left;
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("Right Aligned", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Right;
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("Centered", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Center;
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("Justified", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Justify;
        //    paragraph.AddText(FillerText.MediumText);
        //}

        //private void DemonstrateIndent(Document document)
        //{
        //    document.LastSection.AddParagraph("Indent", "Heading2");

        //    document.LastSection.AddParagraph("Left Indent", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.LeftIndent = "2cm";
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("Right Indent", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.RightIndent = "1in";
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("First Line Indent", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.FirstLineIndent = "12mm";
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("First Line Negative Indent", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.LeftIndent = "1.5cm";
        //    paragraph.Format.FirstLineIndent = "-1.5cm";
        //    paragraph.AddText(FillerText.Text);
        //}

        //private void DemonstrateFormattedText(Document document)
        //{
        //    document.LastSection.AddParagraph("Formatted Text", "Heading2");

        //    //document.LastSection.AddParagraph("Left Aligned", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.AddText("Text can be formatted ");
        //    paragraph.AddFormattedText("bold", TextFormat.Bold);
        //    paragraph.AddText(", ");
        //    paragraph.AddFormattedText("italic", TextFormat.Italic);
        //    paragraph.AddText(", or ");
        //    paragraph.AddFormattedText("bold & italic", TextFormat.Bold | TextFormat.Italic);
        //    paragraph.AddText(".");
        //    paragraph.AddLineBreak();
        //    paragraph.AddText("You can set the ");
        //    FormattedText formattedText = paragraph.AddFormattedText("size ");
        //    formattedText.Size = 15;
        //    paragraph.AddText("the ");
        //    formattedText = paragraph.AddFormattedText("color ");
        //    formattedText.Color = Colors.Firebrick;
        //    paragraph.AddText("the ");
        //    formattedText = paragraph.AddFormattedText("font", new Font("Verdana"));
        //    paragraph.AddText(".");
        //    paragraph.AddLineBreak();
        //    paragraph.AddText("You can set the ");
        //    formattedText = paragraph.AddFormattedText("subscript");
        //    formattedText.Subscript = true;
        //    paragraph.AddText(" or ");
        //    formattedText = paragraph.AddFormattedText("superscript");
        //    formattedText.Superscript = true;
        //    paragraph.AddText(".");
        //}

        //private void DemonstrateBordersAndShading(Document document)
        //{
        //    document.LastSection.AddPageBreak();
        //    document.LastSection.AddParagraph("Borders and Shading", "Heading2");

        //    document.LastSection.AddParagraph("Border around Paragraph", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Borders.Width = 2.5;
        //    paragraph.Format.Borders.Color = Colors.Navy;
        //    paragraph.Format.Borders.Distance = 3;
        //    paragraph.AddText(FillerText.MediumText);

        //    document.LastSection.AddParagraph("Shading", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Shading.Color = Colors.LightCoral;
        //    paragraph.AddText(FillerText.Text);

        //    document.LastSection.AddParagraph("Borders & Shading", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Style = "TextBox";
        //    paragraph.AddText(FillerText.MediumText);
        //}

        //private void DefineTables(Document document)
        //{
        //    Paragraph paragraph = document.LastSection.AddParagraph("Table Overview", "Heading1");
        //    paragraph.AddBookmark("Tables");

        //    DemonstrateSimpleTable(document);
        //    DemonstrateTableAlignment(document);
        //    DemonstrateCellMerge(document);
        //}

        //private void DemonstrateSimpleTable(Document document)
        //{
        //    document.LastSection.AddParagraph("Simple Tables", "Heading2");

        //    Table table = new Table();
        //    table.Borders.Width = 0.75;

        //    Column column = table.AddColumn(Unit.FromCentimeter(2));
        //    column.Format.Alignment = ParagraphAlignment.Center;

        //    table.AddColumn(Unit.FromCentimeter(5));

        //    Row row = table.AddRow();
        //    row.Shading.Color = Colors.PaleGoldenrod;
        //    Cell cell = row.Cells[0];
        //    cell.AddParagraph("Itemus");
        //    cell = row.Cells[1];
        //    cell.AddParagraph("Descriptum");

        //    row = table.AddRow();
        //    cell = row.Cells[0];
        //    cell.AddParagraph("1");
        //    cell = row.Cells[1];
        //    cell.AddParagraph(FillerText.ShortText);

        //    row = table.AddRow();
        //    cell = row.Cells[0];
        //    cell.AddParagraph("2");
        //    cell = row.Cells[1];
        //    cell.AddParagraph(FillerText.Text);

        //    table.SetEdge(0, 0, 2, 3, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

        //    document.LastSection.Add(table);
        //}

        //private void DemonstrateTableAlignment(Document document)
        //{
        //    document.LastSection.AddParagraph("Cell Alignment", "Heading2");

        //    Table table = document.LastSection.AddTable();
        //    table.Borders.Visible = true;
        //    table.Format.Shading.Color = Colors.LavenderBlush;
        //    table.Shading.Color = Colors.Salmon;
        //    table.TopPadding = 5;
        //    table.BottomPadding = 5;

        //    Column column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Left;

        //    column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Center;

        //    column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Right;

        //    table.Rows.Height = 35;

        //    Row row = table.AddRow();
        //    row.VerticalAlignment = VerticalAlignment.Top;
        //    row.Cells[0].AddParagraph("Text");
        //    row.Cells[1].AddParagraph("Text");
        //    row.Cells[2].AddParagraph("Text");

        //    row = table.AddRow();
        //    row.VerticalAlignment = VerticalAlignment.Center;
        //    row.Cells[0].AddParagraph("Text");
        //    row.Cells[1].AddParagraph("Text");
        //    row.Cells[2].AddParagraph("Text");

        //    row = table.AddRow();
        //    row.VerticalAlignment = VerticalAlignment.Bottom;
        //    row.Cells[0].AddParagraph("Text");
        //    row.Cells[1].AddParagraph("Text");
        //    row.Cells[2].AddParagraph("Text");
        //}

        //private void DemonstrateCellMerge(Document document)
        //{
        //    document.LastSection.AddParagraph("Cell Merge", "Heading2");

        //    Table table = document.LastSection.AddTable();
        //    table.Borders.Visible = true;
        //    table.TopPadding = 5;
        //    table.BottomPadding = 5;

        //    Column column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Left;

        //    column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Center;

        //    column = table.AddColumn();
        //    column.Format.Alignment = ParagraphAlignment.Right;

        //    table.Rows.Height = 35;

        //    Row row = table.AddRow();
        //    row.Cells[0].AddParagraph("Merge Right");
        //    row.Cells[0].MergeRight = 1;

        //    row = table.AddRow();
        //    row.VerticalAlignment = VerticalAlignment.Bottom;
        //    row.Cells[0].MergeDown = 1;
        //    row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
        //    row.Cells[0].AddParagraph("Merge Down");

        //    table.AddRow();
        //}

        //private void DefineCharts(Document document)
        //{
        //    Paragraph paragraph = document.LastSection.AddParagraph("Chart Overview", "Heading1");
        //    paragraph.AddBookmark("Charts");

        //    document.LastSection.AddParagraph("Sample Chart", "Heading2");

        //    Chart chart = new Chart();
        //    chart.Left = 0;

        //    chart.Width = Unit.FromCentimeter(16);
        //    chart.Height = Unit.FromCentimeter(12);
        //    Series series = chart.SeriesCollection.AddSeries();
        //    series.ChartType = ChartType.Column2D;
        //    series.Add(new double[] { 1, 17, 45, 5, 3, 20, 11, 23, 8, 19 });
        //    series.HasDataLabel = true;

        //    series = chart.SeriesCollection.AddSeries();
        //    series.ChartType = ChartType.Line;
        //    series.Add(new double[] { 41, 7, 5, 45, 13, 10, 21, 13, 18, 9 });

        //    XSeries xseries = chart.XValues.AddXSeries();
        //    xseries.Add("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N");

        //    chart.XAxis.MajorTickMark = TickMarkType.Outside;
        //    chart.XAxis.Title.Caption = "X-Axis";

        //    chart.YAxis.MajorTickMark = TickMarkType.Outside;
        //    chart.YAxis.HasMajorGridlines = true;

        //    chart.PlotArea.LineFormat.Color = Colors.DarkGray;
        //    chart.PlotArea.LineFormat.Width = 1;

        //    document.LastSection.Add(chart);
        //}        
    }

    //public class FillerText
    //{
    //    public static string ShortText
    //    {
    //        get { return "Andigna cons nonsectem accummo diamet nis diat."; }
    //    }

    //    public static string Text
    //    {
    //        get
    //        {
    //            return "Loboreet autpat, quis adigna conse dipit la consed exeril et utpatetuer autat, voloboreet, consequamet ilit nos aut in henit ullam, sim doloreratis dolobore tat, venim quissequat. " +
    //                "Nisci tat laor ametumsan vulla feuisim ing eliquisi tatum autat, velenisit iustionsed tis dunt exerostrud dolore verae.";
    //        }
    //    }

    //    public static string MediumText
    //    {
    //        get
    //        {
    //            return "Incinibh ecte dionsent am, sisl ute magna faccum ing eu feugait ulla consequismod tetum zzrilluptat. Ut velis accum dit la corper inci essequat, venis nisl dolutat. Sandipit esequisit autpat. " +
    //                "Magnibh et laortie feugiamcommy nulluptat dolorpero euipis nonum augait wis dit, quamcon sequipit at vel il eui blaorper si tat ipit at nis nullan hent num dunt irit, sum dolendio consendigna consent " +
    //                "lan ut illan etue miniam dolenisis nonsenim inim quat, conulla orercinisim vel inci ent illam quat volore veliquam amconsequat. Ut lut incincipit nullaor iriurercip et luptat erat illamco mmoluptat.\n" +
    //                "Ut iriusciduis nonsed do el dolut ea autem il dolore verci blam, quatue el ute facilis cidunt dit alisl ut lut num vercinc illaore del ilisi blandre commodit, quamcon sequipsusto dunt ver illaorperit utpat, " +
    //                "velisci lisciniam vent alis nostisi et, quisit, con eu facipit vulputpat.";
    //        }
    //    }

    //    public static string LargeText
    //    {
    //        get
    //        {
    //            return "Enim vulput ea am, conulput wisi endio ex ent ut velit nosting eugait nonullut nonse modolorperat vulla acipsuscil ut augue tet verilis modiate commodo lesequis eugue esto eugiam in esto corperosto " +
    //                "dipiscipis dunt acil do dolutpatummy nos eugiam nonum ea alisit delit, vendio od tatumsan henim nullamconse minci tem delenit iusto eummolore magna consent ea am aliquis adigna con er sent ad mincidunt num vulla " +
    //                "autat la alit alit am, volutate dolortin ullut alit wis adit verostrud tisi.\n" +
    //                "Ad estionulla feu faci tinit atie modipsuscip essi.\n" +
    //                "Suscinibh el ex euguer autem iure dolum doluptat laoreros am velenia mconulla corem nos ea facilisl et eugiat ecte minisit wis er in utat ip exeratie mincidunt wiscilis nisci essecte eriliquip et illutem duisim " +
    //                "velestrud tat ilisisis ese molesenim vercil el ute magniam augue min erosto con volorper adignim dolorer ostrud exerostio odo doloreet ex et utat. Ud tat. Andipsum erosto odolum dolesequat vent augiamcon ullam " +
    //                "euipis duis adignis autem nullamet, velisi. Tet doloreet venibh ex et ut nullamet adipisis accum ipis acing exer sed et, vero odoloreet, veros atie dolore dolorem quat, se velit velit wismodit er sum iure " +
    //                "te facidunt doloreraesto do dolorem nos nos erat at nit in utpat adigna feugiamet aliquissi blaore dui blan utat vero dolessim zzriustrud digniat, volobor secte core feuis dolore vero odip et ad mod te conulla " +
    //                "feugait volor sum iusciduismod dunt vendigna ad del ut dunt alit augue eliquam ip el ipit in veliqui ssenisci te tis ercing";
    //        }
    //    }
    //}
}
