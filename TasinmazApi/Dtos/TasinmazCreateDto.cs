// Dtos/TasinmazCreateDto.cs
namespace TasinmazApi.Dtos
{
    public class TasinmazCreateDto
    {
        public int IlId { get; set; }
        public int IlceId { get; set; }
        public int MahalleId { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; }
        public string Koordinat { get; set; }
    }
}

