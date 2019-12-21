using System.Collections.Generic;

namespace Store.UI.DateProvider
{
    using Store.Model;

    public interface INavigationDataProvider
    {
        IEnumerable<LookItem> GetAllHumans();
    }
}