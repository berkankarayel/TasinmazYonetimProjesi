using ClosedXML.Excel;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class TasinmazExcelGenerator
    {
        public static byte[] Generate(List<Tasinmaz> list)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Taşınmazlar");

            worksheet.Cell(1, 1).Value = "Ada";
            worksheet.Cell(1, 2).Value = "Parsel";
            worksheet.Cell(1, 3).Value = "Nitelik";
            worksheet.Cell(1, 4).Value = "Koordinat";

            for (int i = 0; i < list.Count; i++)
            {
                var t = list[i];
                worksheet.Cell(i + 2, 1).Value = t.Ada;
                worksheet.Cell(i + 2, 2).Value = t.Parsel;
                worksheet.Cell(i + 2, 3).Value = t.Nitelik;
                worksheet.Cell(i + 2, 4).Value = t.Koordinat;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

