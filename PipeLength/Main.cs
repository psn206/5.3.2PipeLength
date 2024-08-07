using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Plumbing;

namespace PipeLength
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            double sumLength = 0;

            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Reference> selectElements = uiDoc.Selection.PickObjects(ObjectType.Element, "Выберете трубы");

                foreach (var SElement in selectElements)
                {
                    var element = doc.GetElement(SElement);
                    if (element is Pipe)
                    {
                        Parameter pipeLength = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                        double metrePipeLength = UnitUtils.ConvertFromInternalUnits(pipeLength.AsDouble(), UnitTypeId.Meters);
                        sumLength = sumLength + metrePipeLength;
                    }
                }
                TaskDialog.Show("Длинна труб", sumLength.ToString());
                return Result.Succeeded;
            }
            catch
            {
                TaskDialog.Show("Длинна труб", "Не посчитана");
                return Result.Succeeded;
            }
        }
    }
}
