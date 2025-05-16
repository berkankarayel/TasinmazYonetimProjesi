using TasinmazApi.Enums;

namespace TasinmazApi.Entities.Concrete
{
    public class Kullanici
    {
        public int KullaniciId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KullaniciSoyadi { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public string Salt { get; set; } 
        public UserRole Rol { get; set; }
        public string Adres { get; set; }

        

        
    }
}

