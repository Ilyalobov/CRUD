namespace Store.UI.ViewModel
{
    using System.ComponentModel;
    using System.Windows.Input;
    using Prism.Events;
    using Store.Model;
    using Store.UI.DateProvider;
    using Store.UI.Dialogs;
    using Store.UI.Events;
    using Store.UI.Command;
    using Store.UI.Wrapper;

    public interface IHumanEditViewModel
    {
        void Load(int? humanId);

        HumanWrapper Human { get; }
    }

    public class HumanEditViewModel : ViewModelBase, IHumanEditViewModel
    {
        private IHumanDataProvider _dataProvider;

        private HumanWrapper human;

        private IEventAggregator _eventAggregator;

        private IMessageDialogService _messageDialogService;

        public HumanEditViewModel(IHumanDataProvider dataProvider, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            this._dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

        private void OnSaveExecute(object obj)
        {
            this._dataProvider.SaveHuman(Human.Model);
            Human.AcceptChanges();
            this._eventAggregator.GetEvent<HumanSavedEvent>().Publish(Human.Model);
        }

        private bool OnSaveCanExecute(object arg)
        {
            return Human != null && Human.IsChanged;
        }

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public void Load(int? humanId)
        {
            var human = humanId.HasValue ? this._dataProvider.GetHumanById(humanId.Value) : new Human();
            Human = new HumanWrapper(human);

            Human.PropertyChanged += Human_PropertyChanged;

            InvlidateCommands();
        }

        private void Human_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvlidateCommands();
        }

        private void InvlidateCommands()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        public HumanWrapper Human
        {
            get { return this.human; }
            private set
            {
                this.human = value;
                OnPropertyChanged();
            }
        }

        private void OnDeleteExecute(object obj)
        {
            var result = this._messageDialogService.ShowYesNoDialog("Delete Human?", $"Do you really want delete the human '{Human.FirstName} {Human.LastName}' ");
            if (result == MessageDialogResult.Yes)
            {
                this._dataProvider.DeleteHuman(Human.Id);
                this._eventAggregator.GetEvent<HumanDeleteEvent>().Publish(Human.Id);
            }
        }

        private bool OnDeleteCanExecute(object arg)
        {
            return Human != null && Human.Id > 0;
        }
    }
}