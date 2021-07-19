/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Adapters.IFC.Properties;
using BH.oM.Base;
using System.Collections.Generic;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.PropertyResource;

namespace BH.Adapter.IFC
{
    public static partial class Modify
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        public static void CopyIfcPropertiesToFragment(this IBHoMObject target, IfcObject from)
        {
            if (target == null || from == null)
                return;

            List<BH.oM.Adapters.IFC.Properties.IfcProperty> properties = new List<BH.oM.Adapters.IFC.Properties.IfcProperty>();
            
            foreach (IfcPropertySet propSet in from.PropertySets)
            {
                foreach (Xbim.Ifc2x3.PropertyResource.IfcProperty prop in propSet.HasProperties)
                {
                    IfcPropertySingleValue ifcValue = from.GetPropertySingleValue(propSet.Name, prop.Name);
                    properties.Add(new oM.Adapters.IFC.Properties.IfcProperty { Name = prop.Name, PropertySet = propSet.Name, Value = ifcValue.NominalValue?.Value });
                }
            }

            target.Fragments.Add(new IfcPulledProperties(properties));
        }

        /***************************************************/
    }
}