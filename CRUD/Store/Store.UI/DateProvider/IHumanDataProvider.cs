namespace Store.UI.DateProvider
{
    using Store.Model;

    public interface IHumanDataProvider
    {
        Human GetHumanById(int id);

        void SaveHuman(Human human);

        void DeleteHuman(int id);
    }
}