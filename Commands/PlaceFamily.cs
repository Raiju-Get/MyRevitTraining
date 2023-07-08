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

    public class PlaceFamily:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document doc = uiDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            //600 x 900mm
            FamilySymbol symbol = collector.OfClass(typeof(FamilySymbol))
                .WhereElementIsElementType()
                .Cast<FamilySymbol>()
                .First(x => x.Name == "1525 x 762mm");

           


            try
            {
                using (Transaction transaction = new Transaction(doc, "Placing Family"))
                {
                    transaction.Start();

                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                    }
                  

                    TaskDialog tDwag = new TaskDialog("Place Family");

                    tDwag.MainContent = "Are you sure you wanna place a family?";
                    tDwag.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol,
                        Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                    if (tDwag.Show()== TaskDialogResult.Ok)
                    {
                        transaction.Commit();
                        TaskDialog.Show("The Item as been ", "Placed");
                    }
                    else
                    {
                        transaction.RollBack();
                        TaskDialog.Show("The Item as not been ", "Placed");
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
