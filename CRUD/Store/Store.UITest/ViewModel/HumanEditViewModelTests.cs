namespace Store.UITest.ViewModel
{
    using System.Runtime;
    using Store.Model;
    using Store.UI.DateProvider;
    using Store.UI.Dialogs;
    using Store.UI.Events;
    using Store.UI.ViewModel;
    using Store;
    using Moq;
    using Prism.Events;
    using Store.UITest.Extensions;
    using Xunit;

    public class HumanEditViewModelTests
    {
        private const int _humanId = 5;

        private Mock<IHumanDataProvider> _dataProviderMock;

        private HumanEditViewModel _viewModel;

        private Mock<HumanSavedEvent> _humanSavedEventMock;

        private Mock<IEventAggregator> _eventAggrigatorMock;

        private Mock<HumanDeleteEvent> _humanDeletedEventMock;

        private Mock<IMessageDialogService> _messageDialogServiceMock;

        public HumanEditViewModelTests()
        {
            this._humanDeletedEventMock = new Mock<HumanDeleteEvent>();
            this._humanSavedEventMock = new Mock<HumanSavedEvent>();

            this._eventAggrigatorMock = new Mock<IEventAggregator>();
            this._eventAggrigatorMock.Setup(ea => ea.GetEvent<HumanSavedEvent>()).Returns(this._humanSavedEventMock.Object);
            this._eventAggrigatorMock.Setup(ea => ea.GetEvent<HumanDeleteEvent>()).Returns(this._humanDeletedEventMock.Object);
            this._dataProviderMock = new Mock<IHumanDataProvider>();
            this._dataProviderMock.Setup(dp => dp.GetHumanById(_humanId)).Returns(new Human() { Id = _humanId, FirstName = "Max" });
            this._messageDialogServiceMock = new Mock<IMessageDialogService>();
            this._viewModel = new HumanEditViewModel(this._dataProviderMock.Object, this._eventAggrigatorMock.Object, this._messageDialogServiceMock.Object);
        }

        [Fact]
        public void ShouldLoadHuman()
        {
            this._viewModel.Load(_humanId);
            Assert.NotNull(this._viewModel.Human);
            Assert.Equal(_humanId, this._viewModel.Human.Id);
            this._dataProviderMock.Verify(dp => dp.GetHumanById(_humanId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForHuman()
        {
            var fired = this._viewModel.IsPropertyChangedFired(() => this._viewModel.Load(_humanId), nameof(this._viewModel.Human));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenHumanIsLoaded()
        {
            this._viewModel.Load(_humanId);
            Assert.False(this._viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnableSaveCommandWhenHumanChanged()
        {
            this._viewModel.Load(_humanId);
            this._viewModel.Human.FirstName = "Changed";
            Assert.True(this._viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWithoutLoad()
        {
            this._viewModel.Load(_humanId);
            Assert.False(this._viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenHumanIsChanged()
        {
            this._viewModel.Load(_humanId);
            var fired = false;
            this._viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            this._viewModel.Human.FirstName = "Changed";
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            this._viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            this._viewModel.Load(_humanId);
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
        {
            var fired = false;
            this._viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            this._viewModel.Load(_humanId);
            Assert.True(fired);
        }

        [Fact]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
        {
            this._viewModel.Load(_humanId);
            this._viewModel.Human.FirstName = "Changed";
            this._viewModel.SaveCommand.Execute(null);
            this._dataProviderMock.Verify(dp => dp.SaveHuman(this._viewModel.Human.Model), Times.Once);
        }

        [Fact]
        public void ShouldAcceptChangesWhenSaveCommandIsExecuted()
        {
            this._viewModel.Load(_humanId);
            this._viewModel.Human.FirstName = "Changed";

            this._viewModel.SaveCommand.Execute(null);
            Assert.False(this._viewModel.Human.IsChanged);
        }
        [Fact]
        public void ShouldPublishHumanSaveEventWhenSaveCommandIsExecuted()
        {
            this._viewModel.Load(_humanId);
            this._viewModel.Human.FirstName = "Changed";

            this._viewModel.SaveCommand.Execute(null);
            this._humanSavedEventMock.Verify(e => e.Publish(this._viewModel.Human.Model), Times.Once);
            Assert.False(this._viewModel.Human.IsChanged);
        }

        [Fact]
        public void ShouldCreateNewHumanWhenNullIsPassedTOLoadMethod()
        {
            this._viewModel.Load(null);
            Assert.NotNull(this._viewModel.Human);
            Assert.Equal(0, this._viewModel.Human.Id);
            Assert.Null(this._viewModel.Human.FirstName);
            Assert.Null(this._viewModel.Human.FirstName);
            Assert.Null(this._viewModel.Human.Birthday);
            Assert.False(this._viewModel.Human.IsDeveloper);
            this._dataProviderMock.Verify(dp => dp.GetHumanById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ShouldEnableDeleteCommandForExistingHuman()
        {
            this._viewModel.Load(_humanId);
            Assert.True(this._viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisabledDeleteCommandForNewHuman()
        {
            this._viewModel.Load(null);
            Assert.False(this._viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisabledDeleteCommandWithoutLoad()
        {
            Assert.False(this._viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandWhenHumanIsChanged()
        {
            this._viewModel.Load(_humanId);
            var fired = false;
            this._viewModel.Human.FirstName = "Changed";
            this._viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            this._viewModel.Human.AcceptChanges();
            Assert.True(fired);
        }

       

        [Theory]
        [InlineData(MessageDialogResult.Yes, 1)]
        [InlineData(MessageDialogResult.No, 0)]
        public void ShouldPublishHumanDeletedEventWhenCommandIsExecuted(MessageDialogResult result, int expectedPublishCalls)
        {
            this._viewModel.Load(_humanId);

            this._messageDialogServiceMock.Setup(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                                                                          It.IsAny<string>())).Returns(result);

            this._viewModel.DeleteCommand.Execute(null);


            this._humanDeletedEventMock.Verify(e => e.Publish(_humanId), Times.Exactly(expectedPublishCalls));

            this._messageDialogServiceMock.Verify(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                                                                           It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public void ShouldDisplayCorrectMessageInDeleteDialog()
        {
            this._viewModel.Load(_humanId);
            var f = this._viewModel.Human;
            f.FirstName = "Max";
            f.LastName = "Power";

            this._viewModel.DeleteCommand.Execute(null);
            this._messageDialogServiceMock.Verify(d => d.ShowYesNoDialog("Delete Human?",
                                                                       $"Do you really want delete the human '{f.FirstName} {f.LastName}' "), Times.Once);
        }

    }
}