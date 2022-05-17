using System.ComponentModel;

namespace RestaurantCollection.WebApi.Models
{
    public enum GenericErrorCode
    {

        /// <summary>
        /// 1000 -  >  everything else
        /// </summary>
        [Description("Resquest body empty")]
        RequestBodyEmpty = 1001
    }
}
