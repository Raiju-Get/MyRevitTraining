using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace MyRevitTraining
{
    public class ExternalApplication :IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("Personal Commands");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitTraining.PlaceFamily");

            RibbonPanel panel = application.CreateRibbonPanel("Personal Commands", "Commands");

            Uri imagePath = new Uri(@"D:\JP\MyRevitTraining\Images\icons\Desk.png");

            BitmapImage image = new BitmapImage(imagePath);

            PushButton pushButton =    panel.AddItem(button) as PushButton;

            pushButton.LargeImage = image;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
