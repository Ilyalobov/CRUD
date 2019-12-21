namespace Store.UI.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Prism.Events;
    using Store.UI.Command;
    using Store.UI.Events;

    public class MainViewModel : ViewModelBase
    {

        private IHumanEditViewModel selectedHumanEditViewModel;

        public ICommand CloseHumanTabCommand { get; private set; }

        private Func<IHumanEditViewModel> _humanEditVmCreator;

        public MainViewModel(InavigationViewModel navigationViewModel, Func<IHumanEditViewModel> humanEditVmCreator, IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            HumanEditViewModels = new ObservableCollection<IHumanEditViewModel>();
            this._humanEditVmCreator = humanEditVmCreator;
            eventAggregator.GetEvent<OpenHumanEditViewEvent>().Subscribe(OnOpenHumanEditView);
            eventAggregator.GetEvent<HumanDeleteEvent>().Subscribe(OnHumanDeleted);
            CloseHumanTabCommand = new DelegateCommand(OnCloseHumanTabExecute);
            AddHumanCommand = new DelegateCommand(OnAddHumanExecute);
        }

        private void OnHumanDeleted(int humanId)
        {
            var humanEditVm = HumanEditViewModels.Single(vm => vm.Human.Id == humanId);
            HumanEditViewModels.Remove(humanEditVm);
        }

        private void OnCloseHumanTabExecute(object obj)
        {
            var humanEditVm = (IHumanEditViewModel)obj;
            HumanEditViewModels.Remove(humanEditVm);
        }

        private void OnAddHumanExecute(object obj)
        {
            SelectedHumanEditViewModel = CreateAndLoadHumanEditViewModel(null);
        }

        private IHumanEditViewModel CreateAndLoadHumanEditViewModel(int? humanId)
        {
            var humanEditVM = this._humanEditVmCreator();
            HumanEditViewModels.Add(humanEditVM);
            humanEditVM.Load(humanId);
            return humanEditVM;
        }

        private void OnOpenHumanEditView(int humanId)
        {
            var humanEditVm = HumanEditViewModels.SingleOrDefault(vm => vm.Human.Id == humanId);
            if (humanEditVm == null)
            {
                humanEditVm = CreateAndLoadHumanEditViewModel(humanId);
            }
            SelectedHumanEditViewModel = humanEditVm;

        }

        public ICommand AddHumanCommand { get; private set; }

        public InavigationViewModel NavigationViewModel { get; private set; }

        public ObservableCollection<IHumanEditViewModel> HumanEditViewModels { get; private set; }

        public IHumanEditViewModel SelectedHumanEditViewModel
        {
            get
            {
                return this.selectedHumanEditViewModel;
            }
            set
            {
                this.selectedHumanEditViewModel = value;
                OnPropertyChanged();
            }
        }

        public void Load()
        {
            NavigationViewModel.Load();
        }

    }
}