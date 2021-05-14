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
using BH.oM.Adapter;
using BH.oM.Adapters.IFC;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common.Geometry;
using Xbim.Ifc2x3.Interfaces;
using Xbim.ModelGeometry.Scene;

namespace BH.Adapter.IFC
{
    public partial class IFCAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(IRequest request, ActionConfig actionConfig = null)
        {
            IFCPullConfig config = actionConfig as IFCPullConfig;
            if (config == null)
            {
                config = new IFCPullConfig();
                BH.Engine.Reflection.Compute.RecordNote("Config has not been specified, default config is used.");
            }

            IFCSettings settings = this.IFCSettings.DefaultIfNull();

            Discipline? requestDiscipline = request.Discipline(config.Discipline);
            if (requestDiscipline == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Conflicting disciplines have been detected.");
                return new List<IBHoMObject>();
            }

            Discipline discipline = requestDiscipline.Value;

            List<XbimShapeInstance> shapeInstances = null;
            Xbim3DModelContext context = null;

            if (config.PullMeshes)
            {
                context = new Xbim3DModelContext(m_LoadedModel);
                context.CreateContext();
                shapeInstances = context.ShapeInstances().ToList();
            }

            List<IBHoMObject> result = new List<IBHoMObject>();
            foreach (var entity in m_LoadedModel.IIFCEntities(request))  
            {
                IIfcElement element = entity as IIfcElement;
                if (element == null)
                    continue;

                IEnumerable<IBHoMObject> converted = element.IFromIFC(discipline, settings);

                if (shapeInstances != null && context != null)
                {
                    List<Mesh> meshes = shapeInstances.Where(x => x.IfcProductLabel == element.EntityLabel).SelectMany(x => x.Meshes(context)).ToList();
                    IFCRepresentation representation = new IFCRepresentation(meshes);

                    foreach (IBHoMObject obj in converted)
                    {
                        obj.Fragments.Add(representation);
                    }
                }

                result.AddRange(converted);
            }

            return result;
        }

        /***************************************************/
    }
}


