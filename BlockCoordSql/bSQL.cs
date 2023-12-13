using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Collections.Generic;


// Эта строка не является обязательной, но улучшает производительность загрузки
[assembly: CommandClass(typeof(ACADCommands.bSQL))]

namespace ACADCommands
{


    // Этот класс создается AutoCAD для каждого документа, когда
    // команда вызывается пользователем в первый раз в контексте
    // данного документа. Другими словами, нестатические данные в этом классе
    // неявно относятся к каждому документу!
    public class bSQL
    {
        public List<string> CoorxyzGet { get; }

        [CommandMethod("bSQL")]
        public static void BlkCoords()
        {
            List<string> Coorxyz = new List<string>();

            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc == null)
                return;
            Database acCurrDb = acDoc.Database;

            using (Transaction acTrans = acCurrDb.TransactionManager.StartTransaction())
            {
                TypedValue[] acTypValArr = new TypedValue[1];
                acTypValArr.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
                SelectionFilter acSelFilter = new SelectionFilter(acTypValArr);
                PromptSelectionResult acSSPromptRes = acDoc.Editor.GetSelection(acSelFilter);
                if (acSSPromptRes.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPromptRes.Value;
                    foreach (SelectedObject acSObj in acSSet)
                    {
                        if (acSObj != null)
                        {
                            // берем блок по ID
                            BlockReference acBlockRef = acTrans.GetObject(acSObj.ObjectId, OpenMode.ForWrite) as BlockReference;
                            // пока не закончились блоки 
                            if (acBlockRef != null)
                            {
                                BlockTable acBlkTbl = acTrans.GetObject(
                                            acCurrDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                                BlockTableRecord acBlkTblRec = acTrans.GetObject(
                                            acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                                // Добавляем в список координаты блоков
                                Coorxyz.Add("ID - " + acBlockRef.Id.ToString() + "," +
                                            "X - " + acBlockRef.Position.X.ToString() + "," +
                                            "Y - " + acBlockRef.Position.Y.ToString() + "," +
                                            "Z - " + acBlockRef.Position.Z.ToString() + "," +
                                            "LayerEval - " + acBlkTbl.Database.LayerEval.ToString() + "," +
                                            "Handle - " + acBlkTbl.Handle.ToString() + "," +
                                            "Attr - " + acBlockRef.AttributeCollection.ToString());

                            }
                        }
                    }
                    acTrans.Commit();
                    acDoc.Editor.Regen();
                }
                else
                {
                    acDoc.Editor.WriteMessage("\nCanceled.");
                }
            }
        }
    }
}
