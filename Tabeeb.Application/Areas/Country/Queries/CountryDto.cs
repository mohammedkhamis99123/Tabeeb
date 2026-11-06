using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Areas.Country.Queries
{
    public  class CountryDto
    {
        public int Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string ISOCode { get; set; } = null!;

        public int CitiesCount {  get; set; }


    }
}
