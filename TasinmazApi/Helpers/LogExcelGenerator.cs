using ClosedXML.Excel;
using System.Data;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class LogExcelGenerator
    {
        public static byte[] Generate(List<Log> logs)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Log Kayıtları");

            worksheet.Cell(1, 1).Value = "İşlem Tipi";
            worksheet.Cell(1, 2).Value = "Durum";
            worksheet.Cell(1, 3).Value = "Açıklama";
            worksheet.Cell(1, 4).Value = "Tarih";
            worksheet.Cell(1, 5).Value = "IP";

            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                worksheet.Cell(i + 2, 1).Value = log.IslemTipi;
                worksheet.Cell(i + 2, 2).Value = log.Durum;
                worksheet.Cell(i + 2, 3).Value = log.Aciklama;
                worksheet.Cell(i + 2, 4).Value = log.TarihSaat.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cell(i + 2, 5).Value = log.KullaniciIp;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

