using System.Collections.Generic;
using System.Linq;
using Moq;
using Prism.Events;
using Store.Model;
using Store.UI.Events;
using Store.UI.ViewModel;
using Store.UI.Wrapper;
using Store.UITest.Extensions;
using Xunit;

namespace Store.UITest.ViewModel
{
    public class MainViewModelTest
    {
        private MainViewModel _viewModel;

        private Mock<InavigationViewModel> _navigationViewModelMock;

        private Mock<IEventAggregator> _eventAggregatorMock;

        private OpenHumanEditViewEvent openHumanEditViewEvent;

        private List<Mock<IHumanEditViewModel>> _humanEditViewModelMoks;

        private HumanDeleteEvent humanDeletedEvent;

        public MainViewModelTest()
        {
            this._humanEditViewModelMoks = new List<Mock<IHumanEditViewModel>>();
            _navigationViewModelMock = new Mock<InavigationViewModel>();
            this.humanDeletedEvent = new HumanDeleteEvent();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            this.openHumanEditViewEvent = new OpenHumanEditViewEvent();

            this._eventAggregatorMock.Setup(ea => ea.GetEvent<OpenHumanEditViewEvent>()).Returns(this.openHumanEditViewEvent);

            this._eventAggregatorMock.Setup(ea => ea.GetEvent<HumanDeleteEvent>()).Returns(this.humanDeletedEvent);

            this._viewModel = new MainViewModel(_navigationViewModelMock.Object, CreateHumanEditViewModel, this._eventAggregatorMock.Object);
        }

        private IHumanEditViewModel CreateHumanEditViewModel()
        {
            var humanItem = new Mock<IHumanEditViewModel>();
            humanItem.Setup(vm => vm.Load(It.IsAny<int>())).Callback<int?>(i =>
                                                                           {
                                                                               humanItem.Setup(vm => vm.Human).Returns(new HumanWrapper(new Human()
                                                                                                                                        {
                                                                                                                                                Id = i.Value
                                                                                                                                        }));
                                                                           });
            this._humanEditViewModelMoks.Add(humanItem);
            return humanItem.Object;
        }

        [Fact]
        public void ShouldCallTheLoadMethodofTheNavigationViewModel()
        {
            _viewModel.Load();

            _navigationViewModelMock.Verify(r => r.Load(), Times.Once);
        }

        [Fact]
        public void ShouldAddHumanEditViewModelAndLoadAndSelectIt()
        {
            const int humanId = 7;
            this.openHumanEditViewEvent.Publish(humanId);
            Assert.Equal(1, this._viewModel.HumanEditViewModels.Count);
            var humanEditVm = this._viewModel.HumanEditViewModels.First();
            Assert.Equal(humanEditVm, this._viewModel.SelectedHumanEditViewModel);
            this._humanEditViewModelMoks.First().Verify(vm => vm.Load(humanId), Times.Once);
        }

        [Fact]
        public void ShouldAddHumanEditViewModelOnlyOnce()
        {
            this.openHumanEditViewEvent.Publish(5);
            this.openHumanEditViewEvent.Publish(5);
            this.openHumanEditViewEvent.Publish(6);
            this.openHumanEditViewEvent.Publish(7);
            Assert.Equal(3, this._viewModel.HumanEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedHumanEditViewModel()
        {
            var humanEditVmMock = new Mock<IHumanEditViewModel>();

            var fired = this._viewModel.IsPropertyChangedFired(() => { this._viewModel.SelectedHumanEditViewModel = humanEditVmMock.Object; }, nameof(this._viewModel.SelectedHumanEditViewModel));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRemoveHumanEditViewModelOnCloseHumanTabCommand()
        {
            this.openHumanEditViewEvent.Publish(7);
            var humanEditVm = this._viewModel.SelectedHumanEditViewModel;
            this._viewModel.CloseHumanTabCommand.Execute(humanEditVm);
            Assert.Equal(0, this._viewModel.HumanEditViewModels.Count);
        }

        [Fact]
        public void ShouldAddHumanEditViewModelAndLoadItWithIdNullAndSelectIt()
        {
            this._viewModel.AddHumanCommand.Execute(null);

            Assert.Equal(1, this._viewModel.HumanEditViewModels.Count);
            var humanEditVm = this._viewModel.HumanEditViewModels.First();
            Assert.Equal(humanEditVm, this._viewModel.SelectedHumanEditViewModel);
            this._humanEditViewModelMoks.First().Verify(vm => vm.Load(null), Times.Once);
        }

        [Fact]
        public void ShouldRemoveHumanEditViewModelOnHumanDeleteEvent()
        {
            const int deletedHumanId = 7;
            this.openHumanEditViewEvent.Publish(deletedHumanId);
            this.openHumanEditViewEvent.Publish(8);
            this.openHumanEditViewEvent.Publish(9);

            this.humanDeletedEvent.Publish(deletedHumanId);

            Assert.Equal(2, this._viewModel.HumanEditViewModels.Count);

            Assert.True(this._viewModel.HumanEditViewModels.All(vm => vm.Human.Id != deletedHumanId));
        }
    }
}