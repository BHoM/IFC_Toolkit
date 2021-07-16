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

using BH.Engine.Adapters.IFC;
using BH.oM.Adapters.IFC;
using BH.oM.Base;
using BH.oM.Physical.Elements;
using System.Collections.Generic;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.PropertyResource;

namespace BH.Adapter.IFC
{
    public static partial class Convert
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static Floor FloorFromIfc(this IIfcSlab element, IfcSettings settings)
        {
            if (element == null)
            {
                BH.Engine.Reflection.Compute.RecordError("The IFC element could not be converted because it was null.");
                return null;
            }

            IfcObject ifcObject = element as IfcObject;
            if (ifcObject == null)
            {
                BH.Engine.Reflection.Compute.RecordError("The IFC element could not be read due to an internal error.");
                return null;
            }

            settings = settings.DefaultIfNull();
            
            //TODO: refine this!
            Floor floor = new Floor { Name = element.Name };
            floor.CopyParameters(ifcObject);
            return floor;
        }

        /***************************************************/
    }
}


