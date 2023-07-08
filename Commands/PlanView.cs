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
    public class PlanView :IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;


            ViewFamilyType familyType = new FilteredElementCollector(document).OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .First(x => x.ViewFamily == ViewFamily.FloorPlan);


            FilteredElementCollector collector = new FilteredElementCollector(document);

            Level level = collector.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType()
                .Cast<Level>().
                First(x => x.Name == "L1");

            try
            {
                using (Transaction transaction = new Transaction(document,"Making Plans"))
                {
                    transaction.Start();
                    ViewPlan vPlan = ViewPlan.Create(document,familyType.Id,level.Id);
                    vPlan.Name = "First View";
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
