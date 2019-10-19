namespace ComputerScienceBlogBackEnd.Models
{
    public class ComputerScienceBlogDatabaseSettings : IComputerScienceBlogDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ArticlesCollectionName { get; set; }
    }
}
