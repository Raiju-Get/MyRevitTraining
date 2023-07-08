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
    public class PlaceLoopElement:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document doc = uiDocument.Document;

            ElementId floorytypeId = Floor.GetDefaultFloorType(doc, true);

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            Level level = collector.OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .First(x => x.Name == "L1");

            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(15, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            List<Curve> curves = new List<Curve>();

            Line l1 = Line.CreateBound(p1,p2);
            Arc l2 = Arc.Create(p2,p4,p3);
            Line l3 = Line.CreateBound(p4,p5);
            Line l4 = Line.CreateBound(p5,p1);

            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            CurveLoop curveLoop = CurveLoop.Create(curves);
            CurveLoop offsetCurveLoop = CurveLoop.CreateViaOffset(curveLoop, 0.300, new XYZ(0, 0, 1));

            IList<CurveLoop> loop = new List<CurveLoop>(){ offsetCurveLoop };

            try
            {
                using (Transaction transaction = new Transaction( doc , "Placing A floor"))
                {
                    transaction.Start();

                    Floor.Create(doc,loop, floorytypeId,level.Id);

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
