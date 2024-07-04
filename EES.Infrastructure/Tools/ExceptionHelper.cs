using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Tools
{
    public class ExceptionHelper
    {
       public static string GetCompleteErrorMessage(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            string errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += Environment.NewLine + GetCompleteErrorMessage(ex.InnerException);
            }

            return errorMessage;
        }
      
    }
}
