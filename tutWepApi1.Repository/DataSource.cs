using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tutWepApi1.Repository.Entities;

namespace tutWepApi1.Repository
{
    public class DataSource
    {
        public List<Food> GetAllFoods()
        {
            return new List<Food>
            {
                new Food { Id = 1, Description = "Food Market" },
                new Food { Id = 2, Description = "Puran Dhaka Street Food" },
                new Food { Id = 3, Description = "Baily road food shop" },
                new Food { Id = 4, Description = "Mustakim Chaap" }
            };
        }

        public List<Food> GetFoodsWithMeasures()
        {
            List<Food> foods = this.GetAllFoods();

            int index = 1;

            foreach (Food food in foods)
            {
                food.Measures = new List<Measure>
                {
                    new Measure { Id = 1, food_id = food.Id, Description = Category.Sweet.ToString(), Calories = index + 1, Carbonhydrates = index + 2, Fiber = index + 3, Protien = index + 4, Sugar = index + 5 },
                    new Measure { Id = 2, food_id = food.Id, Description = Category.Sour.ToString(), Calories = index + 10, Carbonhydrates = index + 9, Fiber = index + 8, Protien = index + 7, Sugar = index + 6 },
                    new Measure { Id = 3, food_id = food.Id, Description = Category.Salt.ToString(), Calories = index + 11, Carbonhydrates = index + 12, Fiber = index + 13, Protien = index + 14, Sugar = index + 15 }
                };

                index = index + 1;
            }

            return foods;
        }

        public ICollection<Measure> GetMeasure(int foodId)
        {
            ICollection<Measure> results = new List<Measure>();
            var food = this.GetFoodsWithMeasures().Where(x => x.Id == foodId).FirstOrDefault();
            if (food != null)
            {
                results = food.Measures;
            }

            return results;
        }

        public List<Diary> GetDiaries()
        {
            return new List<Diary>
            {
                new Diary { Id = 1, CurrentDate = new DateTime(2015, 12, 1), UserName = "yeasin" },
                new Diary { Id = 2, CurrentDate = new DateTime(2015, 12, 5), UserName = "shakil" },
                new Diary { Id = 3, CurrentDate = new DateTime(2015, 12, 8), UserName = "aminur" },
                new Diary { Id = 4, CurrentDate = new DateTime(2015, 12, 10), UserName = "yeasin" },
            };
        }

        public List<Diary> GetDiaries(string username)
        {
            return this.GetDiaries().Where(x => x.UserName == username).ToList();
        }

        public Diary GetDiary(string username, DateTime date)
        {
            return this.GetDiariesWithEntry().Where(x => x.UserName == username && x.CurrentDate == date).FirstOrDefault();
        }

        public List<Diary> GetDiariesWithEntry()
        {
            List<Diary> diaries = this.GetDiaries();

            int index = 1;

            foreach(Diary diary in diaries)
            {
                diary.DiaryEntires = new List<DiaryEntry>
                {
                    new DiaryEntry { Id = 1, diary_id = diary.Id, DiaryCurrentDate = diary.CurrentDate.ToString("yyyy-MM-dd"), Quantity = index + 2 },
                    new DiaryEntry { Id = 2, diary_id = diary.Id, DiaryCurrentDate = diary.CurrentDate.ToString("yyyy-MM-dd"), Quantity = index + 4 },
                    new DiaryEntry { Id = 3, diary_id = diary.Id, DiaryCurrentDate = diary.CurrentDate.ToString("yyyy-MM-dd"), Quantity = index + 6 },
                    new DiaryEntry { Id = 4, diary_id = diary.Id, DiaryCurrentDate = diary.CurrentDate.ToString("yyyy-MM-dd"), Quantity = index + 8 }
                };

                index = index + 1;
            }

            return diaries;
        }

        public ICollection<DiaryEntry> GetDiaryEntries(DateTime d, string username)
        {
            ICollection<DiaryEntry> results = new List<DiaryEntry>();

            var diaryEntry = this.GetDiariesWithEntry().Where(x => x.CurrentDate == d && x.UserName == username).FirstOrDefault();
            if (diaryEntry != null)
            {
                results = diaryEntry.DiaryEntires;
            }

            return results;
        }

        public DiaryEntry GetDiarEntry(DateTime d, string username, int id)
        {
            DiaryEntry results = null;

            var diaryEntry = this.GetDiariesWithEntry().Where(x => x.CurrentDate == d && x.UserName == username).FirstOrDefault();
            if (diaryEntry != null)
            {
                results = diaryEntry.DiaryEntires.Where(x => x.Id == id).FirstOrDefault();
            }

            return results;
        }

        public List<ApiUser> GetUsers()
        {
            return new List<ApiUser>()
            {
                new ApiUser() { id = 1, AppId = "F1EA05D4-8651-465B-A612-253BDF8C96CB", Secret = "yeasinSecretKey", Name = "yeasin" },
                new ApiUser() { id = 1, AppId = "0161A906-04C9-4B45-853C-28539B0943AA", Secret = "shakilSecretkey", Name = "shakil" }
            };
        }

        public AuthToken GetAuthToken(string GetAuthToken)
        {
            return null;
        }
    }
}
