namespace Store.UITest.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Prism.Events;
    using Store.Model;
    using Store.UI.DateProvider;
    using Store.UI.Events;
    using Store.UI.ViewModel;
    using Xunit;

    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModels;

        private HumanSavedEvent _humanSavedEvent;

        private HumanDeleteEvent _humanDelitedEvent;

        public NavigationViewModelTests()
        {
            this._humanSavedEvent = new HumanSavedEvent();
            this._humanDelitedEvent = new HumanDeleteEvent();

            var eventAggregarorMock = new Mock<IEventAggregator>();
            eventAggregarorMock.Setup(ea => ea.GetEvent<HumanSavedEvent>()).Returns(this._humanSavedEvent);
            eventAggregarorMock.Setup(ea => ea.GetEvent<HumanDeleteEvent>()).Returns(this._humanDelitedEvent);
            var navigationDataProviderMock = new Mock<INavigationDataProvider>();

            navigationDataProviderMock.Setup(dp => dp.GetAllHumans()).Returns(new List<LookItem>()
                                                                              {
                                                                                      new LookItem() { Id = 1, DisplayMember = "Max" },
                                                                                      new LookItem() { Id = 2, DisplayMember = "Power" }
                                                                              });
            this._viewModels = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregarorMock.Object);
        }

        [Fact]
        public void ShouldLoadHuman()
        {
            this._viewModels.Load();
            Assert.Equal(2, this._viewModels.Humans.Count);

            var human = this._viewModels.Humans.SingleOrDefault(r => r.Id == 1);
            Assert.NotNull(human);
            Assert.Equal("Max", human.DisplayMember);

            human = this._viewModels.Humans.SingleOrDefault(r => r.Id == 2);
            Assert.NotNull(human);
            Assert.Equal("Power", human.DisplayMember);
        }

        [Fact]
        public void ShouldLoadeFirendsOnlyOnce()
        {
            this._viewModels.Load();
            this._viewModels.Load();
            Assert.Equal(2, this._viewModels.Humans.Count);
        }

        [Fact]
        public void ShouldUpdateNavigationItemWhenHumanIsSaved()
        {
            this._viewModels.Load();
            var navigationItem = this._viewModels.Humans.First();
            var human = navigationItem.Id;
            this._humanSavedEvent.Publish(new Human()
                                          {
                                                  Id = human,
                                                  FirstName = "Max",
                                                  LastName = "Power"
                                          });
            Assert.Equal("Max Power", navigationItem.DisplayMember);
        }

        [Fact]
        public void ShouldAddNavigationItemWhenAddedHumanIsSaved()
        {
            this._viewModels.Load();
            const int newHumanId = 97;
            this._humanSavedEvent.Publish(new Human()
                                          {
                                                  Id = newHumanId,
                                                  FirstName = "Max",
                                                  LastName = "Power"
                                          });
            Assert.Equal(3, this._viewModels.Humans.Count);
            var addedItem = this._viewModels.Humans.SingleOrDefault(f => f.Id == newHumanId);
            Assert.NotNull(addedItem);
            Assert.Equal("Max Power", addedItem.DisplayMember);
        }

        [Fact]
        public void ShouldRemoveNavigationItemWhenHumanIsDeleted()
        {
            this._viewModels.Load();
            var deletedFriedId = this._viewModels.Humans.First().Id;
            this._humanDelitedEvent.Publish(deletedFriedId);
            Assert.Equal(1, this._viewModels.Humans.Count);
            Assert.NotEqual(deletedFriedId, this._viewModels.Humans.Single().Id);
        }
    }
}