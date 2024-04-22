// using Netbarcode;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Graphics = System.Drawing.Graphics;

public class Printer
{
    string gameTime;
    string gameDate;
    string serviceOperatorRate;
    string totalSelectedSpots;
    string poolPrize;
    string totalAmount;
    string gId;
    string barcodeNo;
    string serviseCharge;
    Dictionary<string, BetInfo> bets;


    public Printer(string gameTime, string gameDate,
        string totalSelectedSpots,
        string barcodeNo, Dictionary<string, BetInfo> bets,float poolPrize,float serviseCharge)
    {
        this.gameTime = gameTime;
        this.gameDate = gameDate;
        this.totalSelectedSpots = bets.Count.ToString();
        this.totalAmount = bets.Values.Sum(x=>x.amount).ToString();
        this.barcodeNo = barcodeNo;
        this.poolPrize = poolPrize.ToString();
        this.bets = bets;
        this.serviseCharge = serviseCharge.ToString();
    }

    public void Print()
    {
        var doc = new PrintDocument();
        doc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
        doc.PrintPage += new PrintPageEventHandler(ProvideContent);
        doc.Print();
        //// generate a file name as the current date/time in unix timestamp format
        //string file = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();

        //// the directory to store the output.
        //string directory = Application.dataPath;

        //// initialize PrintDocument object

        //doc.PrinterSettings = new PrinterSettings()
        //{
        //    // set the printer to 'Microsoft Print to PDF'
        //    PrinterName = "Microsoft Print to PDF",

        //    // tell the object this document will print to file
        //    PrintToFile = true,

        //    // set the filename to whatever you like (full path)
        //    PrintFileName = Path.Combine(directory, file + ".pdf"),
        //};

        //doc.Print();
    }

    void ProvideContent(object sender, PrintPageEventArgs e)
    {
        Graphics graphics = e.Graphics;
        Font font = new Font("Courier New", 10);

        float fontHeight = font.GetHeight();
        Image image;
        int startX = 0;
        int startY = 0;
        int Offset = 20;


        graphics.DrawString("Game Acknowledgement", new Font("Courier New", 15),
                            new SolidBrush(Color.Black), startX+Offset/4, startY + Offset);
        Offset = Offset + 20;

        string data = $"Date: {gameDate}";
        graphics.DrawString(data,
                    new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);
        Offset = Offset + 20;


        string time = $"Time: {gameTime}";
        graphics.DrawString(time,
                    new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);
        
        
        Offset = Offset + 20;


        string poolP = $"Pool Prize: {poolPrize}";
        graphics.DrawString(poolP,
                    new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);
        
        Offset = Offset + 20;


        string ServiseCharge = $"Service Charge : {serviseCharge}";
        graphics.DrawString(ServiseCharge,
                    new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);


        string underLine = "-------------------Bets-------------------";

        Offset = Offset + 30;
        graphics.DrawString(underLine, new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);

        Offset = Offset + 30;

        string totalbets = string.Empty;
        int index = 0;
        foreach (var item in bets)
        {
            totalbets += $" {item.Key}: {item.Value.amount}, ";
            index++;
            if (index % 4 != 0) continue;
            Offset = Offset + 20; 
            graphics.DrawString(totalbets, new Font("Courier New", 14),
                         new SolidBrush(Color.Black), startX, startY + Offset);
            totalbets = string.Empty;
        }
            Offset = Offset + 20; 
        graphics.DrawString(totalbets, new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);

        Offset = Offset + 30;
        underLine = "------------------------------------------";
        graphics.DrawString(underLine, new Font("Courier New", 14),
                    new SolidBrush(Color.Black), startX, startY + Offset);
        Offset = Offset + 20;
        string spots = $"Total Selected Spots : {totalSelectedSpots}";
        graphics.DrawString(spots, new Font("Courier New", 14),
                  new SolidBrush(Color.Black), startX, startY + Offset);

        Offset = Offset + 20;
        string amount = $"Total Amount ₹ {totalAmount}";
        graphics.DrawString(amount, new Font("Courier New", 14),
                  new SolidBrush(Color.Black), startX, startY + Offset);

        Offset = Offset + 30;
        var barcode = new Barcode(barcodeNo);
        graphics.DrawImage(barcode.GetImage(), startX + Offset / 4, startY + Offset);
       
        Offset = Offset + 100;
        string serialNo = barcodeNo;
        graphics.DrawString(serialNo, new Font("Courier New", 14),
                   new SolidBrush(Color.Black), startX+Offset/5, startY + Offset);
        // SaveImage(graphics);

    }
    void SaveImage(Graphics graphics)
    {
        int x = (int)graphics.DpiX;
        int y = (int)graphics.DpiX;
        Bitmap bmpPicture = new Bitmap(x, y);

        graphics.DrawImage(bmpPicture, x, y);
        Bitmap bitmap = new Bitmap((int)graphics.DpiX, (int)graphics.DpiY, graphics);
        Graphics g = Graphics.FromImage(bitmap);
        g.DrawImage(bitmap, 0, 0);

        g.Clear(Color.Green);

    }
    Bitmap CreatBarCode(string data)
    {
        Bitmap barCode = new Bitmap(1, 1);
        Font threeOfNine = new Font("Free 3 of 9", 60, System.Drawing.FontStyle.Regular,
            System.Drawing.GraphicsUnit.Point);
        Graphics graphics = Graphics.FromImage(barCode);
        SizeF dataSize = graphics.MeasureString(data, threeOfNine);
        barCode = new Bitmap(barCode, dataSize.ToSize());
        graphics = Graphics.FromImage(barCode);
        graphics.Clear(Color.White);
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        graphics.DrawString(data, threeOfNine,
            new SolidBrush(Color.Black), 0, 0);
        graphics.Flush();
        threeOfNine.Dispose();
        graphics.Dispose();
        return barCode;
    }



}
public class PrintText
{
    public PrintText(string text, Font font) : this(text, font, new StringFormat()) { }

    public PrintText(string text, Font font, StringFormat stringFormat)
    {
        Text = text;
        Font = font;
        StringFormat = stringFormat;
    }

    public string Text { get; set; }

    public Font Font { get; set; }

    /// <summary> Default is horizontal string formatting </summary>
    public StringFormat StringFormat { get; set; }
}