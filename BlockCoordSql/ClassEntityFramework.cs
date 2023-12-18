using ACADCommands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADCommands
{
    public class ClassEntityFramework : DbContext
    {
        // берем строку из app.config    add name="UserDB"
        public ClassEntityFramework() : base("UserDB") { }
        // правильно обьекты нужно сформировать 19-12-2023 
        public DbSet<GetAtSaveCSV> Users { get; set; }
        public void EntList(List<string> strings)
        {
            // вывести из базы добавленные обьекты - не нужно делать
            using (ClassEntityFramework db = new ClassEntityFramework())
            {
                var users = db.Users;
                foreach (var User in users)
                {

                }
            }
        }
    }
}
