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
    public class EditElement :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;


            try
            {
                Reference pickObj = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                ElementId eleId = pickObj.ElementId;
                Element element = document.GetElement(eleId);


                using (Transaction transaction = new Transaction(document, "Editing Elements"))
                { 

                    LocationPoint location = element.Location as LocationPoint;
                    if (location != null)
                    {
                        transaction.Start();
                        XYZ movePoint = new XYZ(3,3,0);
                        ElementTransformUtils.MoveElement(document,eleId, movePoint );

                        XYZ p1 = location.Point;
                        XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);
                        Line axis = Line.CreateBound(p1,p2);

                        double angle = 30 * Math.PI / 180;
                        ElementTransformUtils.RotateElement(document,eleId,axis, angle);


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
