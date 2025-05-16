namespace TasinmazApi.Entities.Concrete
{
    public class Ilce
    {
        public int IlceId { get; set; }
        public string IlceAdi { get; set; }

        public int IlId { get; set; }  
        public Il Il { get; set; }
    }
}

