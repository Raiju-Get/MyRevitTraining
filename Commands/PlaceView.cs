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
    public class PlaceView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            ViewSheet sheet = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .Cast<ViewSheet>().First(x => x.Name == "First Sheet");

            Element vPlaim = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_Views)
                .First(x => x.Name == "First View");

            BoundingBoxUV outline = sheet.Outline;

            double xu = (outline.Max.U + outline.Min.U) /2;
            double yv = (outline.Max.V + outline.Min.V) /2;

            XYZ midpoint = new XYZ(yv, yv, 0);

            try
            {
                using (Transaction transaction  = new Transaction(document,"Place View"))
                {
                    transaction.Start();

                   Viewport vPort = Viewport.Create(document, sheet.Id,vPlaim.Id,midpoint);

                    transaction.Commit();
                }

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
