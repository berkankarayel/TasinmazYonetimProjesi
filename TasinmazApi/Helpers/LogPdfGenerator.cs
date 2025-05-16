using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Helpers
{
    public class LogPdfGenerator
    {
        public static byte[] Generate(List<Log> logs)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Helvetica"));

                    // Başlık
                    page.Header().Element(header =>
                    {
                        header
                            .PaddingBottom(15)
                            .Text("📋 Log Kayıtları")
                            .SemiBold().FontSize(20).FontColor(Colors.Grey.Darken3);
                    });

                    // İçerik
                    page.Content().Element(content =>
                    {
                        content.Table(table =>
                        {
                            // Sütun yapısı
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(90);   // İşlem Tipi
                                columns.ConstantColumn(70);   // Durum
                                columns.RelativeColumn(3);    // Açıklama
                                columns.ConstantColumn(110);  // Tarih
                                columns.ConstantColumn(70);   // IP
                            });

                            // Başlık
                            table.Header(header =>
                            {
                                header.Cell().Element(StyleHeaderCell).Text("İşlem Tipi");
                                header.Cell().Element(StyleHeaderCell).Text("Durum");
                                header.Cell().Element(StyleHeaderCell).Text("Açıklama");
                                header.Cell().Element(StyleHeaderCell).Text("Tarih");
                                header.Cell().Element(StyleHeaderCell).Text("IP");
                            });

                            // Satırlar
                            for (int i = 0; i < logs.Count; i++)
                            {
                                var log = logs[i];
                                var bgColor = i % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;

                                table.Cell().Element(c => StyleCell(c, bgColor)).Text(log.IslemTipi);
                                table.Cell().Element(c => StyleCell(c, bgColor)).Text(log.Durum);
                                table.Cell().Element(c => StyleCell(c, bgColor)).Text(log.Aciklama);
                                table.Cell().Element(c => StyleCell(c, bgColor)).Text(log.TarihSaat.ToString("dd.MM.yyyy HH:mm"));
                                table.Cell().Element(c => StyleCell(c, bgColor)).Text(log.KullaniciIp ?? "-");
                            }
                        });
                    });

                    // Footer
                    page.Footer().AlignCenter().Column(column =>
                    {
                        column.Item().Text("Taşınmaz Yönetim Sistemi").SemiBold().FontSize(10);
                        column.Item().Text(DateTime.Now.ToString("dd.MM.yyyy HH:mm"))
                            .FontColor(Colors.Grey.Darken2).FontSize(9);
                    });

                });
            });

            return document.GeneratePdf();
        }

        private static IContainer StyleHeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken2)
                .PaddingVertical(6).PaddingHorizontal(5)
                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                .AlignCenter()
                .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White));
        }

        private static IContainer StyleCell(IContainer container, string backgroundColor)
        {
            return container
                .Background(backgroundColor)
                .PaddingVertical(6).PaddingHorizontal(5)
                .BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                .AlignLeft();
        }
    }
}
