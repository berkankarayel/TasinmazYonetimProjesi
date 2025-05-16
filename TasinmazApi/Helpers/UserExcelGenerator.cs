using ClosedXML.Excel;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class UserExcelGenerator
    {
        public static byte[] Generate(List<Kullanici> users)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Kullanıcılar");

            worksheet.Cell(1, 1).Value = "Ad Soyad";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Rol";
            worksheet.Cell(1, 4).Value = "Adres";

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                worksheet.Cell(i + 2, 1).Value = $"{user.KullaniciAdi} {user.KullaniciSoyadi}";
                worksheet.Cell(i + 2, 2).Value = user.Email;
                worksheet.Cell(i + 2, 3).Value = user.Rol.ToString();
                worksheet.Cell(i + 2, 4).Value = user.Adres;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
