namespace TasinmazApi.Entities.Concrete
{
    public class Mahalle
    {
        public int MahalleId { get; set; }
        public string MahalleAdi { get; set; }

        public int IlceId { get; set; }
        public Ilce Ilce { get; set; }
    }
}
