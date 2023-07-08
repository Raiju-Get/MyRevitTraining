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
    public class ViewFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

           List<ElementId> catElementIds = new List<ElementId>();
           catElementIds.Add(new ElementId(BuiltInCategory.OST_Sections));

           ElementParameterFilter filter =
               new ElementParameterFilter(ParameterFilterRuleFactory.CreateContainsRule(new ElementId(BuiltInParameter.VIEW_NAME),"WIP",false));

            try
            {
                using (Transaction transaction = new Transaction(document,"View Filters") )
                {
                    transaction.Start();

                    ParameterFilterElement filterElement = ParameterFilterElement.Create(document,"WIP Filter", catElementIds,filter);
                    document.ActiveView.AddFilter(filterElement.Id);
                    document.ActiveView.SetFilterVisibility(filterElement.Id,false);
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
