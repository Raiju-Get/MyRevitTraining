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
    public class SetParameter:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document doc = uiDocument.Document;

            try
            {
                Reference pickObj = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                ElementId elementId = pickObj.ElementId;
                Element element = doc.GetElement(elementId);

                if (pickObj != null)
                {

                    Parameter eParameter = element.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);
                    TaskDialog.Show("Parameter", string.Format("parameter storage type {0} and the value {1}",
                        eParameter.StorageType.ToString(),
                        eParameter.AsDouble()));
                    using (Transaction transaction = new Transaction(doc, "Change Head height"))
                    {
                        transaction.Start();
                        eParameter.Set(10);
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
