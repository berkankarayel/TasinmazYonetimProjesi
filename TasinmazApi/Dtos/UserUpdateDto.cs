namespace TasinmazApi.Dtos
{
    public class UserUpdateDto
    {
        public int KullaniciId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KullaniciSoyadi { get; set; }
        public string Email { get; set; }
        public string? Sifre { get; set; }
        public string Rol { get; set; } // "Admin" veya "Kullanici"
        public string Adres { get; set; }
    }
}