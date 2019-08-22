using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTTV_Revit
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if(commandData.Application.ActiveUIDocument==null)
            {
                TaskDialog.Show("Project missing", "Please open project first");
                return Result.Cancelled;
            }
            var win = new OTTV.MainWindow();
            win.Show();

            return Result.Cancelled;
        }
    }
}
