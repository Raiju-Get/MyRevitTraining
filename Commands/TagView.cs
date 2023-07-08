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
    public class TagView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagOrientation = TagOrientation.Horizontal;

            List<BuiltInCategory> categories = new List<BuiltInCategory>();
            categories.Add(BuiltInCategory.OST_Windows);
            categories.Add(BuiltInCategory.OST_Doors);

            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(categories);



            IList<Element> tElements = new FilteredElementCollector(document, document.ActiveView.Id)
                .WherePasses(filter).WhereElementIsNotElementType().ToElements();

            try
            {
                using (Transaction transaction = new Transaction( document , "Tagging Windows and Doors"))
                {
                    transaction.Start();
                    foreach (var ele in tElements)
                    {
                        Reference reference = new Reference(ele);
                        LocationPoint loc = ele.Location as LocationPoint;
                        XYZ point = loc.Point;

                        IndependentTag tag = IndependentTag.Create(document,document.ActiveView.Id,reference,true,tagMode,tagOrientation,point);
                    }

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
