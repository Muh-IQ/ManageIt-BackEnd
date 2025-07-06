using Interface_Tier.Utiltiy;

namespace Presentation_Tier.RequestDTOs
{
    public class SetProjectMemberPermissionRequestDTO
    {
        public int ProjectMemberID { get; set; }
        public int ProjectID { get; set; }
        public Permission permission { get; set; }
    }
}
