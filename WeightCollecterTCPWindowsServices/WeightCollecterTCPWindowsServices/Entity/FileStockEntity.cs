using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.FIS.Model.Domiain.Entity
{
    public class FileStockEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Site { get; set; }
        public string Category { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteDir { get; set; }
        public bool Active { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
        public string EKey { get; set; }
    }
}
