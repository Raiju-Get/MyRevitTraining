using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitTraining
{
    [Transaction(TransactionMode.Manual)]
    public class CollectWindows :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(doc);

                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

                IList<Element> windows =   collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

                TaskDialog.Show("Number of Windows", string.Format("{0} windows counted", windows.Count));

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
