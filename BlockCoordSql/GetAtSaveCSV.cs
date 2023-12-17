using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;


[assembly: CommandClass(typeof(ACADCommands.GetAtSaveCSV))]


namespace ACADCommands
{
    public class GetAtSaveCSV
    {
        [CommandMethod("ListCSV")]
        public static void ListAttrSaveCSV()
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
                // добавляем в массив выбранные обьекты
                ObjectId[] idArray = selSet.GetObjectIds();
                // строка для сохранения в csv
                StringBuilder stringBuilder = new StringBuilder();
                // перебираем блоки
                foreach (ObjectId blkId in idArray)
                {
                    BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
                    AttributeCollection attCol = blkRef.AttributeCollection;
                    // перебираем аттрибуты
                    foreach (ObjectId blkAttId in attCol)
                    // btr.Dispose();
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(blkAttId, OpenMode.ForRead);
                        // btr.Dispose();
                        //  выводим координаты блока,слой и handle
                        if (attRef.Tag == "ОБОЗНАЧ_КАБЕЛЯ")
                        {
                            string str = ("\n--------------------------\n" +
                                           "Block name: " + btr.Name + ",\n" +
                                           "ID: " + blkRef.Id.ToString() + ",\n" +
                                           "Attribute String: " + attRef.TextString + ",\n" +
                                           "X: " + blkRef.Position.X.ToString() + ",\n" +
                                           "Y: " + blkRef.Position.Y.ToString() + ",\n" +
                                           "Z: " + blkRef.Position.Z.ToString() + ",\n" +
                                           "Handle BlockRef : " + blkRef.Handle.ToString() + ",\n" + // вот нужная фигня - Handle
                                           "Layer: " + blkRef.Layer.ToString() + ",\n");
                            stringBuilder.Append(str);
                            ed.WriteMessage(str);
                        }
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

