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

using BH.Engine.Geometry;
using BH.oM.Adapters.IFC;
using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.StructuralElementsDomain;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.ModelGeometry.Scene;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        public static List<Mesh> Meshes(this XbimShapeInstance instance, Xbim3DModelContext context)
        {
            List<Mesh> result;

            //Instance's geometry
            XbimShapeGeometry geometry = context.ShapeGeometry(instance);
            byte[] data = ((IXbimShapeGeometryData)geometry).ShapeData;
            
            //If you want to get all the faces and trinagulation use this
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    XbimShapeTriangulation shape = reader.ReadShapeTriangulation();
                    result = shape.Meshes();
                }
            }

            TransformMatrix transform = instance.Transformation.FromIFC();
            return result.Select(x => x.Transform(transform)).ToList();
        }

        /***************************************************/

        public static List<Mesh> Meshes(this XbimShapeTriangulation shape)
        {
            List<Mesh> result = new List<Mesh>();
            List<Point> allVertices = shape.Vertices.Select(x => x.FromIFC()).ToList();

            foreach (XbimFaceTriangulation subMesh in shape.Faces)
            {
                Mesh mesh = new Mesh();
                List<int> uniqueIds = subMesh.Indices.Distinct().ToList();
                mesh.Vertices = uniqueIds.Select(i => allVertices[i]).ToList();
                
                for (int i = 0; i < subMesh.TriangleCount; i++)
                {
                    mesh.Faces.Add(new Face { A = uniqueIds.IndexOf(subMesh.Indices[3 * i]), B = uniqueIds.IndexOf(subMesh.Indices[3 * i + 1]), C = uniqueIds.IndexOf(subMesh.Indices[3 * i + 2]) });
                }

                result.Add(mesh);
            }

            return result;
        }

        /***************************************************/
    }
}


