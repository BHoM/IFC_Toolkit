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
    public static partial class Query
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/
        
        [Description("Retrieves IFC properties that are attached to a BHoM object. If a property with given name exists in both collections of pulled properties and the ones to push, the latter is returned.")]
        [Input("bHoMObject", "BHoMObject to which the properties will be attached.")]
        [Output("ifcProperties")]
        public static List<IfcProperty> GetIfcProperties(this IBHoMObject bHoMObject)
        {
            if (bHoMObject == null)
                return null;

            // Warning to be removed once the support for units is added
            BH.Engine.Base.Compute.RecordWarning("Please note that IFC_Toolkit currently does not support units in property conversion - please be careful when working with dimensions etc.");

            IfcPulledProperties pullFragment = bHoMObject.FindFragment<IfcPulledProperties>();
            IfcPropertiesToPush pushFragment = bHoMObject.FindFragment<IfcPropertiesToPush>();

            List<IfcProperty> result = new List<IfcProperty>();
            if (pullFragment?.Properties != null)
                result.AddRange(pullFragment.Properties);

            if (pushFragment?.Properties != null)
            {
                bool mixed = false;
                foreach (IfcProperty prop in pushFragment.Properties)
                {
                    int index = result.FindIndex(x => x.Name == prop.Name);
                    if (index == -1)
                        result.Add(prop);
                    else
                    {
                        mixed = true;
                        result.RemoveAt(index);
                        result.Add(prop);
                    }
                }

                if (mixed)
                    BH.Engine.Base.Compute.RecordNote("Some of the properties were retrieved from collection of pulled ones, some from the ones meant to be pushed.");
            }

            return result;
        }

        /***************************************************/
    }
}




