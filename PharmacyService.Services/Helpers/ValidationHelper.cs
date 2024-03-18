using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Services.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }
        }

        public static void ValidateDto(object dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "DTO object cannot be null.");
            }
            // Additional validation logic for DTOs if needed
        }
    }
}
