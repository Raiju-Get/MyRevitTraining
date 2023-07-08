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
    public class DeleteElement:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                ElementId eleId = pickObj.ElementId;

                if (pickObj != null)
                {
                    using (Transaction trans = new Transaction(doc , "Delete Elements"))
                    {
                        trans.Start();
                        doc.Delete(eleId);
                        TaskDialog taskDialog = new TaskDialog("Delete Element");
                        taskDialog.MainContent = "Are you sure you want to delete this element?";
                        taskDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if (taskDialog.Show() == TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Delete", pickObj.ElementId.ToString() + "was Deleted");
                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Delete", pickObj.ElementId.ToString() + "was not Deleted");
                        }
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
