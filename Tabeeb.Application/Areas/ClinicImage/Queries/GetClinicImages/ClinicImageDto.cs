using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.ClinicImage.Queries.GetClinicImages
{
    public class ClinicImageDto
    {
        public string ImagePath { get; set; } = null!;

        public int ClinicId { get; set; }


        public ImageType ImageType { get; set; }
    }
}