using System;
using System.Collections.Generic;

namespace Store.UI.DateProvider
{
    using Store.DataAccess;
    using Store.Model;

    class NavigationDataProvider : INavigationDataProvider
    {
        private Func<IDataService> _dateServiceCreater;

        public NavigationDataProvider(Func<IDataService> dataServiceCreater)
        {
            _dateServiceCreater = dataServiceCreater;
        }

        public IEnumerable<LookItem> GetAllHumans()
        {
            using (var dataservice = _dateServiceCreater())
            {
                return dataservice.GetAllHumans();
            }
        }
    }
}