using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitTraining
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetElementById:IExternalCommand
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

                ElementId eTypeId = ele.GetTypeId();

                ElementType eType  = doc.GetElement(eTypeId) as ElementType;

                if (pickObj !=null)
                {
                    TaskDialog.Show("Element Classification: ", eTypeId.ToString() + Environment.NewLine
                    +"Category:" + ele.Category.Name + Environment.NewLine
                    + "Instance:" + ele.Name+ Environment.NewLine
                    + "Symbol: " + eType.Name  +Environment.NewLine
                    + "Family: " + eType.FamilyName);
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
