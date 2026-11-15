using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Queries;
using Tabeeb.Domain.Entities;
using Mapster;
using Tabeeb.Application.Areas.Offer.Queries.GetOffers;

namespace Tabeeb.Application.Mapping
{
    public  class OfferMapping
    {
        public static void Run()
        {
            TypeAdapterConfig<Offer, OfferDto>
                .NewConfig()
                .Map(src => src.DoctorArName, dest => dest.Doctor.ArName)
                .Map(src => src.DoctorEnName, dest => dest.Doctor.EnName);
          





        }
    }
}
