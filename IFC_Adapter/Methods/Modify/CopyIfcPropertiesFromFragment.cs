/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using BH.Engine.Base;
using BH.oM.Adapters.IFC.Properties;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PropertyResource;

namespace BH.Adapter.IFC
{
    public static partial class Modify
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        public static void CopyIfcPropertiesFromFragment(this IfcObject target, IBHoMObject source)
        {
            if (target == null || source == null)
                return;

            IList<BH.oM.Adapters.IFC.Properties.IfcProperty> propertiesToPush = source.FindFragment<IfcPropertiesToPush>()?.Properties;
            if (propertiesToPush != null)
            {
                foreach (BH.oM.Adapters.IFC.Properties.IfcProperty prop in propertiesToPush)
                {
                    bool set = false;
                    foreach (IfcPropertySet propSet in target.PropertySets)
                    {
                        foreach (Xbim.Ifc2x3.PropertyResource.IfcProperty ifcProp in propSet.HasProperties)
                        {
                            if (ifcProp.Name == prop.Name)
                            {
                                IfcPropertySingleValue ifcValue = target.GetPropertySingleValue(propSet.Name, prop.Name);

                                //TODO: This looks a bit like a hammer solution, could possibly create a method with proper type mapping.
                                // Alternatively (possibly even better), try making use of implicit casting operators declared in each type that implements IfcValue
                                IfcValue newValue = null;
                                try
                                {
                                    Type t = ifcValue.NominalValue.GetType();
                                    newValue = Activator.CreateInstance(t, new object[] { prop.Value }) as IfcValue;
                                }
                                catch
                                {
                                    BH.Engine.Base.Compute.RecordError($"Property named {prop.Name} could not be set because the provided value has wrong type.\nElement name: {target.Name}, Element Id: {target.GlobalId}.");
                                    continue;
                                }
                                target.SetPropertySingleValue(propSet.Name, prop.Name, newValue);

                                if (set == true)
                                    BH.Engine.Base.Compute.RecordWarning($"Ifc element has more than one property named {prop.Name}, all of them have been updated.\nElement name: {target.Name}, Element Id: {target.GlobalId}.");

                                set = true;
                            }
                        }
                    }

                    if (set == false)
                        BH.Engine.Base.Compute.RecordWarning($"Ifc element does not own any property named {prop.Name}, so it has not been updated.\nElement name: {target.Name}, Element Id: {target.GlobalId}.");
                }
            }
        }

        /***************************************************/
    }
}

