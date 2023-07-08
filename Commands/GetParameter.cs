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
    public class GetParameter:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document doc = uiDocument.Document;


            try
            {
                Reference pickObj = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (pickObj != null)
                {
                    ElementId eleId = pickObj.ElementId;
                    Element ele = doc.GetElement(eleId);

                    Parameter parameter = ele.LookupParameter("Head Height");
                    InternalDefinition parameterDef = parameter.Definition as InternalDefinition;



                    if (parameterDef != null)
                    {
                        TaskDialog.Show("parameter", string.Format("{0} parameter of type {1} with builtinParameter{2}",
                            parameter.Definition.Name,
                            parameterDef.GetParameterTypeId().TypeId,
                            parameterDef.BuiltInParameter));
                    }
                    else
                    {
                        TaskDialog.Show("parameter", "This is empty");
                    }

                    
                }
                else
                {
                    TaskDialog.Show("parameter", "This is Null");
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
