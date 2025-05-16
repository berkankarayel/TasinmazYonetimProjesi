using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class TasinmazPdfGenerator
    {
        public static byte[] Generate(List<Tasinmaz> list)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(9));
                    page.Header().Text("Taşınmaz Listesi").SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1); // İl
                            columns.RelativeColumn(1); // İlçe
                            columns.RelativeColumn(1); // Mahalle
                            columns.RelativeColumn(1); // Ada
                            columns.RelativeColumn(1); // Parsel
                            columns.RelativeColumn(1); // Nitelik
                            columns.RelativeColumn(2); // Koordinat
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("İl").Bold();
                            header.Cell().Text("İlçe").Bold();
                            header.Cell().Text("Mahalle").Bold();
                            header.Cell().Text("Ada").Bold();
                            header.Cell().Text("Parsel").Bold();
                            header.Cell().Text("Nitelik").Bold();
                            header.Cell().Text("Koordinat").Bold();
                        });

                        foreach (var t in list)
                        {
                            table.Cell().PaddingVertical(4).Text(t.Mahalle?.Ilce?.Il?.IlAdi ?? "-");
                            table.Cell().PaddingVertical(4).Text(t.Mahalle?.Ilce?.IlceAdi ?? "-");
                            table.Cell().PaddingVertical(4).Text(t.Mahalle?.MahalleAdi ?? "-");
                            table.Cell().PaddingVertical(4).Text(t.Ada);
                            table.Cell().PaddingVertical(4).Text(t.Parsel);
                            table.Cell().PaddingVertical(4).Text(t.Nitelik);
                            table.Cell().PaddingVertical(4).Text(t.Koordinat);
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
