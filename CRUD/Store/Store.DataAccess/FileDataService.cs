using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Store.Model;

namespace Store.DataAccess
{
    public class FileDataService : IDataService
    {
        private const string StorageFile = "Store.json";

        public Human GetHumanById(int humanId)
        {
            var humans = ReadFromFile();
            return humans.Single(f => f.Id == humanId);
        }

        public void SaveHuman(Human human)
        {
            if (human.Id <= 0)
            {
                InsertHuman(human);
            }
            else
            {
                UpdateHuman(human);
            }
        }

        public void DeleteHuman(int humanId)
        {
            var humans = ReadFromFile();
            var existing = humans.Single(f => f.Id == humanId);
            humans.Remove(existing);
            SaveToFile(humans);
        }

        private void UpdateHuman(Human human)
        {
            var humans = ReadFromFile();
            var existing = humans.Single(f => f.Id == human.Id);
            var indexOfExisting = humans.IndexOf(existing);
            humans.Insert(indexOfExisting, human);
            humans.Remove(existing);
            SaveToFile(humans);
        }

        private void InsertHuman(Human human)
        {
            var humans = ReadFromFile();
            var maxHumanId = humans.Count == 0 ? 0 : humans.Max(f => f.Id);
            human.Id = maxHumanId + 1;
            humans.Add(human);
            SaveToFile(humans);
        }

        public IEnumerable<LookItem> GetAllHumans()
        {
            return ReadFromFile().Select(r => new LookItem()
                                              {
                                                      Id = r.Id,
                                                      DisplayMember = $"{r.FirstName} {r.LastName}"
                                              });
        }

        public void Dispose() { }

        private void SaveToFile(List<Human> humanList)
        {
            string json = JsonConvert.SerializeObject(humanList, Formatting.Indented);
            File.WriteAllText(StorageFile, json);
        }

        private List<Human> ReadFromFile()
        {
            if (!File.Exists(StorageFile))
            {
                var readFromFile = new List<Human>
                                   {
                                           new Human
                                           {
                                                   Id = 1, FirstName = "Pam", LastName = "Wilson",
                                                   Birthday = new DateTime(1980, 10, 28), IsFriend = true
                                           },
                                           new Human
                                           {
                                                   Id = 2, FirstName = "Edna", LastName = "Byrd",
                                                   Birthday = new DateTime(1982, 10, 10)
                                           },
                                           new Human
                                           {
                                                   Id = 3, FirstName = "Albert", LastName = "Wilson",
                                                   Birthday = new DateTime(2011, 05, 13)
                                           },
                                           new Human
                                           {
                                                   Id = 4, FirstName = "Valerie", LastName = "Kammer",
                                                   Birthday = new DateTime(2013, 02, 25)
                                           },
                                           new Human
                                           {
                                                   Id = 5, FirstName = "Gerald", LastName = "Solis",
                                                   Birthday = new DateTime(1981, 01, 10), IsFriend = true
                                           },
                                           new Human
                                           {
                                                   Id = 6, FirstName = "Mike", LastName = "Walton",
                                                   Birthday = new DateTime(1970, 03, 5), IsFriend = true
                                           },
                                           new Human
                                           {
                                                   Id = 7, FirstName = "Mary", LastName = "Plank",
                                                   Birthday = new DateTime(1987, 07, 16)
                                           },
                                           new Human
                                           {
                                                   Id = 8, FirstName = "William", LastName = "Norris",
                                                   Birthday = new DateTime(1983, 05, 23)
                                           },
                                   };
                return readFromFile;
            }

            string json = File.ReadAllText(StorageFile);
            return JsonConvert.DeserializeObject<List<Human>>(json);
        }
    }
}