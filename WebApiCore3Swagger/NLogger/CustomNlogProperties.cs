using NLog;
using System;

namespace WebApiCore3Swagger.NLogger
{
    public class CustomNlogProperties : ICustomNlogProperties
    {
        private Logger loggerManager;
        public CustomNlogProperties()
        {
            loggerManager = LogManager.GetCurrentClassLogger();
        }
        public void LogProperty(CustomProperty property)
        {
            LogEventInfo logEvent = new LogEventInfo(property.Level, property.LoggerName, property.Message);
            SetCustomPropertyData(logEvent, property);
            loggerManager.Log(logEvent);
        }

        private void SetCustomPropertyData(LogEventInfo logEvent, CustomProperty customProperty )
        {
            logEvent.Properties["UserName"] = customProperty.UserName;
        }
    }
}
