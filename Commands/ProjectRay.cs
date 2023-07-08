using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace MyRevitTraining
{
    [Transaction(TransactionMode.Manual )]
    public class ProjectRay :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            try
            {
                Reference pickObj = uiDocument.Selection.PickObject(ObjectType.Element);


                if (pickObj != null)
                {
                    ElementId elementId = pickObj.ElementId;
                    Element element = document.GetElement(elementId);

                    LocationPoint locoPoint = element.Location as LocationPoint;

                    XYZ p1 = locoPoint.Point;

                    XYZ rayZ = new XYZ(0,0,1);

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
                    ReferenceIntersector RefInst = new ReferenceIntersector(filter,FindReferenceTarget.Face,(View3D)document.ActiveView);
                    ReferenceWithContext refCon = RefInst.FindNearest(p1, rayZ);
                    Reference reference = refCon.GetReference();
                    XYZ intPoint = reference.GlobalPoint;
                    double dist = p1.DistanceTo(intPoint);

                    TaskDialog.Show("The Distance", string.Format("{0} is the distance between the points", dist));

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
