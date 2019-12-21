namespace Store.UITest.ViewModel
{
    using Moq;
    using Prism.Events;
    using Store.UI.Events;
    using Store.UI.ViewModel;
    using Store.UITest.Extensions;
    using Xunit;

    public class NavigationItemViewModelTests
    {
        private NavigationItemViewModel _viewModel;

        private Mock<IEventAggregator> _eventAggrigationMock;

        const int _humanId = 7;

        public NavigationItemViewModelTests()
        {
            this._eventAggrigationMock = new Mock<IEventAggregator>();
            this._viewModel = new NavigationItemViewModel(_humanId, "Max", this._eventAggrigationMock.Object);
        }

        [Fact]
        public void ShouldPublishOpenHumanEditViewEvent()
        {
            var eventMock = new Mock<OpenHumanEditViewEvent>();
            this._eventAggrigationMock.Setup(ea => ea.GetEvent<OpenHumanEditViewEvent>()).Returns(eventMock.Object);
            _viewModel.OpenHumanEditViewCommand.Execute(null);
            eventMock.Verify(e => e.Publish(_humanId), Times.Once);
        }

        [Fact]
        public void ShoudRaisePropertyChangedEventForDisplayMember()
        {
            var fired = this._viewModel.IsPropertyChangedFired(() => { this._viewModel.DisplayMember = "Changed"; }, nameof(this._viewModel.DisplayMember));
            Assert.True(fired);
        }
    }
}