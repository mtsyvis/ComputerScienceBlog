namespace ComputerScienceBlogBackEnd.Models
{
    public interface IComputerScienceBlogDatabaseSettings
    {
        string UsersCollectionName { get; set; }

        string ArticlesCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
