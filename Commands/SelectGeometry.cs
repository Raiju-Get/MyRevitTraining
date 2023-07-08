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
    public class SelectGeometry :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            try
            {
                Reference picReference =
                    uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (picReference != null)
                {
                    ElementId elementId = picReference.ElementId;
                    Element element = document.GetElement(elementId);

                    Options gOptions = new Options();
                    gOptions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement gElement = element.get_Geometry(gOptions);

                    foreach (var gItem in gElement)
                    {
                        Solid gSolid = gItem as Solid;
                        int faces = 0;
                        double area = 0;
                        foreach (Face gFaces in gSolid.Faces)
                        {
                            area += gFaces.Area;
                            faces++;
                        }

                        double areMetric = UnitUtils.ConvertFromInternalUnits(area, UnitTypeId.SquareMeters);
                        TaskDialog.Show("Show Area", string.Format("The Element has {0} walls and the area is {1}", faces, areMetric));
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
