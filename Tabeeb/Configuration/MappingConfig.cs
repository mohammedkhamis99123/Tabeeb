

using Tabeeb.Application.Mapping;

namespace Tabeeb.Configuration
{
    public static class MappingConfig
    {
     
        public static void Run()
        {
            ClinicMapping.Run();
            OfferMapping.Run();

        }


    }
}
