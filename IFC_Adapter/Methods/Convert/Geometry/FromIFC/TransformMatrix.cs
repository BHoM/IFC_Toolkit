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

using BH.oM.Geometry;
using Xbim.Common.Geometry;

namespace BH.Adapter.IFC
{
    public static partial class Convert
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static TransformMatrix TransformMatrixFromIfc(this XbimMatrix3D matrix)
        {
            double[] dbls = matrix.ToDoubleArray();
            TransformMatrix result = new TransformMatrix();

            result.Matrix[0, 0] = dbls[0];
            result.Matrix[1, 0] = dbls[1];
            result.Matrix[2, 0] = dbls[2];
            result.Matrix[3, 0] = dbls[3];
            result.Matrix[0, 1] = dbls[4];
            result.Matrix[1, 1] = dbls[5];
            result.Matrix[2, 1] = dbls[6];
            result.Matrix[3, 1] = dbls[7];
            result.Matrix[0, 2] = dbls[8];
            result.Matrix[1, 2] = dbls[9];
            result.Matrix[2, 2] = dbls[10];
            result.Matrix[3, 2] = dbls[11];
            result.Matrix[0, 3] = dbls[12];
            result.Matrix[1, 3] = dbls[13];
            result.Matrix[2, 3] = dbls[14];
            result.Matrix[3, 3] = dbls[15];

            return result;
        }

        /***************************************************/
    }
}





