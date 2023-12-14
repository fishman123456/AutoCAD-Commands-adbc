using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Эта строка не является обязательной, но улучшает производительность загрузки
[assembly: CommandClass(typeof(ACADCommands.GetAttribValue))]

namespace ACADCommands
{
    internal class GetAttribValue
    {
        public class DumpAttributes
        {
            [CommandMethod("LISTATT")]
            public void ListAttributes()
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                Database db = HostApplicationServices.WorkingDatabase;

                Transaction tr = db.TransactionManager.StartTransaction();
                // Start the transaction
                try
                {
                    // Build a filter list so that only
                    // block references are selected
                    TypedValue[] filList = new TypedValue[1] {new TypedValue((int)DxfCode.Start, "INSERT")};
                    SelectionFilter filter = new SelectionFilter(filList);
                    PromptSelectionOptions opts = new PromptSelectionOptions();
                    opts.MessageForAdding = "Select block references: ";
                    PromptSelectionResult res = ed.GetSelection(opts, filter);
                    // Do nothing if selection is unsuccessful
                    if (res.Status != PromptStatus.OK)
                        return;
                    SelectionSet selSet = res.Value;
                    ObjectId[] idArray = selSet.GetObjectIds();
                    foreach (ObjectId blkId in idArray)
                    {
                        BlockReference blkRef = (BlockReference)tr.GetObject(blkId,OpenMode.ForRead);

                        BlockTableRecord btr =(BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord,OpenMode.ForRead);
                        ed.WriteMessage("\nBlock: " + btr.Name);
                        btr.Dispose();
                        AttributeCollection attCol = blkRef.AttributeCollection;

                        foreach (ObjectId attId in attCol)
                        {
                            AttributeReference attRef = (AttributeReference)tr.GetObject(attId,OpenMode.ForRead);
                            string str = ("\n  Attribute Tag: " + attRef.Tag + "\n    Attribute String: " + attRef.TextString);
                            ed.WriteMessage(str);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage(("Exception: " + ex.Message));
                }
                finally
                {
                    tr.Dispose();
                }
            }
        }
    }
}

