using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

// Эта строка не является обязательной, но улучшает производительность загрузки
[assembly: CommandClass(typeof(ACADCommands.СableSections))]
// сборка позволяет создавать участки кабеля по блокам 15-12-2023
namespace ACADCommands
{
    public class СableSections
    {
        [CommandMethod("Cableselect")]
        public static void BlockCableSelections()
        {
            // проверка по текущей дате - защита
            CheckDateWork.CheckDate();
            // берем активный документ, чертеж в котором мы сейчас находимся
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // берем базу данных чертежа, контейнер где есть все данные по обьектам в чертеже
            Database db = HostApplicationServices.WorkingDatabase;
            // начинаем транзакцию
            Transaction tr = db.TransactionManager.StartTransaction();
            // Start the transaction
            try
            {
                // Build a filter list so that only
                // block references are selected
                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.Start, "INSERT") };
                SelectionFilter filter = new SelectionFilter(filList);
                PromptSelectionOptions opts = new PromptSelectionOptions();
                opts.MessageForAdding = "Выберите блоки: ";
                PromptSelectionResult res = ed.GetSelection(opts, filter);
                // Do nothing if selection is unsuccessful
                if (res.Status != PromptStatus.OK)
                    return;
                SelectionSet selSet = res.Value;
                // загоняем id обьектов(блоков) в массив
                ObjectId[] idArray = selSet.GetObjectIds();
                // список для добавления имен кабельных линий
                List<string> stringCable = new List<string>();
                // список для сортировки по линкам 15-12-2023
               List <string> splitStr = new List<string>();
                // перебираем id блоков из массива
                foreach (ObjectId blkId in idArray)
                {
                    // берем блоки по id
                    BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);

                    // btr.Dispose();
                    AttributeCollection attCol = blkRef.AttributeCollection;
                    //  выводим координаты блока,слой и handle
                    foreach (ObjectId attId in attCol)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                        // сортируем с помощью линков
                        if (attRef.Tag == "ОБОЗНАЧ_КАБЕЛЯ")
                        {

                            string str = (attRef.TextString + "\n");
                            //ed.WriteMessage(str);
                            stringCable.Add(str);

                            // stringCable.Sort((n1) => n1.Split('-')[1].CompareTo(n1.Split('s')[1]));

                            foreach (var strList  in stringCable)
                            {
                                ed.WriteMessage(str);
                            }
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
