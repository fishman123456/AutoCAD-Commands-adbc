using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace ACADCommands
{
        public class Commands

        {
            [CommandMethod("OBL", CommandFlags.UsePickSet)]
            public static void ObjectsByLayer()
            {
            CheckDateWork.CheckDate();
            var activeDocument = Application.DocumentManager.MdiActiveDocument;
                if (activeDocument == null) return;
                var dokEditor = activeDocument.Editor;
                // Select the objects to sort
                var psr = dokEditor.GetSelection();
                if (psr.Status != PromptStatus.OK)
                    return;
                // We'll sort them based on a string value (the layer name)
                var dictObj = new Dictionary<ObjectId, string>();
                foreach (dynamic id in psr.Value.GetObjectIds())
                {
                    dictObj.Add(id, id.Layer);
                }
                var sorted = dictObj.OrderBy(kv => kv.Value);
                // Print them in order to the command-line
                foreach (var item in sorted)
                {
                    dokEditor.WriteMessage("\nObject {0} on layer {1}", item.Key, item.Value);
                }
            }

            [CommandMethod("OBT", CommandFlags.UsePickSet)]
            public static void ObjectsByType()
            {
            CheckDateWork.CheckDate();
            var activeDocument = Application.DocumentManager.MdiActiveDocument;
            if (activeDocument == null) return;
            var dokEditor = activeDocument.Editor;
            // Select the objects to sort
            var psr = dokEditor.GetSelection();
            if (psr.Status != PromptStatus.OK)
                return;
            // We'll sort them based on a string value (the layer name)
            var dictObj = new Dictionary<ObjectId, string>();
            foreach (dynamic id in psr.Value.GetObjectIds())
            {
                dictObj.Add(id, id.ObjectClass.Name);
            }
            var sorted = dictObj.OrderBy(kv => kv.Value);
            // Print them in order to the command-line
            foreach (var item in sorted)
            {
                dokEditor.WriteMessage("\nObject {0} on layer {1}", item.Key, item.Value);
            }
        }
        }
    }


