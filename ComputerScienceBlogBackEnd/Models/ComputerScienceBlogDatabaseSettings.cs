using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScienceBlogBackEnd.Models
{
    public class ComputerScienceBlogDatabaseSettings : IComputerScienceBlogDatabaseSettings
    {
        public string UsersCollectionName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DatabaseName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ArticlesCollectionName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
