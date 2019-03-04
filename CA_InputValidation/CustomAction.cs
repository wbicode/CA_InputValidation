using Microsoft.Deployment.WindowsInstaller;
using System.Net.NetworkInformation;

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
        public static ActionResult CheckPort(Session session)
        {
            session.Log("Begin CheckPort");

            int port;
            if (!int.TryParse(session["PORT_TO_CHECK"], out port))
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
            session["PORT_IS_AVAILABLE"] = isAvailable.ToString();

            return ActionResult.Success;
        }
    }
}
