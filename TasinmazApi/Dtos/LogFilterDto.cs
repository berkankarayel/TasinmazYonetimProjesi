using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TasinmazApi.Dtos
{
    public class LogFilterDto
    {
        public int? KullaniciId { get; set; }
        public string? IslemTipi { get; set; }
        public string? Durum { get; set; }

        /// <summary>
        /// Başlangıç tarihi (örn: 2025-05-01T00:00:00)
        /// </summary>
        public DateTime? BaslangicTarihi { get; set; }

        /// <summary>
        /// Bitiş tarihi. Tüm gün dahil olsun istiyorsanız saatini 23:59:59 olarak belirtin.
        /// </summary>
        public DateTime? BitisTarihi { get; set; }

        // Gün sonunu otomatik alacak, servis içinde kullanılacak
        [JsonIgnore]
        public DateTime? BitisTarihiGunSonu => BitisTarihi?.Date.AddDays(1).AddSeconds(-1);

    }
}
