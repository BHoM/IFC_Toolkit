/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.IFC;
using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        [Description("Returns either the input settings if they are not null, or the default settings.")]
        [Input("settings", "Input settings to be replaced with defaults in case they are null.")]
        [Output("settings", "Input settings replaced with defaults in case they were null.")]
        public static IfcSettings DefaultIfNull(this IfcSettings settings)
        {
            if (settings == null)
            {
                settings = new IfcSettings();
                BH.Engine.Base.Compute.RecordNote("Settings have not been specified, default settings are used.");
            }

            return settings;
        }

        /***************************************************/
    }
}



