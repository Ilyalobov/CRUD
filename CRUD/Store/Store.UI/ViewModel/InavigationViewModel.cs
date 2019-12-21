namespace Store.UI.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Prism.Events;
    using Store.Model;
    using Store.UI.DateProvider;
    using Store.UI.Events;

    public interface InavigationViewModel
    {
        void Load();
    }

    public class NavigationViewModel : ViewModelBase, InavigationViewModel
    {
        private INavigationDataProvider _dataProvider;

        private IEventAggregator _eventAgriggator;

        public NavigationViewModel(INavigationDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            Humans = new ObservableCollection<NavigationItemViewModel>();
            _dataProvider = dataProvider;
            _eventAgriggator = eventAggregator;
            this._eventAgriggator.GetEvent<HumanSavedEvent>().Subscribe(OnHumanSaved);
            this._eventAgriggator.GetEvent<HumanDeleteEvent>().Subscribe(OnHumanDeleted);
        }

        private void OnHumanDeleted(int humanId)
        {
            var navigationItem = Humans.Single(n => n.Id == humanId);
            Humans.Remove(navigationItem);

        }

        private void OnHumanSaved(Human human)
        {
            var DisplayMember = $"{human.FirstName} {human.LastName}";
            var navigationItem = Humans.SingleOrDefault(r => r.Id == human.Id);
            if (navigationItem != null)
            {
                navigationItem.DisplayMember = DisplayMember;
            }
            else
            {
                navigationItem = new NavigationItemViewModel(human.Id, DisplayMember, this._eventAgriggator);
                Humans.Add(navigationItem);
            }
        }

        public void Load()
        {
            Humans.Clear();

            foreach (var human in this._dataProvider.GetAllHumans())
            {
                Humans.Add(new NavigationItemViewModel(human.Id, human.DisplayMember, this._eventAgriggator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Humans { get; private set; }
    }
}