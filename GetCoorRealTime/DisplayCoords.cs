using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(ACADCommands.DisplayCoords))]

namespace ACADCommands
{
    public class DisplayCoords
    {
        [CommandMethod("CC")]

        public void CursorCoords()

        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            ed.PointMonitor += (s, e) =>
            {
                var ed2 = (Editor)s;
                if (ed2 == null) return;
                // If the call is just to set the last point, ignore
                if (e.Context.History == PointHistoryBits.LastPoint)
                    return;
                // Get the inverse of the current UCS matrix, to display in UCS
                var ucs = ed2.CurrentUserCoordinateSystem.Inverse();
                // Checked whether the point was snapped to
                var snapped = (e.Context.History & PointHistoryBits.ObjectSnapped) > 0;
                // Transform the snapped or computed point to the current UCS
                var pt =
                  (snapped ?
                    e.Context.ObjectSnappedPoint :
                    e.Context.ComputedPoint).TransformBy(ucs);
                // Display the point with each ordinate at 4 decimal places
                try
                {
                    ed2.WriteMessage("{0}: {1:F4}\n", snapped ? "Snapped" : "Found", pt);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    if (ex.ErrorStatus != ErrorStatus.NotApplicable)
                       throw;
                }
            };
        }
    }
}

