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
    public class ElementIntersections :IExternalCommand
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

                    Options gOPtions = new Options();
                    gOPtions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement gElement = element.get_Geometry(gOPtions);

                    Solid gSolid = null;

                    foreach (GeometryObject gObj in gElement)
                    {
                        GeometryInstance gInstan = gObj as GeometryInstance;

                        if (gInstan != null)
                        {
                            GeometryElement geometryElement = gInstan.GetInstanceGeometry();

                            foreach (GeometryObject item in geometryElement)
                            {
                                gSolid = item as Solid;
                                
                            }
                        }
                    }

                    FilteredElementCollector collector = new FilteredElementCollector(document);
                    ElementIntersectsSolidFilter filter = new ElementIntersectsSolidFilter(gSolid);

                    ICollection<ElementId> intersection = collector.OfCategory(BuiltInCategory.OST_Roofs).WherePasses(filter).ToElementIds();

                    TaskDialog.Show("Revit", intersection.Count + " family instances intersect with the selected element (" + element.Category.Name + " id:" + element.Id.ToString() + ")");
                    uiDocument.Selection.SetElementIds(intersection);
                   
                    uiDocument.ShowElements(intersection);
                    uiDocument.RefreshActiveView();


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
