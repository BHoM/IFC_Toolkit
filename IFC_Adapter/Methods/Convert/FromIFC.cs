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

        public static IEnumerable<IBHoMObject> IFromIFC(this IIfcElement element, Discipline discipline, IFCSettings settings = null)
        {
            IEnumerable<IBHoMObject> result = FromIFC(element as dynamic, discipline, settings);
            if (result == null)
                BH.Engine.Reflection.Compute.RecordError($"IFC element conversion to BHoM failed for discipline {discipline}.");

            return result;
        }


        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static IEnumerable<IBHoMObject> FromIFC(this IIfcSlab element, Discipline discipline, IFCSettings settings)
        {
            switch (discipline)
            {
                default:
                    return new List<IBHoMObject> { element.FloorFromIFC(settings) };
            }
        }

        /***************************************************/

        public static IEnumerable<IBHoMObject> FromIFC(this IIfcReinforcingBar element, Discipline discipline, IFCSettings settings)
        {
            switch (discipline)
            {
                default:
                    return new List<IBHoMObject> { element.ReinforcingBarFromIFC(settings) };
            }
        }

        /***************************************************/
    }
}

