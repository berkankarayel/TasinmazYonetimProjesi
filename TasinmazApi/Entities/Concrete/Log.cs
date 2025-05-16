using System;

namespace TasinmazApi.Entities.Concrete
{
    public class Log
    {
        public int LogId { get; set; }

        public int? KullaniciId { get; set; } 

        public string IslemTipi { get; set; }
        public string Durum { get; set; }
        public string Aciklama { get; set; }
        public DateTime TarihSaat { get; set; }
        public string KullaniciIp { get; set; }
    }
}

