using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSN.Configuration
{
    public class Config
    {
        public IList<TorrentConfig> Torrents { get; set; } = [];
        public IList<PingUp> PingUps { get; set; } = [];

        public TaskList Tasks { get; set; } = new TaskList();
    }
}
