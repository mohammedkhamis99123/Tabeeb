using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Queries;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Mapping
{
    public  class CountryMapping
    {
        public static void Run()
        {
            TypeAdapterConfig<Country, CountryDto>
                .NewConfig()
                .Map(dest => dest.CitiesCount, src => src.Cities.Count());

        }
    }
}
