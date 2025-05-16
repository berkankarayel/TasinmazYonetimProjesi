using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class UserPdfGenerator
    {
        public static byte[] Generate(List<Kullanici> users)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.Header().Text("Kullanıcı Listesi").SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Ad Soyad
                            columns.RelativeColumn(3); // Email
                            columns.RelativeColumn(2); // Rol
                            columns.RelativeColumn(3); // Adres
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Ad Soyad").Bold();
                            header.Cell().Text("Email").Bold();
                            header.Cell().Text("Rol").Bold();
                            header.Cell().Text("Adres").Bold();
                        });

                        foreach (var user in users)
                        {
                            table.Cell().PaddingVertical(5).Text($"{user.KullaniciAdi} {user.KullaniciSoyadi}");
                            table.Cell().PaddingVertical(5).Text(user.Email);
                            table.Cell().PaddingVertical(5).Text(user.Rol.ToString());
                            table.Cell().PaddingVertical(5).Text(user.Adres);
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
