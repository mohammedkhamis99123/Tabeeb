using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Areas.City.Queries.GetCities
{
    public class CityDto
    {
        public int Id { get; set; }

        public string ArName { get; set; } = null!;


        public string EnName { get; set; } = null!;

        public bool IsActive { get; set; }

        public byte CountryId { get; set; }

        public string CountryArName { get; set; } = null!;


        public string CountryEnName { get; set; } = null!;


    }
}
