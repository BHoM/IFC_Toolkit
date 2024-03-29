/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Base;
using System.Collections.Generic;
using Xbim.Ifc2x3.Interfaces;

namespace BH.Adapter.IFC
{
    public static partial class Convert
    {
        /***************************************************/
        /****             Interface Methods             ****/
        /***************************************************/

        public static IEnumerable<IBHoMObject> IFromIfc(this IIfcProduct element, Discipline discipline, IfcSettings settings = null)
        {
            IEnumerable<IBHoMObject> result = FromIfc(element as dynamic, discipline, settings);
            if (result == null)
            {
                BH.Engine.Base.Compute.RecordError($"IFC element conversion to BHoM failed for discipline {discipline}. A CustomObject is returned instead.");
                return new List<IBHoMObject> { new CustomObject { Name = element.Name } };
            }

            // Copy identifiers
            foreach (IBHoMObject obj in result)
            {
                obj.CopyIdentifiersToFragment(element);
            }

            return result;
        }


        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static IEnumerable<IBHoMObject> FromIfc(this IIfcSlab element, Discipline discipline, IfcSettings settings)
        {
            switch (discipline)
            {
                default:
                    return new List<IBHoMObject> { element.FloorFromIfc(settings) };
            }
        }

        /***************************************************/

        public static IEnumerable<IBHoMObject> FromIfc(this IIfcReinforcingBar element, Discipline discipline, IfcSettings settings)
        {
            switch (discipline)
            {
                default:
                    return new List<IBHoMObject> { element.ReinforcingBarFromIfc(settings) };
            }
        }

        /***************************************************/

        public static IEnumerable<IBHoMObject> FromIfc(this IIfcSpace element, Discipline discipline, IfcSettings settings)
        {
            switch (discipline)
            {
                default:
                    return new List<IBHoMObject> { element.SpaceFromIfc(settings) };
            }
        }

        /***************************************************/
    }
}





