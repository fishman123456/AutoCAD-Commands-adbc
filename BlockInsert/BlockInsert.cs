using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(ACADCommands.BlockInsert))]

namespace ACADCommands
{
    // This line is not mandatory, but improves loading performances
    public class BlockInsert
    {
        // реализовать код для вставки блока
    }
}
