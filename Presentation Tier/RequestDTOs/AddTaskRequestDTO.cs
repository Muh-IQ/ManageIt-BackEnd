namespace Presentation_Tier.RequestDTOs
{
    public class AddTaskRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskListID { get; set; }
    }
}
