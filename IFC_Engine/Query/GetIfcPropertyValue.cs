﻿/*
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
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using BH.Engine.Base;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Retrieves value of a property attached to a BHoM object. If a property with given name exists in both collections of pulled properties and the ones to push, the latter is returned.")]
        [Input("bHoMObject", "BHoMObject to which the properties will be attached.")]
        [Input("propertyName", "Name of the property to be sought for.")]
        [Output("value")]
        public static object GetIfcPropertyValue(this IBHoMObject bHoMObject, string propertyName)
        {
            if (bHoMObject == null)
                return null;

            Compute.UnsupportedUnitsWarning();

            IfcPropertiesToPush pushFragment = bHoMObject.FindFragment<IfcPropertiesToPush>();
            if (pushFragment?.Properties != null)
            {
                IfcProperty prop = pushFragment.Properties.FirstOrDefault(x => x.Name == propertyName);
                if (prop != null)
                    return prop.Value;
            }

            IfcPulledProperties pullFragment = bHoMObject.FindFragment<IfcPulledProperties>();
            if (pullFragment?.Properties != null)
            {
                IfcProperty prop = pullFragment.Properties.FirstOrDefault(x => x.Name == propertyName);
                if (prop != null)
                    return prop.Value;
            }
            
            Dictionary<string, object> bHoMPropDic = Reflection.Query.PropertyDictionary(bHoMObject);
            foreach (KeyValuePair<string, object> bHoMPropEntry in bHoMPropDic)
            {
                IBHoMObject bHoMProp = bHoMPropEntry.Value as IBHoMObject;
                if (bHoMProp != null)
                {
                    IfcPulledProperties typePullFragment = bHoMProp.FindFragment<IfcPulledProperties>();
                    if (typePullFragment?.Properties != null)
                    {
                        IfcProperty prop = typePullFragment.Properties.FirstOrDefault(x => x.Name == propertyName);
                        if (prop != null)
                        {
                            Engine.Reflection.Compute.RecordWarning("The value for property " + propertyName + " for the object with BHoM_Guid " + bHoMObject.BHoM_Guid + " has been retrieved from its property " + bHoMPropEntry.Key + ".");
                            return prop.Value;
                        }
                    }
                }
            }

            return null;
        }

        /***************************************************/
    }
}


