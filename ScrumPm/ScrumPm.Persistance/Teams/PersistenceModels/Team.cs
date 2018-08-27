namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProductOwnerId { get; set; }
        public ProductOwner ProductOwner { get; set; }
    }
}
