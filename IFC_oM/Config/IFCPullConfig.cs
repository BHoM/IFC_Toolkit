using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.IFC
{
    public class IFCPullConfig : ActionConfig
    {
        //TODO: change it to PullGeometryConfig!
        public bool PullMeshes { get; set; } = false;
    }
}
