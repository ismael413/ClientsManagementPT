using CM.DominioApi.Port.Common;
using System.ComponentModel.DataAnnotations;


namespace CM.DominioApi.Port.Models.Addreses
{
    public class Address
    {
        public int Id { get; set; }

        [Required, MaxLength(200, ErrorMessage = "Full Address Name cannot exceed 200 characters.")]
        public string FullAddress { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Street Name cannot exceed 50 characters.") ]
        public string StreetName { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Building Name cannot exceed 50 characters.")]
        public string BuildingName { get; set; }

        //relaciones
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }


    }
}
