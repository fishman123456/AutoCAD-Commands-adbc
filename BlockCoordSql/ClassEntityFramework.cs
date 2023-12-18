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
            public ClassEntityFramework() : base("UserDB")
            { }
            public DbSet<GetAtSaveCSV> Users { get; set; }
    }
}
// добавить в базу данных список
//using (ClassEntityFramework db = new ClassEntityFramework())
//{
//    var users = db.Users;
//    foreach (var User in users)
//    {

//    }