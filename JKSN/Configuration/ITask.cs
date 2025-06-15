using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSN.Configuration
{
    public interface ITask
    {
        public string Name { get; }

        public int Interval { get; }

        public DateTimeOffset LastRun { get; set; }

        public FailedState FailedState { get; set; }

        public void Load(ILogger logger);

        public Task LoadAsync(ILogger logger);

        public void Run();

        public Task RunAsync();

        public void Save();

        public Task SaveAsync();
    }
}
