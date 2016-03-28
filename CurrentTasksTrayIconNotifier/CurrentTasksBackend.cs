using FizzWare.NBuilder;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CurrentTasksTrayIconNotifier
{
    internal class CurrentTasksBackend
    {
        private IList<CurrentTask> Tasks { get; set; }

        public CurrentTasksBackend()
        { 
            Tasks = Builder<CurrentTask>.CreateListOfSize(10)
                .All()
                    .With(x => x.Id = Guid.NewGuid().ToString())
                    .With(x => x.Name = Faker.TextFaker.Sentence())
                    .With(x => x.Start = Faker.DateTimeFaker.DateTimeBetweenDays(5))
                    .With(x => x.ETA = x.Start.AddMinutes(Faker.NumberFaker.Number()))
                    .With(x => x.Percent = 10)
                .Build()
                ;
            var n = 0;
            foreach(var t in Tasks)
            {
                t.Order = n;
                n++;
            }
        }

        internal void RandomIncreaseCounters()
        {
            var die = new Random();
            foreach(var task in Tasks)
            {
                task.IncProgress(die.Next(10, 30));
            }
        }

        internal IEnumerable<CurrentTask> DeletedTasks()
        {
            return new List<CurrentTask>();
        }

        public IEnumerable<CurrentTask> GetTasks()
        {
            return Tasks.OrderBy(x => x.Order);
        }

        public CurrentTask GetById(string id)
        {
            return Tasks.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}