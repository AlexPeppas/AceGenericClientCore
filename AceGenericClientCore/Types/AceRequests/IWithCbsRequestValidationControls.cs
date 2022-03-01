using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    /// <summary>
    ///Περιέχει πληροφορίες
    ///σχετικά με τη διαχείριση του
    ///flow μεταξύ του client
    ///(MyNBG UI) και του CBS
    ///(ACE)
    /// </summary>
    public interface IWithCbsRequestValidationControls
    {
        RequestValidationControls ValidationControlsRequest { get; set; }
    }
}
