using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

// Эта строка не является обязательной, но улучшает производительность загрузки
[assembly: CommandClass(typeof(ACADCommands.GetAttribValue))]

namespace ACADCommands
{
    public class GetAttribValue
    {
        [CommandMethod("LISTATT")]
        public static void ListAttributes()
        {
            CheckDateWork.CheckDate();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Database db = HostApplicationServices.WorkingDatabase;

            Transaction tr = db.TransactionManager.StartTransaction();
            // Start the transaction
            try
            {
                // Build a filter list so that only
                // block references are selected
                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.Start, "INSERT") };
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
                    BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
                    ed.WriteMessage("\nBlock: " + btr.Name);
                    btr.Dispose();
                    AttributeCollection attCol = blkRef.AttributeCollection;

                    foreach (ObjectId attId in attCol)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                        string str = ("ID: " + blkRef.Id.ToString() + ",\n " +
                                      "X: " + blkRef.Position.X.ToString() + ",\n" +
                                      "Y: " + blkRef.Position.Y.ToString() + ",\n" +
                                      "Z: " + blkRef.Position.Z.ToString() + ",\n" +
                                      "Handle BlockRef : " + blkRef.Handle.ToString() + ",\n" + // вот нужная фигня - Handle
                                      "Layer: " + blkRef.Layer.ToString() + ",\n" +
                                      "Attribute Tag: " + attRef.Tag + ",\n" +
                                      "Attribute String: " + attRef.TextString);
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

