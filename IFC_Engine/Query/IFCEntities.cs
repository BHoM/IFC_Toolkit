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

using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Physical.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****             Interface Methods             ****/
        /***************************************************/

        public static List<IPersistEntity> IIFCEntities(this Xbim.Ifc.IfcStore model, IRequest request)
        {
            return IFCEntities(model, request as dynamic);
        }


        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static List<IPersistEntity> IFCEntities(this Xbim.Ifc.IfcStore model, FilterRequest request)
        {
            if (model == null || request?.Type == null)
                return null;
            
            if (!typeof(IBHoMObject).IsAssignableFrom(request.Type))
            {
                BH.Engine.Reflection.Compute.RecordError($"Input type {request.Type} is not a BHoM type.");
                return null;
            }

            if (request.Type == typeof(IBHoMObject) || request.Type == typeof(BHoMObject) || request.Type == typeof(IMaterialProperties))
            {
                BH.Engine.Reflection.Compute.RecordError($"It is not allowed to pull elements of type {request.Type} because it is too general, please try to narrow the filter down.");
                return null;
            }

            IEnumerable<Type> correspondentTypes = request.Type.CorrespondentIFCTypes();

            return model.Instances.Where(x => correspondentTypes.Any(y => y.IsAssignableFrom(x.GetType()))).ToList();
        }


        /***************************************************/
        /****             Fallback Methods              ****/
        /***************************************************/

        public static List<IPersistEntity> IFCEntities(this Xbim.Ifc.IfcStore model, IRequest request)
        {
            BH.Engine.Reflection.Compute.RecordError($"Request of type {request.GetType()} is not supported.");
            return null;
        }

        /***************************************************/
    }
}

