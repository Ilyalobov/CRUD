namespace Store.UI.ViewModel
{
    using System.Windows.Input;
    using Prism.Events;
    using Store.UI.Command;
    using Store.UI.Events;
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _dispalyMember;
        public string DisplayMember
        {
            get
            {
                return _dispalyMember;
            }
            set
            {
                _dispalyMember = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; private set; }

        public ICommand OpenHumanEditViewCommand { get; private set; }

        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string dispalyMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = dispalyMember;
            OpenHumanEditViewCommand = new DelegateCommand(OnHumanEditViewCommand);
            _eventAggregator = eventAggregator;
        }

        private void OnHumanEditViewCommand(object obj)
        {
            this._eventAggregator.GetEvent<OpenHumanEditViewEvent>()
                .Publish(Id);
        }
    }
}