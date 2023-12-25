using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(ACADCommands.ClassLinetype))]

namespace ACADCommands
{
    internal class ClassLinetype
    {
        [CommandMethod("SLT", CommandFlags.UsePickSet)]

        public void SetLineType()

        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
                return;
            var ed = doc.Editor;
            // Get the pickfirst selection set or ask the user to
            // select some entities
            var psr = ed.GetSelection();
            if (psr.Status != PromptStatus.OK || psr.Value.Count == 0)
                return;
            using (var tr = doc.TransactionManager.StartTransaction())
            {
                // Get the IDs of the selected objects
                var ids = psr.Value.GetObjectIds();
                // Loop through in read-only mode, checking whether the
                // selected entities have the same linetype
                // (if so, it'll be set in ltId, otherwise different will
                // be true)
                var ltId = ObjectId.Null;
                bool different = false;
                foreach (ObjectId id in ids)
                {
                    // Get the entity for read
                    var ent = (Entity)tr.GetObject(id, OpenMode.ForRead);
                    // On the first iteration we store the linetype Id
                    if (ltId == ObjectId.Null)
                        ltId = ent.LinetypeId;
                    else
                    {
                        // On subsequent iterations we check against the
                        // first one and set different to be true if they're
                        // not the same
                        if (ltId != ent.LinetypeId)
                        {
                            different = true;
                            break;
                        }
                    }
                }
                // Now we can display our linetype dialog with the common
                // linetype selected (if they have the same one)
                var ltd = new LinetypeDialog();
                if (!different)
                    ltd.Linetype = ltId;
                var dr = ltd.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return; // We might also commit before returning
                // Assuming we have a different linetype selected
                // (or the entities in the selected have different
                // linetypes to start with) then we'll loop through
                // to set the new linetype
                if (different || ltId != ltd.Linetype)
                {
                    foreach (ObjectId id in ids)
                    {
                        // This time we need write access
                        var ent = (Entity)tr.GetObject(id, OpenMode.ForWrite);
                        // Set the linetype if it's not the same
                        if (ent.LinetypeId != ltd.Linetype)
                            ent.LinetypeId = ltd.Linetype;
                    }
                }
                // Finally we commit the transaction
                tr.Commit();
            }
        }
    }
}
    

