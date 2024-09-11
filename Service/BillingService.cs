using CoolCBackEnd.Controllers;
using CoolCBackEnd.Data;
using CoolCBackEnd.Models;
using iText.Html2pdf;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

public class BillingService
{
    public byte[] GeneratePdf(BillingReport report)
    {
        var htmlContent = GetHtmlTemplate(report);

        using (MemoryStream pdfStream = new MemoryStream())
        {
            HtmlConverter.ConvertToPdf(htmlContent, pdfStream);
            return pdfStream.ToArray(); // Return PDF as byte array
        }
    }

    private string GetHtmlTemplate(BillingReport report)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "YourProjectName.Resources.BillingTemplate.html"; // Replace with your project's name

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            var template = reader.ReadToEnd();
            template = template.Replace("{{ OrderId }}", report.OrderId.ToString());
            template = template.Replace("{{ PaymentDate }}", report.PaymentDate.ToString());
            template = template.Replace("{{ PaymentMethod }}", report.PaymentMethod);
            template = template.Replace("{{ TransactionId }}", report.TransactionId);
            template = template.Replace("{{ TotalAmount }}", report.TotalAmount.ToString());
            template = template.Replace("{{ AmountPaid }}", report.AmountPaid.ToString());
            template = template.Replace("{{ PaymentStatus }}", report.PaymentStatus);
            return template;
        }
    }
}

