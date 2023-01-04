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
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Modify
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Attaches property to a BHoM object, which will be applied to a correspondent IFC element on Push.")]
        [Input("bHoMObject", "BHoMObject to which the property will be attached.")]
        [Input("propName", "Name of the property to be attached.")]
        [Input("value", "Value of the property to be attached.")]
        [Output("bHoMObject")]
        public static IBHoMObject SetIfcProperty(this IBHoMObject bHoMObject, string propName, object value)
        {
            if (bHoMObject == null)
                return null;

            List<IfcProperty> properties = new List<IfcProperty> { new IfcProperty { Name = propName, Value = value } };

            IfcPropertiesToPush existingFragment = bHoMObject.Fragments.FirstOrDefault(x => x is IfcPropertiesToPush) as IfcPropertiesToPush;
            if (existingFragment != null)
            {
                foreach (IfcProperty prop in existingFragment.Properties)
                {
                    if (prop.Name != propName)
                        properties.Add(prop);
                }
            }

            IfcPropertiesToPush fragment = new IfcPropertiesToPush { Properties = properties };

            IBHoMObject obj = bHoMObject.ShallowClone();
            obj.Fragments.AddOrReplace(fragment);

            // Warning to be removed once the support for units is added
            BH.Engine.Base.Compute.RecordWarning("Please not that IFC_Toolkit currently does not support units in property conversion - please be careful when working with dimensions etc.");

            return obj;
        }

        /***************************************************/
    }
}

