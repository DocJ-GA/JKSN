using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using JKSN.Configuration;

namespace JKSN
{
    public class TaskList
        : List<ITask>
    {
        public TaskList()
            : base()
        { }

        public TaskList(IEnumerable<ITask> collection)
            : base(collection)
        { }

        public new void Add(ITask item)
        {
            if (this.Any(x => x.Name == item.Name))
                throw new ArgumentException($"Task with name {item.Name} already exists.");
            else
                base.Add(item);
        }

        public bool TryAdd(ITask item)
        {
            if (this.Any(x => x.Name == item.Name))
                return false; // Do nothing if the task already exists
            
            base.Add(item);
            return true;
        }
    }
}
