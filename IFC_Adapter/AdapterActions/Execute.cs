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

using BH.oM.Adapter;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;

namespace BH.Adapter.IFC
{
    public partial class IfcAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        [MultiOutput(0, "success", "List of booleans indicating whether the command succeeded for the individual items.")]
        [MultiOutput(1, "globalSuccess", "Bool indicating whether the command succeded for all the provided inputs.")]
        public override oM.Base.Output<List<object>, bool> Execute(IExecuteCommand command, ActionConfig actionConfig = null)
        {
            BH.Engine.Base.Compute.RecordError("Execute action is not implemented in the IFC_Toolkit");
            return null;
        }

        /***************************************************/
    }
}




