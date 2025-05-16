using System.Text.Json.Serialization;

namespace TasinmazApi.Entities.Concrete
{
    public class Tasinmaz
    {
        public int TasinmazId { get; set; }

        public int KullaniciId { get; set; }

        [JsonIgnore]
        public Kullanici Kullanici { get; set; }

        public int IlId { get; set; }
        public int IlceId { get; set; }
        public int MahalleId { get; set; }

        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; } 
        public string Koordinat { get; set; }
      
        public Mahalle Mahalle { get; set; }
    }
}
