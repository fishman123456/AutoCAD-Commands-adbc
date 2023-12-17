using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Db =Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Ed = Autodesk.AutoCAD.EditorInput;
using System.Globalization;


// Метод который проходится по списку паттернов блоков и
/// заполняет ВСЕ поля и свойства в объекте block
/// </summary>
/// <param name="acCurDb">Текущщая база чертежа</param>
/// <param name="BlockID">Db.ObjectId выбранного пользователем блока</param>
/// 
// Эта строка не является обязательной, но улучшает производительность загрузки
[assembly: CommandClass(typeof(ACADCommands.GetAttr))]


namespace ACADCommands
{
    public class GetAttr
    {
        [CommandMethod("BlockAtrr")]
      public static void GetAttrVoid()
        {
            CheckDateWork.CheckDate();
            App.Document acDoc = App.Application.DocumentManager.MdiActiveDocument;
            Db.Database acCurDb = acDoc.Database;
            Ed.Editor acEd = acDoc.Editor;

            Ed.PromptEntityOptions proO = new Ed.PromptEntityOptions("\n Выбери блок");
            Ed.PromptEntityResult proR = acEd.GetEntity(proO);
            using (Db.Transaction acTrans = acCurDb.TransactionManager.StartOpenCloseTransaction())
            {
                Db.BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, Db.OpenMode.ForRead) as Db.BlockTable;
                Db.BlockReference blockReferens = acTrans.GetObject(proR.ObjectId, Db.OpenMode.ForRead) as Db.BlockReference;
                Db.BlockTableRecord blockTable = acTrans.GetObject(acBlkTbl[blockReferens.Name], Db.OpenMode.ForRead) as Db.BlockTableRecord;
                // если блок содержит аттрибуты
                if (blockTable.HasAttributeDefinitions)
                {
                    // делаем linq запрос по наличию тега "ОБОЗНАЧ_КАБЕЛЯ"
                    Db.AttributeReference blockAttr = ((from Db.ObjectId attribRefId in blockReferens.AttributeCollection
                                                        let attribRef = (Db.AttributeReference)acTrans.GetObject(attribRefId, Db.OpenMode.ForRead)
                                                        where attribRef.Tag.Equals("ОБОЗНАЧ_КАБЕЛЯ", StringComparison.OrdinalIgnoreCase)
                                                        select attribRef).FirstOrDefault()) as Db.AttributeReference;
                    // список для собор аттрибутов из блока
                    List <string> attrContent = new List <string>();
                    attrContent.Add(blockReferens.Name );
                    List<Db.AttributeReference> attrC = new List<Db.AttributeReference>();
                   



                    string attr = blockAttr.Layer.ToString();
                }
            };
        }
    }
}
