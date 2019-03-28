using Microsoft.Deployment.WindowsInstaller;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;

namespace CA_InputValidation
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult ReplaceBackslashWithFrontslash(Session session)
        {
            session.Log("Begin ReplaceBackslashWithFrontslash");

            string propertyToReplace = session["PROPERTY_TO_REPLACE_BACKSLASH"];

            session[propertyToReplace] = session[propertyToReplace].Replace("\\", "/");

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ReplaceFrontslashWithBackslash(Session session)
        {
            session.Log("Begin ReplaceFrontslashWithBackslash");

            string propertyToReplace = session["PROPERTY_TO_REPLACE_FRONTSLASH"];

            session[propertyToReplace] = session[propertyToReplace].Replace("/", "\\");

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult CheckPort(Session session)
        {
            session.Log("Begin CheckPort");

            if (!int.TryParse(session["PORT_TO_CHECK"], out int port))
            {
                return ActionResult.NotExecuted;
            }
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();

            foreach (var listener in tcpListeners)
            {
                if (listener.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }
            session["PORT_IS_AVAILABLE"] = (isAvailable ? "1" : "");

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ExistsService(Session session)
        {
            ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == session["CA_IV_SERVICE_NAME"]);
            if (ctl == null)
            {
                session["CA_IV_SERVICE_EXISTS"] = "";
            }
            else
            {
                session["CA_IV_SERVICE_EXISTS"] = "1";
            }

            return ActionResult.Success;
        }
    }
}
