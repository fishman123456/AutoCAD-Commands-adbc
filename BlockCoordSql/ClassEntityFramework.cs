using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ACADCommands
{ 
    public class ClassEntityFramework
    {
        public class UserContext : DbContext
        {
            // берем строку из app.config    add name="UserDB"
            public UserContext() :
                base("UserDB")
            { }

            public DbSet<GetAtSaveCSV> Users { get; set; }
        }
    }
}
