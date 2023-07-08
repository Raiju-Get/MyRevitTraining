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
    public class ChangeLocation:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                ElementId eleId = pickObj.ElementId;

                Element ele = doc.GetElement(eleId);

                using (Transaction transaction = new Transaction( doc , "Changing location"))
                {
                    LocationPoint location = ele.Location as LocationPoint;

                    if (location != null)
                    {
                        transaction.Start();
                        XYZ locationPoint = location.Point;
                        XYZ newLocation = new XYZ(locationPoint.X + 3, locationPoint.Y, locationPoint.Z);

                        location.Point = newLocation;


                        transaction.Commit();
                    }
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
