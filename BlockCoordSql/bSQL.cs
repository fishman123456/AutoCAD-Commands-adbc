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
            CheckDateWork.CheckDate();
            List<string> Coorxyz = new List<string>();

            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc == null)
                return;
            Database acCurrDb = acDoc.Database;

            using (Transaction acTrans = acCurrDb.TransactionManager.StartTransaction())
            {
                // открываем таблицу слоев документа
                LayerTable acLyrTbl = acTrans.GetObject(acCurrDb.LayerTableId, OpenMode.ForWrite) as LayerTable;
                // массив для фильтра
                TypedValue[] acTypValArr = new TypedValue[1];
                // первый элемент для фильтра
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
                                // Добавляем в список координаты блоков 14-12-2023
                                Coorxyz.Add("ID: " + acBlockRef.Id.ToString() + ",\n " +
                                            "X: " + acBlockRef.Position.X.ToString() + ",\n" +
                                            "Y: " + acBlockRef.Position.Y.ToString() + ",\n" +
                                            "Z: " + acBlockRef.Position.Z.ToString() + ",\n" +
                                            "Handle: " + acBlkTbl.Handle.ToString() + ",\n" +
                                            "ObjectId: " + acBlkTblRec.ObjectId.ToString() + ",\n" +
                                            "Handle BlockRef : " + acBlockRef.Handle.ToString() + ",\n" + // вот нужеая фигня - Handle
                                            "Layer: " + acBlockRef.Layer.ToString());
                                // слой забирается, в котором блок находится - 13-02-2023 
                                // нужен аттрибут 
                                // вывод в коммандную строку
                                foreach(string str  in Coorxyz)
                                {
                                    acDoc.Editor.WriteMessage(str);
                                }
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
