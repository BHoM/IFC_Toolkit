using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.IFC
{
    public class IFCRepresentation : IFragment, IImmutable
    {
        //[Description("Meshed surfaces of Revit element represented by the BHoM object carrying this fragment.")]
        public virtual ReadOnlyCollection<Mesh> Meshes { get; } = null;


        /***************************************************/
        /****            Public Constructors            ****/
        /***************************************************/

        public IFCRepresentation(IEnumerable<Mesh> meshes)
        {
            Meshes = meshes == null ? null : new ReadOnlyCollection<Mesh>(meshes.ToList());
        }

        /***************************************************/
    }
}
