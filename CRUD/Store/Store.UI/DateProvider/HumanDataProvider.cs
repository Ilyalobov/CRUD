using System;
using Store.DataAccess;
using Store.Model;

namespace Store.UI.DateProvider
{
    class HumanDataProvider : IHumanDataProvider
    {
        private readonly Func<IDataService> _dataServiceCreator;

        public HumanDataProvider(Func<IDataService> dataServiceCreator)
        {
            this._dataServiceCreator = dataServiceCreator;
        }

        public Human GetHumanById(int id)
        {
            using (var dataService = this._dataServiceCreator())
            {
                return dataService.GetHumanById(id);
            }
        }

        public void SaveHuman(Human human)
        {
            using (var dataService = this._dataServiceCreator())
            {
                dataService.SaveHuman(human);
            }
        }

        public void DeleteHuman(int id)
        {
            using (var dataService = this._dataServiceCreator())
            {
                dataService.DeleteHuman(id);
            }
        }
    }
}