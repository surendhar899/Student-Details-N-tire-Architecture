using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Student_Details_N_tire.Filter
{
    public class MyexceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    { 
        public override void OnException(ExceptionContext context)
        {
           var exceptionMessage=context.Exception.Message;  
            var stackTrace=context.Exception.StackTrace;
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var Message = "Date :" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "Error Message:" + exceptionMessage + Environment.NewLine + "Stack Trace:" + stackTrace;
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string logFilePath = @"C:\Users\surendhar.duraisamy\source\repos\Student Details N-tire\Log\";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
            logFileInfo=new FileInfo(logFilePath);
            logDirInfo=new DirectoryInfo(logFileInfo.DirectoryName);
            if(!logDirInfo.Exists)
                logDirInfo.Create();
            if(!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();

            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);

            }
            log=new StreamWriter(fileStream);
            log.WriteLine("===========================START================================");
            log.WriteLine("Log Create At-" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "Controller :"+controllerName+",Action:"+actionName+",Message:-" + Message);
            log.WriteLine("===========================END================================");
            log.Close();
            context.Result = new RedirectResult("/Home/Error");
            
            base.OnException(context);
        }
    }
}
