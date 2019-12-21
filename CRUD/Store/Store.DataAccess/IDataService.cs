using System;
using Store.Model;
using System.Collections.Generic;

namespace Store.DataAccess
{
    public interface IDataService : IDisposable
    {
        Human GetHumanById(int humanId);

        void SaveHuman(Human human);

        void DeleteHuman(int humanId);

        IEnumerable<LookItem> GetAllHumans();
    }
}