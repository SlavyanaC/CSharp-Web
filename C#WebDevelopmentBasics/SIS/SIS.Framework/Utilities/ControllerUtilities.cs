namespace SIS.Framework.Utilities
{
    using Controllers;

    public static class ControllerUtilities
    {
        public static string GetControllerName(Controller controller)
        {
            return controller.GetType()
                .Name
                .Replace(MvcContext.Get.ControllerSuffix, string.Empty);
        }

        public static string GetViewFullyQualifiedName(string controller, string action)
        {
            return string.Format("..\\..\\..\\{0}\\{1}\\{2}.html",
                MvcContext.Get.ViewsFolder,
                controller,
                action);
        }
    }
}
