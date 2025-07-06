using Interface_Tier.DTOs.User_DTOs;

namespace Presentation_Tier.RequestDTOs
{
    public class UpdateUserServiceRequestDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public IFormFile? image { get; set; }
    }
}
